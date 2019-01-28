using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

	[SerializeField] private Vector3 _rotV;

	// Use this for initialization
	void Start () {
		
		_rotV.x = 6f * ( Random.value - 0.5f );
		_rotV.y = 6f * ( Random.value - 0.5f );
		_rotV.z = 6f * ( Random.value - 0.5f );

	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(_rotV);

	}
}
