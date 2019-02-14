using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerPlay : MonoBehaviour {

	[SerializeField] private Panfus _panfus;

	// Use this for initialization
	void Awake () {
		
		_panfus.Init(300,-0.1f,0.7f,true);


	}
	
}
