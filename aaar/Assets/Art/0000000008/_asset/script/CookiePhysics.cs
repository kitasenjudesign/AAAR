using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CookiePhysics : MonoBehaviour {


	private float _lim = 0.5f;
	private float _time = 0;

	[SerializeField] private Rigidbody _rigidBody;
	private Vector3 _gravity = new Vector3(0,-9.8f*0.1f,0);
	//private Vector3 _dir = new Vector3();
	private Transform _parent;
	// Use this for initialization
	void Start () {
		
		_rigidBody.useGravity = false;
		//_rigidBody.mass
	}
	
	public void Init(Transform parent){

		
		var v = parent.up * 10f + 3f * parent.right * (Random.value-0.5f);
		_parent = parent;
		_rigidBody.isKinematic=true;

		_rigidBody.AddForce( v );
		
		//　方向ベクトルをワールド→ローカルへと変える
		//transform.InverseTransformDirection(Vector3の値);
		//　位置をワールド→ローカルへと変える
		//transform.InverseTransformPoint (Vector3の値);
		//　方向ベクトルをローカル→ワールドへと変える
		//transform.TransformDirection(Vector3の値);
		//　位置をローカル→ワールドへと変える
		//transform.TransformPoint (Vector3の値);
	
		Invoke("_OnStart",1f);
		Invoke("_OnDelay",20f);	//消滅

	}

	void _OnStart(){

		//Debug.Log( _rigidBody.mass );

		_rigidBody.isKinematic=false;
		_rigidBody.velocity = GetVelosity() * 30f;//40f;//_parent.up * 0.4f;
		//_rigidBody.AddForce( _parent.up * 10f );
		//_rigidBody.AddForce( GetVelosity() );


		//rot
		var r = 50f * _parent.up * (Random.value-0.5f);
		r += new Vector3(
			0.4f*(Random.value-0.5f),
			0.4f*(Random.value-0.5f),
			0.4f*(Random.value-0.5f)
		);
		//r = transform.TransformDirection(r);
		//transform.tran
		//_rigidBody.angularVelocity = r;
		//_rigidBody.AddTorque( r );	

	}

	void _OnDelay(){
		Destroy(gameObject);
	}

	public Vector3 GetVelosity(){
		return _parent.up * Time.deltaTime * 0.2f;
	}

	// Update is called once per frame
	void Update () {
		
		if( _rigidBody.isKinematic ){

			//最初
			transform.position += GetVelosity();

		}else{

			//摩擦、空気抵抗
			//_rigidBody.AddTorque( -_rigidBody.angularVelocity * 0.15f );
			_rigidBody.AddForce( -_rigidBody.velocity * 0.4f );

		}

		if( Random.value < 0.004f ){

			var r = new Vector3(
				10f * (Random.value - 0.5f),
				10f * (Random.value - 0.5f),
				10f * (Random.value - 0.5f)
			);
			
			_rigidBody.angularVelocity = r;

		}else{

			_rigidBody.angularVelocity *= 0.998f;

		}

		//_rigidBody.AddForce( _gravity );


		//_rigidBody.AddTorque( -_rigidBody.angularVelocity * 0.3f );
		//transform.localPosition 
		
		/*
		_time += Time.deltaTime;

		if(_time<_lim){
			var p = transform.localPosition;
			p.y += 0.005f;
			transform.localPosition = p;
		}*/
	}
}
