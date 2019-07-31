using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*五边形雷达
 * 
 * 
 *          0
 * 
 *    1            4
 * 
 *       2     3
 *   
 *   
 * 012
 * 023
 * 034
*/


public class Radar : MaskableGraphic// Graphic
{
    //雷达最大各顶点pos
    public Vector3[] vertexesMax;
    //现在现在各顶点pos
    private Vector3[] vertexesNow = new Vector3[5];
    //雷达最大百分比
    private float[] valuesMax = new float[5] { 1, 1, 1, 1, 1 };
    //雷达现在百分比
    private float[] valuesNow = new float[5];
    //雷达mesh颜色
    private Color vertexColor = new Color(12 / 255f, 229 / 255f, 255 / 255f);


    [SerializeField] private List<float> testList = new List<float>();
    [ContextMenu("Test")]
    public void Test()
    {
        UpdateDate(testList[0], testList[1], testList[2], testList[3], testList[4]);
    }



    //按顺序传入各属性百分比,顶点transform
    public void UpdateDate(float attribute1, float attribute2, float attribute3, float attribute4, float attribute5,
        Transform vertex1 = null, Transform vertex2 = null, Transform vertex3 = null, Transform vertex4 = null, Transform vertex5 = null)
    {
        valuesNow[0] = attribute1;
        valuesNow[1] = attribute2;
        valuesNow[2] = attribute3;
        valuesNow[3] = attribute4;
        valuesNow[4] = attribute5;
        Refresh();
        List<Transform> transforms = new List<Transform>() { vertex1, vertex2, vertex3, vertex4, vertex5 };
        for (int i = 0; i < vertexesMax.Length; i++)
        {
            Transform trans = transforms[i];
            if (trans == null) continue;
            trans.localPosition = vertexesNow[i];
        }
    }

    private void Refresh()
    {
        for (int i = 0; i < vertexesMax.Length; i++)
        {
            float percent = valuesNow[i] / valuesMax[i];
            percent = Mathf.Min(1, percent);
            percent = Mathf.Max(0, percent);
            vertexesNow[i] = vertexesMax[i] * percent;
        }
        SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        vh.AddVert(vertexesNow[0], vertexColor, Vector2.zero);
        vh.AddVert(vertexesNow[1], vertexColor, Vector2.zero);
        vh.AddVert(vertexesNow[2], vertexColor, Vector2.zero);
        vh.AddVert(vertexesNow[3], vertexColor, Vector2.zero);
        vh.AddVert(vertexesNow[4], vertexColor, Vector2.zero);
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(0, 3, 4);
    }
}