using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleanup : MonoBehaviour {

    public float DestroyTimer;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, DestroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
