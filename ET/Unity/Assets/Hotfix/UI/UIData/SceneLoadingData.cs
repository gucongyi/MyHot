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

public class SceneLoadingData
{

	#region UI Variable Statement 
	public Transform transform; 
	public RawImage rawimage_ChangeScendLoadingBg_Texture_1; 
	public Image image_ProgressBackward_2; 
	public Image image_ProgressForward_3; 
	public GameObject go_m_LoadingProgress; 
	#endregion 

	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		rawimage_ChangeScendLoadingBg_Texture_1 = transform.Find("ChangeScendLoadingBg_Texture_1").GetComponent<RawImage>(); 
		image_ProgressBackward_2 = transform.Find("m_LoadingProgress/ProgressBackward_2").GetComponent<Image>(); 
		image_ProgressForward_3 = transform.Find("m_LoadingProgress/ProgressForward_3").GetComponent<Image>(); 
		go_m_LoadingProgress = transform.Find("m_LoadingProgress").gameObject; 

	}
	#endregion 

}
