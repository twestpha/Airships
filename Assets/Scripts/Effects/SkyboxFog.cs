//
// KinoFog - Deferred fog effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SkyboxFog : MonoBehaviour
{

    public bool useRadialDistance;
    public bool fadeToSkybox;
    public float density = 0.01f;
    public float fogStartDistance = 0.0f;
    public float fogEndDistantace = 1000.0f;

    public enum SkyboxFogMode {
        Linear,
        Exponential,
        ExponentialSquared,
    };

    public SkyboxFogMode mode;

    public Shader shader;
    private Material material;

    void OnEnable(){
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        if (material == null){
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
        }

        float coeff = 0.0f;

        switch(mode){
        case SkyboxFogMode.Linear:
            var start = fogStartDistance;
            var end = fogEndDistantace;
            var invDiff = 1.0f / Mathf.Max(end - start, 1.0e-6f);
            material.SetFloat("_LinearGrad", -invDiff);
            material.SetFloat("_LinearOffs", end * invDiff);
            material.DisableKeyword("FOG_EXP");
            material.DisableKeyword("FOG_EXP2");
            break;
        case SkyboxFogMode.Exponential:
            coeff = 1.4426950408f; // 1/ln(2)
            material.SetFloat("_Density", coeff * density);
            material.EnableKeyword("FOG_EXP");
            material.DisableKeyword("FOG_EXP2");
            break;
        case SkyboxFogMode.ExponentialSquared:
            coeff = 1.2011224087f; // 1/sqrt(ln(2))
            material.SetFloat("_Density", coeff * density);
            material.DisableKeyword("FOG_EXP");
            material.EnableKeyword("FOG_EXP2");
            break;
        default:
            break;
        }

        if (useRadialDistance) {
            material.EnableKeyword("RADIAL_DIST");
        } else {
            material.DisableKeyword("RADIAL_DIST");
        }

        if (fadeToSkybox){
            material.EnableKeyword("USE_SKYBOX");
            // Transfer the skybox parameters.
            var skybox = RenderSettings.skybox;
            material.SetTexture("_SkyCubemap", skybox.GetTexture("_Tex"));
            material.SetColor("_SkyTint", skybox.GetColor("_Tint"));
            material.SetFloat("_SkyExposure", skybox.GetFloat("_Exposure"));
            material.SetFloat("_SkyRotation", skybox.GetFloat("_Rotation"));
        } else {
            material.DisableKeyword("USE_SKYBOX");
            material.SetColor("_FogColor", RenderSettings.fogColor);
        }

        // Calculate vectors towards frustum corners.
        var cam = GetComponent<Camera>();
        var camtr = cam.transform;
        var camNear = cam.nearClipPlane;
        var camFar = cam.farClipPlane;

        var tanHalfFov = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2);
        var toRight = camtr.right * camNear * tanHalfFov * cam.aspect;
        var toTop = camtr.up * camNear * tanHalfFov;

        var v_tl = camtr.forward * camNear - toRight + toTop;
        var v_tr = camtr.forward * camNear + toRight + toTop;
        var v_br = camtr.forward * camNear + toRight - toTop;
        var v_bl = camtr.forward * camNear - toRight - toTop;

        var v_s = v_tl.magnitude * camFar / camNear;

        // Draw screen quad.
        RenderTexture.active = destination;

        material.SetTexture("_MainTex", source);
        material.SetPass(0);

        GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);

                GL.MultiTexCoord2(0, 0, 0);
                GL.MultiTexCoord(1, v_bl.normalized * v_s);
                GL.Vertex3(0, 0, 0.1f);

                GL.MultiTexCoord2(0, 1, 0);
                GL.MultiTexCoord(1, v_br.normalized * v_s);
                GL.Vertex3(1, 0, 0.1f);

                GL.MultiTexCoord2(0, 1, 1);
                GL.MultiTexCoord(1, v_tr.normalized * v_s);
                GL.Vertex3(1, 1, 0.1f);

                GL.MultiTexCoord2(0, 0, 1);
                GL.MultiTexCoord(1, v_tl.normalized * v_s);
                GL.Vertex3(0, 1, 0.1f);

            GL.End();
        GL.PopMatrix();
    }
}
