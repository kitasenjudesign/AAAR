using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Panfus : DrawMeshInstancedBase {

    private PanfuData[] _data;
    private Vector4[] _rand;
    private float[] _ratios;
    public const int MAX = 1023;
    private int _index = 0;
    private float _startRotY = 180f;
    
    private float _startRatio;
    private float _radiusRatio;

    private bool _isPlayMode;

    public void Init(int num, float startRatio, float radiusRatio, bool isPlayMode){

        Debug.Log("init");

        _isPlayMode = isPlayMode;

        _count = num;
        _startRatio = startRatio;
        _radiusRatio = radiusRatio;

        _propertyBlock = new MaterialPropertyBlock();
        _matrices = new Matrix4x4[MAX];
        _data = new PanfuData[MAX];
        _colors = new Vector4[MAX];
        _rand = new Vector4[MAX];
        _ratios= new float[MAX];

        int cnt = _isPlayMode ? 1 : 0;

        for(int i=0;i<_count + cnt;i++){
                _matrices[i] = Matrix4x4.identity;
                _data[i] = new PanfuData();
                _data[i].startRad = _startRotY * Mathf.Deg2Rad;
                _data[i].pos.x = 0;//10f * (Random.value - 0.5f );
                _data[i].pos.y = 0;//10f * (Random.value );
                _data[i].pos.z = 0;//10f * (Random.value - 0.5f );
                _ratios[i] = 0;
                _rand[i] = new Vector4(
                    Random.value,
                    Random.value,
                    Random.value,
                    Random.value
                );

                _data[i].rot = Quaternion.Euler(
                    0,//360f * ( Random.value - 0.5f ),
                    0,
                    0//360f * ( Random.value - 0.5f )
                );
                //_uvs[_count] = SpriteUV.GetUv(Mathf.FloorToInt(Random.value*6),4,4);
                _colors[i] = new Vector4(
                    Random.value,
                    Random.value,
                    Random.value,
                    1f
                );
        }
        

        _loop();
    }

    void _loop(){

       
        int index = _index % _count;
        //Debug.Log(index);
        _data[index].Reset(
            transform,_startRatio,_radiusRatio
        );
        
        _index++;
        Invoke("_loop",0.3f);
    }
    
    void OnDestroy()
    {
        //Debug.Log("OnDestroy1");
        CancelInvoke("_loop");
    }    

    void Update(){

        //Debug.Log("update");


        for (int i = 0; i < _count; i++)
        {
        
            _data[i].Update();

            _matrices[i].SetTRS( 
                _data[i].pos,
                _data[i].rot,
                _data[i].scale
            );
            _matrices[i] = _data[i].parentMatrix * _matrices[i];
            _ratios[i] = _data[i].ratio<0 ? 0 : _data[i].ratio;

        }

        //
        if(_isPlayMode){
            
            _data[_count].scale.Set(1f,1f,1f);
            _data[_count].rot = Quaternion.Euler(0,-90f,0);

            _matrices[_count].SetTRS( 
                _data[_count].pos,
                _data[_count].rot,
                _data[_count].scale
            );
            _matrices[_count] = transform.localToWorldMatrix * _matrices[_count];
            _ratios[_count] = 0;

        }


        _propertyBlock.SetVectorArray("_Color", _colors);
        _propertyBlock.SetVectorArray("_Rand", _rand);
        _propertyBlock.SetFloatArray("_Ratio", _ratios);

        int cnt = _isPlayMode ? 1 : 0;

        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, 
                _matrices, 
                _count + cnt,
                _propertyBlock, 
                ShadowCastingMode.On, 
                false, 
                gameObject.layer
        );

    }



}