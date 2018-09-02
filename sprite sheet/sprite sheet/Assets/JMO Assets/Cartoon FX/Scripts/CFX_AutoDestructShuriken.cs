using UnityEngine;
using System.Collections;
using UniRx;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;
	
	void OnEnable()
	{
        Observable.Timer(TimeSpan.FromMilliseconds(800)).Subscribe(x => {
            if (OnlyDeactivate)
            {

                this.gameObject.SetActive(false);
            }
            else
                GameObject.Destroy(this.gameObject);
        });
    }
	
	
	
}
