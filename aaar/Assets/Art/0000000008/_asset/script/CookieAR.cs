using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAR : MonoBehaviour {

	[SerializeField] private CookiePhysicsGen _gen;

	// Use this for initialization
	void Start () {
		_gen.SetSize(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
