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

public class WaitData
{

	#region UI Variable Statement 
	public Transform transform; 
	public Image image_UIWait; 
	public Image image_ImageWaitBg; 
	public Image image_ImageWaitFg; 
	#endregion 

	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_UIWait = transform.GetComponent<Image>(); 
		image_ImageWaitBg = transform.Find("GoWait/ImageWaitBg").GetComponent<Image>(); 
		image_ImageWaitFg = transform.Find("GoWait/ImageWaitFg").GetComponent<Image>(); 

	}
	#endregion 

}
