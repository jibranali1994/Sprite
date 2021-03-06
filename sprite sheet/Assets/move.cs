﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class move : MonoBehaviour {




    private Animator animator;
    public GameObject text;
    private Vector2 fp;   //First touch position
    private Vector2 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private float deltaX;
    private float deltaY;
    private Vector2 delta;
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    private float jumpSpeed = 15f;
    private bool check = false;
    public bool shouldJump = false;
    public bool force=true;
    private Text textbox;

    void Start()
    {
        //dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        delta = new Vector2();
        textbox = text.GetComponent<Text>();

    }

    void Update()
    {
        //animator.SetBool("sword", true);

        //List<Touch> touches = InputHelper.GetTouches();

        //Debug.Log(touches.Count);

        // animator.SetBool("sword", true);

        if (Input.touchCount > 0 ) // user is touching the screen with a single touch
        {
            
            // Touch touch = touches[0];
            Touch touch = Input.GetTouch(0); // get the touch


            if (  touch.phase==TouchPhase.Began) //check for the first touch
                {
                    //fp = touch.position;
                fp = touch.position;
                    //animator.SetBool("sword", true);
                    //lp = touch.position;
                    Debug.Log("fp" + fp);
                animator.SetBool("sword", true);
                    
                }

           
            if ( touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;
                   // check = true; //last touch position. Ommitted if you use list
                    
                    Debug.Log("lp" + lp);
                    check = true;
               // animator.SetBool("sword", false);
                }

                if (check)
                {
                    deltaX = lp.x - fp.x;
                    deltaY = lp.y - fp.y;
                    delta.Set(deltaX, deltaY);
                    delta.Normalize();
                    Debug.Log(delta.x);

                    animator.SetFloat("x", delta.x);
                    animator.SetFloat("y", delta.y);
                if (delta.y > 0.5f)
                    shouldJump = true;

                //textbox.text = delta.y.ToString();
                check = false;
                Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(x => {

                    animator.SetFloat("x", 0);
                    animator.SetFloat("y", 0);
                });
               


                }

        }


    }
    private void FixedUpdate()
    {
        // move
        if (force)
        {
            rb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(x => {
               
                force = false;
            });

        }

        // jump
        if(shouldJump) {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            shouldJump = false;
        }
    }


   
   
}
	

