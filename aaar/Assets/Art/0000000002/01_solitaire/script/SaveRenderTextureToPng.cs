using UnityEngine;
using System.Collections;
using System.IO;

public class SaveRenderTextureToPng : MonoBehaviour {

    public RenderTexture RenderTextureRef;
    private int _index = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Space) ){
			Debug.Log("save!");
        	savePng();
		}
    }

    void savePng()
    {

        Texture2D tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height, TextureFormat.RGB24, false);
        RenderTexture.active = RenderTextureRef;
        tex.ReadPixels(new Rect(0, 0, RenderTextureRef.width, RenderTextureRef.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        //Write to a file in the project folder
        _index++;
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen" +_index+  ".png", bytes);

    }


}