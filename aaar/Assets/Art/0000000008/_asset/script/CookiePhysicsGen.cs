using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookiePhysicsGen : MonoBehaviour {

	[SerializeField] private CookiePhysics _physics;
	[SerializeField] private GameObject _camBlocker;
	[SerializeField] private GameObject _maskCube;
	[SerializeField] private GameObject _shadowPlane;

	private GameObject _container;
	private int _count = 0;

	void Start () {
		
		//Physics.gravity = new Vector3();
		//_Gen();
		_physics.gameObject.SetActive(false);
		_container = new GameObject("CookieContainer");
		Invoke("_Gen",0.5f);
	}
	
	public void SetSize(bool isAR){

		if(isAR){
			_shadowPlane.transform.localScale = new Vector3(0.026f,1f,0.016f);
			_maskCube.transform.localScale = new Vector3(0.26f,1f,0.16f);
		}else{
			//なにもしない
		}

	}

	void _Gen(){
		
		//調整する
		
		var scale = transform.lossyScale;

		var g = Instantiate( _physics, _container.transform, false );//, transform, false );
		g.transform.rotation = transform.rotation;
		g.transform.position = transform.position　- transform.up * 0.1f;
		g.transform.localScale = scale;

		_count++;
		if(_count%10==7){
			var s = scale * ( Random.value*0.5f + 1.5f );
			g.transform.localScale = s;
		}
		
		//g.transform.SetParent(transform);
		
		g.gameObject.SetActive(true);
		//g.transform.localPosition = new Vector3(0,-0.05f,0);
		
		//戻る
		
		g.Init( transform );

		Invoke("_Gen", 1f);
	}

	// Update is called once per frame
	void Update () {
		
		_camBlocker.transform.position = Camera.main.transform.position;
		_camBlocker.transform.rotation = Camera.main.transform.rotation;
	}

	void OnDestroy(){
		Destroy(_container);
	}
}
