using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using UnityEngine.Rendering;
using TMPro;
//葉っぱたち

namespace money
{
public class Pieces : DrawMeshInstancedBase {

    private PieceData[] _data;

    [SerializeField] private Transform[] _clippingPlanes;
    
    private int _index = 0;
    private Vector4[] _randoms;

    [SerializeField] private TMPro.TextMeshPro _text;
    [SerializeField] private bool _isYen = true;
    void Start(){

        _propertyBlock = new MaterialPropertyBlock();
        _matrices = new Matrix4x4[_count];
        _data = new PieceData[_count];
        _colors = new Vector4[_count];
        _randoms = new Vector4[_count];

        for (int i = 0; i < _count; i++)
        {
            _matrices[i] = Matrix4x4.identity;
            _data[i] = new PieceData();//
            _data[i].rot = Quaternion.Euler(
                Random.value * 360f,
                Random.value * 360f,
                Random.value
            );
            _randoms[i] = _data[i].random;
            _colors[i] = new Vector4(
                1f,
                1f,
                1f,
                1f
            );
            _data[i].Reset(transform.localToWorldMatrix);

        }

        _mat.SetVectorArray("_Random", _randoms);
        _propertyBlock.SetVectorArray("_Color", _colors);
        
        _Loop();
    }

    void _Loop(){

        _data[_index % _data.Length].Reset(transform.localToWorldMatrix);
        _index++;

        if(_isYen){
            _text.text = "¥" + _index * 1000;
        }else{
            _text.text = "$" + _index * 10000000000000;
        }

        Invoke("_Loop",0.1f);
    }

    void Update(){

        //_____clipping

        Vector4[] planes = new Vector4[_clippingPlanes.Length];
        Matrix4x4 viewMatrix = Camera.main.worldToCameraMatrix;
        for (int i = 0; i < planes.Length; i++)
        {
            Vector3 viewUp = viewMatrix.MultiplyVector(_clippingPlanes[i].up);
            Vector3 viewPos = viewMatrix.MultiplyPoint(_clippingPlanes[i].position);
            float distance = Vector3.Dot(viewUp, viewPos);
            planes[i] = new Vector4(viewUp.x, viewUp.y, viewUp.z, distance);
        }
        _mat.SetVectorArray("_ClippingPlanes", planes);
        

        //_____update data

        for (int i = 0; i < _count; i++)
        {
            _data[i].Update();
            _matrices[i].SetTRS( 
                _data[i].pos,
                _data[i].rot,
                _data[i].scale
            );
            //_matrices[i] = transform.localToWorldMatrix * _matrices[i];

            _matrices[i] = _data[i].parentMatrix * _matrices[i];
        }

        Graphics.DrawMeshInstanced(
                _mesh, 
                0, 
                _mat, 
                _matrices, 
                _count, 
                _propertyBlock, 
                ShadowCastingMode.Off, 
                false, 
                gameObject.layer
        );

    }

}

}