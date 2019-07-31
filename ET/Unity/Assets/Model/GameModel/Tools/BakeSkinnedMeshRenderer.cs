using System.Collections.Generic;
using UnityEngine;

public class BakeSkinnedMeshRenderer : MonoBehaviour
{
    /*
    private Mesh mesh;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public string tag;
    public List<MeshFilter> meshFilterList = new List<MeshFilter>();

    [ContextMenu("填充需要bake的列表")]
    public void FillListWithTag()
    {
        if (string.IsNullOrEmpty(tag) || tag == gameObject.tag)
        {
            Debug.LogError("tag错误");
            return;
        }
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        if (gameObjects == null || gameObjects.Length <= 0)
        {
            return;
        }
        meshFilterList.Clear();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            MeshFilter meshFilter = gameObjects[i].GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                continue;
            }
            meshFilterList.Add(meshFilter);
        }
    }

    // Use this for initialization
    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        mesh = new Mesh();
    }

    // Update is called once per frame
    private void Update()
    {
        skinnedMeshRenderer.BakeMesh(mesh);
        for (int i = 0; i < meshFilterList.Count; i++)
        {
            meshFilterList[i].mesh = mesh;
        }
    }

    */
    private Mesh mesh;
    public Animator animator;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public string targetTag;
    public List<MeshFilter> meshFilterList = new List<MeshFilter>();
    private List<Mesh> meshList = new List<Mesh>();
    private int meshListIndex = 0;
    private float animatorLength;
    private float animatorFrameRate;
    private float animatorPassTime;

    [ContextMenu("填充需要bake的列表")]
    public void FillListWithTag()
    {
        if (string.IsNullOrEmpty(targetTag) || targetTag == gameObject.tag)
        {
            Debug.LogError("tag错误");
            return;
        }
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(targetTag);
        if (gameObjects == null || gameObjects.Length <= 0)
        {
            return;
        }
        meshFilterList.Clear();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            MeshFilter meshFilter = gameObjects[i].GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                continue;
            }
            meshFilterList.Add(meshFilter);
        }
    }

    // Use this for initialization
    private void Start()
    {
        //var clipinfo = animator.GetCurrentAnimatorClipInfo(0);
        //animatorLength = clipinfo[0].clip.length;
        //float temp = 1 / clipinfo[0].clip.frameRate;
        //animatorFrameRate = clipinfo[0].clip.frameRate;
        //float frameCount = animatorLength * animatorFrameRate;
        //while (true)
        //{
        //    if (frameCount < 0)
        //    {
        //        break;
        //    }
        //    Mesh mesh = new Mesh();
        //    skinnedMeshRenderer.BakeMesh(mesh);
        //    meshList.Add(mesh);
        //    frameCount--;
        //    animator.Update(temp);
        //}
        //skinnedMeshRenderer.enabled = false;
        mesh = new Mesh();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        //for (int i = 0; i < meshFilterList.Count; i++)
        //{
        //    meshFilterList[i].mesh = meshList[meshListIndex];
        //}
        //animatorPassTime += Time.deltaTime;
        //if (animatorPassTime >= animatorLength)
        //{
        //    animatorPassTime = 0;
        //}
        //meshListIndex = UnityEngine.Mathf.FloorToInt(animatorPassTime * animatorFrameRate);
        skinnedMeshRenderer.BakeMesh(mesh);
        for (int i = 0; i < meshFilterList.Count; i++)
        {
            meshFilterList[i].mesh = mesh;
        }
    }
}