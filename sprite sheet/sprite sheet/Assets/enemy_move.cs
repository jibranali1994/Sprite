using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class enemy_move : MonoBehaviour {

    // Use this for initialization

    Animator anim;
    void Start () {
        anim = this.GetComponent<Animator>();
	}
	
    public void Die()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = true;
        anim.SetInteger("transition", -1);
        Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(x => {

            Destroy(this.gameObject);
        });
    }
	// Update is called once per frame
	void Update () {
		
	}
}
