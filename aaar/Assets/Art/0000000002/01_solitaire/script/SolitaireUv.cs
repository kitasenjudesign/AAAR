using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SolitaireUV {

    public static Vector4 GetUvByIndex(int idx,int numX, int numY){

        int splitX = numX;
        int splitY = numY;

        int xx = idx % splitX;
        int yy = Mathf.FloorToInt( idx / splitX );

        return GetUV(xx,yy,splitX,splitY);

    }

    public static Vector4 GetUV(int idxX, int idxY, float splitX, float splitY){

        float xx = idxX * 1f/splitX;
        float yy = ((splitY-1f)-idxY) * 1f/splitY;
        return new Vector4(xx,yy,1/splitX,1/splitY);

    }


    public static Vector4 GetUV2(int idxX, int idxY){
        
        //sizeX = 512 , 71
        //sizeY = 2048 , 96
        float sizeX = 71f / 512f;
        float sizeY = 96f / 2048f;

        float xx = idxX * sizeX;
        float yy = idxY * sizeY;
        return new Vector4( xx,yy,sizeX,sizeY);

    }



    public static int IndexToX(int idx,int numX){
        return idx % numX;
    }
    public static int IndexToY(int idx,int numX){
        return Mathf.FloorToInt( idx / numX );
    }

}