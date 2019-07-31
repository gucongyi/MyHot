//this file is auto created by QuickCode,you can edit it 
//do not need to care initialization of ui widget any more 
//------------------------------------------------------------------------------
/**
* @author :
* date    :
* purpose :
*/
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Quick.UI;

public class FrameShowData
{

	#region UI Variable Statement 
	public Transform transform; 
	public Text text_FrameCount; 
	public GameObject go_FrameCount; 
	#endregion 

	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		text_FrameCount = transform.Find("FrameCount").GetComponent<Text>(); 
		go_FrameCount = transform.Find("FrameCount").gameObject; 

	}
	#endregion 

}
