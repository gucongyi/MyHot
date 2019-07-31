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
using System;

public class UIUpdateVersionTip
{
    public Action OnConfirm;
    #region UI Variable Statement 
    public Transform transform; 
	public Image image_Bg_Di_1; 
	public Image image_BgTitle_2; 
	public Text text_TextTitle_3; 
	public Image image_BgCenter1_4; 
	public Image image_ButtonRightOK; 
	public Button button_ButtonRightOK; 
	public Text text_TextRightOK_7; 
	public Text text_info; 
	public GameObject go_m_BgCenter1; 
	#endregion 

	#region UI Variable Assignment 
	public void InitUI() 
	{
		//assign transform by your ui framework
		//transform = ; 
		image_Bg_Di_1 = transform.Find("ComBgRoot/Bg_Di_1").GetComponent<Image>(); 
		image_BgTitle_2 = transform.Find("ComBgRoot/BgTitle_2").GetComponent<Image>(); 
		text_TextTitle_3 = transform.Find("TextTitle_3").GetComponent<Text>(); 
		image_BgCenter1_4 = transform.Find("m_BgCenter1/BgCenter1_4").GetComponent<Image>(); 
		image_ButtonRightOK = transform.Find("ButtonRightOK").GetComponent<Image>(); 
		button_ButtonRightOK = transform.Find("ButtonRightOK").GetComponent<Button>(); 
		text_TextRightOK_7 = transform.Find("ButtonRightOK/TextRightOK_7").GetComponent<Text>(); 
		text_info = transform.Find("info").GetComponent<Text>(); 
		go_m_BgCenter1 = transform.Find("m_BgCenter1").gameObject; 

	}
    #endregion
    private void AddEvent()
    {
        button_ButtonRightOK.onClick.RemoveAllListeners();
        button_ButtonRightOK.onClick.AddListener(OnButtonOKClicked);
    }
    private void OnButtonOKClicked()
    {
        OnConfirm?.Invoke();
    }

    public UIUpdateVersionTip(Transform trans)
    {
        this.transform = trans;
        InitUI();
        AddEvent();
        trans.gameObject.SetActive(true);
    }
}
