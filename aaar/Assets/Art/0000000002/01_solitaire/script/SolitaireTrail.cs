using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using UnityEngine.Rendering;
using TMPro;
//葉っぱたち

public class SolitaireTrail : DrawMeshInstancedBase {


    [SerializeField,Range(-2f,-0.1f)] private float _bottomDistance = -1f;
    [SerializeField] private float _startPosSpace = 0.076f;
    //[SerializeField] public bool isModeA = false;

    private SolitaireData[] _data;
    private int _index = 0;
    private Vector4[] _randoms;
	private SolitaireMotion[] _motions;
    private Vector4[] _uvs;// = new Vector4[MAX];


    void Start(){

        
        //モーションを10個くらいつくっとく
        //


		//motion
		int numMotion = 7;
        
		_motions = new SolitaireMotion[numMotion];
		for(int i=0;i<numMotion;i++){
			_motions[i] = new SolitaireMotion();
            _motions[i]._limit = _bottomDistance;
            _motions[i].Init(_startPosSpace);
		}

        _propertyBlock = new MaterialPropertyBlock();
        _matrices = new Matrix4x4[_count];
        _data = new SolitaireData[_count];
        _colors = new Vector4[_count];
        _randoms = new Vector4[_count];
        _uvs = new Vector4[_count];

        for (int i = 0; i < _count; i++)
        {
            _matrices[i] = Matrix4x4.identity;
            _data[i] = new SolitaireData();//
            _data[i].rot = Quaternion.Euler(
                0,0,0
            );
            _randoms[i] = _data[i].random;
            _colors[i] = new Vector4(
                1f,
                1f,
                1f,
                1f
            );
            
			_matrices[i].SetTRS( 
				_data[i].pos,
				_data[i].rot,
				_data[i].scale
        	);

            _uvs[i] = SolitaireUV.GetUV(0,0,4,13);
            //_uvs[i] = SolitaireUV.GetUV2(0,0);

        }

        _mat.SetVectorArray("_Random", _randoms);
        _propertyBlock.SetVectorArray("_Uv", _uvs);
        _propertyBlock.SetVectorArray("_Color", _colors);
        
        //_Loop();
    }


    void Update(){

        //_____update data

        if( Input.GetKeyDown( KeyCode.R )){
            //Reset();
			for(int i=0;i<_motions.Length;i++){
				_motions[i].Reset();
			}
        }

        for(int i=0;i<_motions.Length;i++){
            
            _motions[i].ResetData( _data[_index % _data.Length], transform );
            _motions[i]._limit = _bottomDistance;
            _index++;

        }

    
		
        for (int i = 0; i < _count; i++)
        {
            _data[i].Update();

            //a
            //b
            //c

            //ratio
            //int idx = i % _motions.Length;
            //_data[i].pos = _motions[idx].GetPosition( _data[i].ratio );

            _matrices[i].SetTRS( 
                _data[i].pos,
                _data[i].rot,
                _data[i].scale
            );
            //_matrices[i] = transform.localToWorldMatrix * _matrices[i];
            _matrices[i] = _data[i].parentMatrix * _matrices[i];


            _uvs[i] = _data[i].uv;
        }

        //_propertyBlock.SetVectorArray("_Uv", _uvs);
        _propertyBlock.SetVectorArray("_Uv", _uvs);

        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, 
                _matrices, 
                _count, 
                _propertyBlock, 
                ShadowCastingMode.On, 
                true,
                gameObject.layer
        );

    }

}