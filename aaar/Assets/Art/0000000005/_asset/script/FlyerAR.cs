using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAR : MonoBehaviour {

	[SerializeField] private Panfus _panfus;

	// Use this for initialization
	void Start () {
		
		_panfus.Init(800,-0.5f,0.8f);

	}
	
}
