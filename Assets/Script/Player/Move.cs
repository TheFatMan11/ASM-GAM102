﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float nhay = 16f;
    [SerializeField] private Animator animator;
    [SerializeField] private MeshRenderer listMaterial;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject tochToPlay;
    [SerializeField] private SettingMusicAndSound setting;
    private float tocDo = 0;
    private float tocDoMap;
    public static int jumpCount = 2;
    public static int jump;
    private float ngang = 0f;
    private RunAudio audioManager;
    public static GameManager insta;
    // Start is called before the first frame update
    void Start()
    {
        jump = jumpCount;
        tocDoMap = 100f;
        audioManager = RunAudio.instance;
        insta = gameManager;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(tocDo, rb.velocity.y);
        MapRun();
    }
    private void Update()
    {
        animator.SetFloat("Jump", rb.velocity.y);
        animator.SetBool("isChamDat", isChamDat.chamDat);
    }
    private void MapRun()
    {
        if (listMaterial==null)
        {
            return;
        }
        foreach (Material mr in listMaterial.materials)
        {
            mr.mainTextureOffset = new Vector2(transform.position.x / (tocDoMap -= 20), transform.position.y / 100);
        }
        if (tocDoMap <= 20)
        {
            tocDoMap = 100f;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        ngang = context.ReadValue<Vector2>().x;
        animator.SetBool("isRun", true);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (jump <= 0)
        {
            return;
        }
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (!gameManager.GetPlaying())
                {
                    gameManager.SetPLaying(true);
                    tocDo = 7f;
                    animator.SetBool("isRun", true);
                    if (tochToPlay != null)
                    {
                        tochToPlay.SetActive(false);
                    }

                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, nhay);
                    jump--;
                    PlayJump();
                }

                break;
            case InputActionPhase.Canceled:
                break;
        }
    }

    public float GetSpeed()
    {
        return tocDo;
    }
    public void SetSpeed(float nSpeed)
    {
        tocDo = nSpeed;
    }
    public void PlayRun()
    {
        if (audioManager == null)
        {
            Debug.Log("Bị null");
            return;
        }
        // chưa có sound chạy
    }
    public void StopRun()
    {
        if (audioManager == null)
        {
            Debug.Log("Bị null");
            return;
        }
        audioManager.StopSound();
    }
    public void PlayJump()
    {
        if (audioManager == null)
        {
            Debug.Log("Bị null");
            return;
        }
        audioManager.PlaySound("Jump");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadPoint"))
        {
            Debug.Log("chet : Bị bắn");
            if (setting == null)
            {
                return;
            }
            setting.OnMenuDead();
        }
        // cơ chế dẫm đầu quái thì quái chết
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadPoint"))
        {
            Debug.Log("chet : Bị bắn");
            if (setting == null)
            {
                return;
            }
            setting.OnMenuDead();
        }
    }
}
