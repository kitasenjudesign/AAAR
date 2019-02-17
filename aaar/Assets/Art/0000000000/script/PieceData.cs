using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace money
{
public class PieceData {

    public Vector3 pos = new Vector3(
        1f * ( Random.value-0.5f ),
        1f * ( Random.value-0.5f ),
        1f * ( Random.value-0.5f )      
    );
    public Quaternion rot = Quaternion.Euler(0,0,0);
    private Vector3 degree = Vector3.zero;
    public Vector3 scale = new Vector3(0.1f,0.1f,0.1f);

    public Vector4 random = new Vector4(
        Random.value,
        Random.value,
        Random.value,
        Random.value
    );
    public float time = 0;
    public float limit = 8f;

    private bool _isActive = false;

    public Matrix4x4 parentMatrix = Matrix4x4.identity;
    public void Reset(Matrix4x4 matrix){
        //初期値に戻る
        _isActive = true;
        parentMatrix = matrix;
        time = 0;
        pos = new Vector3(0,0.1f,0);
        //rot = Quaternion.Euler(0,0,0);
        degree = new Vector3(0,180f,0);//Vector3.zero;
        scale = new Vector3(0.1f,0.1f,0.1f);
    }

    public void Update(){

        if(!_isActive){
            scale = Vector3.zero;
            return;
        }

        //更新する
        time += Time.deltaTime;
        float r = time / limit;
        if( r > 1f){
            _isActive=false;
            scale = Vector3.zero;
        }

        //角度
        float rr = r * 2f;
        if(rr>2f)rr=2f;
        degree.y += rr * Mathf.Sin( Time.realtimeSinceStartup * 0.4f ) * 7f;

        rot = Quaternion.Euler(degree);

        pos.x = 0.2f * Mathf.Pow(r,4f) * Mathf.Cos( r * Mathf.PI*2f* 8f);
        pos.z = 0.2f * Mathf.Pow(r,4f) * Mathf.Sin( r * Mathf.PI*2f* 8f);

        pos.y = r * 2f;

    }

}

}