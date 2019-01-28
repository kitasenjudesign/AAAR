using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerPlay : MonoBehaviour {

	[SerializeField] private Panfus _panfus;

	// Use this for initialization
	void Awake () {
		
		_panfus.Init(600,0f,0.7f);


	}
	
}
