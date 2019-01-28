using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

	[SerializeField] private Vector3 position;

	private Vector3 basePosition;
	
	// Use this for initialization
	void Start () {
		basePosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		var p = new Vector3(
			basePosition.x,
			basePosition.y + 0.02f * Mathf.Sin(Time.realtimeSinceStartup),
			basePosition.z
		);
		transform.localPosition = p;
	}
	
}
