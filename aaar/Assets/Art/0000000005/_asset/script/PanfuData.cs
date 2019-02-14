using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PanfuData {

    public Vector3 pos = new Vector3(0,0,0);
    public Quaternion rot =Quaternion.Euler(0,0,0);
    public Vector3 scale = new Vector3(0.1f,0.1f,0.1f);
    public Vector4 uv = new Vector4();
    public Vector4 rand = new Vector4();

    private int _count = 0;

    public bool enable = false;
    public float ratio = -1f;
    public Matrix4x4 parentMatrix;

    private float _amp = 0;
    private float _rad = 0;
    public float startRad = 0;
    private float _limitY = 0;
    private float _targetAmp = 0;
    private float _flyingOffset = 0;

    public void Reset(Transform t, float startRatio, float radiusRatio){

        //ratio=-1f;
        ratio = startRatio;

        enable = true;
        parentMatrix = t.localToWorldMatrix;
        _rad = startRad;
        _limitY     = 1f + Random.value;
        _targetAmp  = radiusRatio * ( 0.15f + 0.65f * Random.value );
        pos = Vector3.zero;
        _amp=0;
    }


    public void Update(){

        if( enable ){
            
            if(ratio>0){
                _rad += 1.5f * Time.deltaTime;
            }else{
                //_rad += 1f * (1f + ratio)* Time.deltaTime;
            }

            //pos.y += 0.3f * Time.deltaTime;
            pos.y += 0.2f * Time.deltaTime;
            ratio += 0.5f * Time.deltaTime;


            if( pos.y > _limitY){
                pos.y = _limitY;
            }

            pos.x = 2f * _amp * Mathf.Cos( -_rad );
            pos.z = 2f * _amp * Mathf.Sin( -_rad );

            rot = Quaternion.Euler(0,_rad*Mathf.Rad2Deg-90f,0);

            scale.Set(1f,1f,1f);
            

            if(ratio>1f){
                ratio = 1f;
            }

            if(ratio>0){
                _amp += 0.1f * Time.deltaTime;//ratio * _targetAmp;
            }

            if(_amp > _targetAmp){
                _amp = _targetAmp;
            }

            if(_amp<0){
                _amp = 0;
            }
            //ato choto
        }else{
            scale.Set(0,0,0);
        }

    }

}