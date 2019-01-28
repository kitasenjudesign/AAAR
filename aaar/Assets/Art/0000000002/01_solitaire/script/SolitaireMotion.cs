using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireMotion {


	public Vector3 position = new Vector3();
	public Quaternion rotation;
	private Vector3 _v = new Vector3();
	private Vector4 _uv = new Vector4(0,0,1f,1f);

	private Vector3 _a = new Vector3(0,-0.003f,0);

	public float _limit = -1f;
	private int _counter = 0;
	private int _counterLimit = 100;

    public List<Vector3> _positions;

	private float _space;

	public void Init(float space){
		_space = space;
		Reset();
	}

	private int _cardIndex = 0;//ハート、スペード、など


    /**
        計算をしないように
        time: 0--1
     */
    public static Vector3 GetPosition(float ratio, List<Vector3> positions){

        int idx1 = Mathf.FloorToInt( positions.Count * ratio );
        int idx2 = Mathf.FloorToInt( positions.Count * ratio )+1;

		if(idx1<=0)idx1=0;
		if(idx2<=0)idx2=0;

        if(idx1>positions.Count-1) idx1 = positions.Count-1;
        if(idx2>positions.Count-1) idx2 = positions.Count-1;

        var r1 = (float)idx1/(positions.Count);
        var r2 = (float)idx2/(positions.Count);

		var p1 = positions[idx1];
		var p2 = positions[idx2];

		if(r1==0 && r2==0) return p1;

        float r = (ratio - r1) / (r2 - r1);

        var v = Vector3.Lerp(p1,p2,r);

        return v;

    }

    //motion kara dispatch suru



    //計算
	public void Reset(){

		//三箇所　ランダムに

		/*
		Vector3[] startPos = new Vector3[]{
			new Vector3(-0.111f,0,0),
			new Vector3(-0.038f,0,0),
			new Vector3( 0.036f,0,0),
			new Vector3( 0.111f,0,0)
		};*/
		Vector3[] startPos = new Vector3[]{
			new Vector3(-_space * 1.5f,0,0),
			new Vector3(-_space * 0.5f,0,0),
			new Vector3( _space * 0.5f,0,0),
			new Vector3( _space * 1.5f,0,0)
		};


		//0,1,2,3
		int kindOfCard = Mathf.FloorToInt(startPos.Length * Random.value);

		position = startPos[ kindOfCard ];

		_v = new Vector3();
		_v.x = Mathf.Sign(Random.value-0.5f) * (0.005f + 0.01f * Random.value);//デフォルトでどっちかに降る
		_v.y = Random.value * 0.03f;
		_v.z = -(Random.value + 0.2f) * 0.02f;
		

		_uv =  SolitaireUV.GetUV(
			kindOfCard,
			Mathf.FloorToInt(Random.value * 13f),
			4,
			13
		);

		rotation = Quaternion.Euler(0,Mathf.Atan2(_v.z,_v.x)/Mathf.PI*180f*0.5f,0);

		_counter=0;
		_counterLimit = 300;// + Mathf.FloorToInt( 100 * Random.value );
        _positions = new List<Vector3>();

        for(int i=0;i<_counterLimit;i++){
            UpdatePos();
        }

	}	

    //
    public void ResetData(SolitaireData data, Transform transform){

        //データの初期化

		data.pos = new Vector3();
        data.positions = _positions;
		data._v.x = _v.x;
		data._v.y = _v.y;
		data._v.z = _v.z;
		data.uv.x = _uv.x;
		data.uv.y = _uv.y;
		data.uv.z = _uv.z;
		data.uv.w = _uv.w;
		data.parentMatrix = transform.localToWorldMatrix;

		data.scale = SolitaireParams.BASE_SCALE;

        data.time = Random.value * Time.deltaTime;
		
		//rotation = Quaternion.Euler(0,Mathf.Atan2(_v.z,_v.x)/Mathf.PI*180f*0.5f,0);

		if(Random.value<0.02f){
            Reset();
        }
        
        //_counterLimit = 30 + Mathf.FloorToInt( 30 * Random.value );
        
    }


	public void UpdatePos(){

        _positions.Add( position );

		_v += _a;
		position += _v;

		if( position.y < _limit ){
			
			position.y = _limit;
			_v.y *= -0.95f;

		}


	}


}
