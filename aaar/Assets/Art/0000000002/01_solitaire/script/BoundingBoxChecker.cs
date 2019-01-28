using UnityEngine;
using System.Collections;

public class BoundingBoxChecker : MonoBehaviour
{

    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 center;
    [SerializeField] private Vector3 scale;

    void OnDrawGizmosSelected()
    {
        if (GetComponent<MeshFilter>()==null) return;

        var mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.RecalculateBounds();

        var bounds = mesh.bounds;

        var scl = transform.localScale;
        var sizee = bounds.size;

        var ss = new Vector3();
        ss.x = scl.x * sizee.x;
        ss.y = scl.y * sizee.y;
        ss.z = scl.z * sizee.z;

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        //Gizmos.DrawCube( bounds.center, ss );
        Gizmos.DrawCube( transform.localPosition, ss );


        //size, center, scale
        size = ss;
        center = bounds.center;
        scale = transform.localScale;
    }
    
}