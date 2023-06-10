using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    public Material camMat;

    private RenderTexture rt1;
    private RenderTexture rt2;

    private int player1ID = Shader.PropertyToID("_Player1");
    private int player2ID = Shader.PropertyToID("_Player2");
    private int cam1ID = Shader.PropertyToID("_Cam1");
    private int cam2ID = Shader.PropertyToID("_Cam2");
    void Start()
    {
        int w = Screen.width;
        int h = Screen.height;

        rt1 = new RenderTexture(w, h, 32, RenderTextureFormat.ARGB32, 0);
        rt2 = new RenderTexture(w, h, 32, RenderTextureFormat.ARGB32, 0);

        cam1.targetTexture = rt1;
        cam1.forceIntoRenderTexture = true;
        cam2.targetTexture = rt2;
        cam2.forceIntoRenderTexture = true;

        camMat.SetTexture(cam1ID, rt1);
        camMat.SetTexture(cam2ID, rt2);
    }

    void LateUpdate()
    {
        camMat.SetVector(player1ID, cam1.transform.position);
        camMat.SetVector(player2ID, cam2.transform.position);
    }
}
