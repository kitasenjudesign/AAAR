using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireData {



    public Vector3 pos = new Vector3(
        0,0,0    
    );
    public Quaternion rot = Quaternion.Euler(0,0,0);
    private Vector3 degree = Vector3.zero;
    public Vector3 scale = new Vector3(0,0,0);//0.08451f,0.08451f,0.08451f);
    //0.1f,0.1f,0.1f);

    public Vector4 random = new Vector4(
        Random.value,
        Random.value,
        Random.value,
        Random.value
    );
    public float time = Random.value * 8f;
    public float limit = 12f;
    public float ratio = 0;

    public bool isActive = false;

	public Vector3 _v = new Vector3();
	private Vector3 _a = new Vector3(0,-0.003f,0);

    //uv wo motaseru
    public Vector4 uv = new Vector4(0,0);
    public Matrix4x4 parentMatrix = Matrix4x4.identity;

    public List<Vector3> positions;

	private float _limit = -1f;

    public void Init(){

        Debug.Log("Init");

    }

    public void Reset(List<Vector3> pos){

        time = 0;
        positions = pos;
    }


    public void Update(){
        //time=0;
        time += Time.deltaTime;
        time = time % limit;
        ratio = time / limit;

        if(positions!=null){
            pos = SolitaireMotion.GetPosition(ratio,positions); //targetMotion.GetPosition(ratio);
        }

        return;



        _v += _a;
		
		pos += _v;

		if( pos.y < _limit ){
			
			pos.y = _limit;
			_v.y *= -0.97f;

		}

    }

}