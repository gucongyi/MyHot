using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIUpdateTip
    {
        public Action OnConfirm;
        public Action OnCancel;

        private Transform transform;
        private Image image_Bg_Di_1;
        private Image image_BgTitle_2;
        private Text text_TextTitle_3;
        private Image image_ButtonClose;
        private Button button_ButtonClose;
        private Image image_BgCenter1_4;
        private Image image_ButtonLeftCancel;
        private Button button_ButtonLeftCancel;
        private Text text_TextLeftCancel_6;
        private Image image_ButtonRightOK;
        private Button button_ButtonRightOK;
        private Text text_TextRightOK_7;
        private Text text_info;
        private GameObject go_m_BgCenter1;

        private void InitUI()
        {
            //assign transform by your ui framework
            //transform = ; 
            image_Bg_Di_1 = transform.Find("ComBgRoot/Bg_Di_1").GetComponent<Image>();
            image_BgTitle_2 = transform.Find("ComBgRoot/BgTitle_2").GetComponent<Image>();
            text_TextTitle_3 = transform.Find("TextTitle_3").GetComponent<Text>();
            image_ButtonClose = transform.Find("ButtonClose").GetComponent<Image>();
            button_ButtonClose = transform.Find("ButtonClose").GetComponent<Button>();
            image_BgCenter1_4 = transform.Find("m_BgCenter1/BgCenter1_4").GetComponent<Image>();
            image_ButtonLeftCancel = transform.Find("ButtonLeftCancel").GetComponent<Image>();
            button_ButtonLeftCancel = transform.Find("ButtonLeftCancel").GetComponent<Button>();
            text_TextLeftCancel_6 = transform.Find("ButtonLeftCancel/TextLeftCancel_6").GetComponent<Text>();
            image_ButtonRightOK = transform.Find("ButtonRightOK").GetComponent<Image>();
            button_ButtonRightOK = transform.Find("ButtonRightOK").GetComponent<Button>();
            text_TextRightOK_7 = transform.Find("ButtonRightOK/TextRightOK_7").GetComponent<Text>();
            text_info = transform.Find("info").GetComponent<Text>();
            go_m_BgCenter1 = transform.Find("m_BgCenter1").gameObject;
        }

        public void SetInfo(string info)
        {
            text_info.text = info;
        }
        private void AddEvent()
        {
            button_ButtonLeftCancel.onClick.RemoveAllListeners();
            button_ButtonLeftCancel.onClick.AddListener(OnButtonLeftCancelClicked);
            button_ButtonRightOK.onClick.RemoveAllListeners();
            button_ButtonRightOK.onClick.AddListener(OnButtonRightOKClicked);
        }
        private void OnButtonLeftCancelClicked()
        {
            OnCancel?.Invoke();
        }
        private void OnButtonRightOKClicked()
        {
            OnConfirm?.Invoke();
        }

        public UIUpdateTip(Transform trans)
        {
            this.transform = trans;
            InitUI();
            AddEvent();
            trans.gameObject.SetActive(true);

        }
    }
}
