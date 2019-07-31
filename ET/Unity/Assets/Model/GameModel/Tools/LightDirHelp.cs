using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightDirHelp : MonoBehaviour
{
    public float w;
    public Light light1;
    private Material mat;

    // Use this for initialization
    void Start()
    {
        mat = GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (light1 == null) return;

        SetParam(-1 * light1.transform.forward.x, light1.transform.forward.y, light1.transform.forward.z);
    }

    private void SetParam(float x, float y, float z)
    {
        if (mat == null) return;
        Vector4 vector4 = new Vector4(x, y, z, w);
        mat.SetVector("_Light1Dir", vector4);
    }
}
