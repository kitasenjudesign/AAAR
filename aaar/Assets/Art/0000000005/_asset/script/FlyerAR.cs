using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAR : MonoBehaviour {

	[SerializeField] private Panfus _panfus;

	// Use this for initialization
	void Start () {
		
		_panfus.Init(250,-0.1f,0.5f,false);

	}
	
}
