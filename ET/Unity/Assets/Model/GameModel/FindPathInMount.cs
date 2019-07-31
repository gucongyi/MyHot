using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI路径,填新手引导用
/// </summary>
public class FindPathInMount : MonoBehaviour
{
    public string ComPath;

    private List<string> comPaths=new List<string>();

    private Transform tempTrans;
	// Use this for initialization
	void Start ()
	{
        var root = GameObject.Find("Global/UI").transform;
	    comPaths.Clear();
        tempTrans = transform;
	    while(true)
	    {
	        comPaths.Add(tempTrans.name);
	        if (tempTrans.parent != null)
	        {
                if (tempTrans.parent == root) break;
	            tempTrans = tempTrans.parent;
	        }
	        else
	        {
	            break;
	        }
        }
	    for (int i=comPaths.Count-1;i >= 0;i--)
	    {
	        if (string.IsNullOrEmpty(ComPath))
	        {
	            ComPath = comPaths[i];
	        }
	        else
	        {
	            ComPath = ComPath + "/" + comPaths[i];
            }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
