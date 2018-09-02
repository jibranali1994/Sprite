using System;
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
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    private float jumpSpeed = 15f;
    private bool check = false;
    public bool shouldJump = false;
    public bool force = false;
    private Text textbox;
    private Rigidbody2D playerBody;
    public GameObject jumpAnimation;


    void OnCollisionEnter2D(Collision2D coll)
    {

        var x = Mathf.Round(animator.GetFloat("x"));
        var y = Mathf.Round(animator.GetFloat("y"));
        

        switch (coll.gameObject.tag)
        {
            case "enemy":
                if (x > 0 && y == 0)
                    Destruct(coll.gameObject);
                else
                    Destroy(this.gameObject);

                break;
            case "enemyUp":
                if (x > 0 && y > 0)
                    Destruct(coll.gameObject);
                else
                    Destroy(this.gameObject);
                break;





        }

       

        
        

    }


    void Destruct(GameObject obj)
    {
        var objControl = obj.GetComponent<enemy_move>();
        objControl.Die();

    }

    void Start()
    {
        //dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        textbox = text.GetComponent<Text>();
        playerBody = this.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
       

#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0)) // user is touching the screen with a single touch
        {

            
            fp = Input.mousePosition; // get the touch
          
            animator.SetBool("sword", true);


        }


        if (Input.GetMouseButtonUp(0)) //check if the finger is removed from the screen
            {
                lp = Input.mousePosition;
              //  Debug.Log("lp" + lp);
                check = true;
                // animator.SetBool("sword", false);
            }

        check = check ? SwipeSet(fp, lp) : false; 




#endif


        if (Input.touchCount > 0 ) // user is touching the screen with a single touch
        {
            
            
            Touch touch = Input.GetTouch(0); // get the touch


            if (  touch.phase==TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    animator.SetBool("sword", true);    
                }

           
            if ( touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;    
                    check = true;
                }

            check = check ? SwipeSet(fp, lp) : false; 

        }


    }

    bool SwipeSet (Vector2 startPos, Vector2 endPos)
    {

        var deltaX = endPos.x - startPos.x;
        var deltaY = endPos.y - startPos.y;
        Vector2 delta = new Vector2();
        delta.Set(deltaX, deltaY);
        delta.Normalize();
       // Debug.Log(delta.x);

        animator.SetFloat("x", delta.x);
        animator.SetFloat("y", delta.y);
        if (delta.y > 0.5f)
            shouldJump = true;

        //textbox.text = delta.y.ToString();
       
        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(x => {

            animator.SetFloat("x", 0);
            animator.SetFloat("y", 0);
        });
        return false;

    }
    private void FixedUpdate()
    {
        // move
        /*
        if (force)
        {
            rb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
            Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(x => {
               
                force = false;
            });

        }*/


        if (playerBody.velocity.magnitude < moveSpeed)
            rb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        

        // jump
        if(shouldJump) {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpAnimation.SetActive(true);
            shouldJump = false;
        }
    }


   
   
}
	

