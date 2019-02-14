using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookiePlay : MonoBehaviour {


	[SerializeField] private CookiePhysicsGen _gen;
	
	// Use this for initialization
	void Start () {
		_gen.SetSize(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
