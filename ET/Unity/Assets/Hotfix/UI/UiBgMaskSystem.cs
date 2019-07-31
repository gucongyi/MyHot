using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 黑底只能有一个的系统
    /// </summary>
    public class UiBgMaskSystem
    {
        /// <summary>
        /// 有黑底的ui生成这个数据
        /// </summary>
        public class MaskData
        {
            public class BlackData
            {
                public Image image;
                public Color oldColor;
                public Color noShowColor;
                public Transform transform;
                public bool IsActive { get; private set; } = false;
                public void SetActive(bool active)
                {
                    IsActive = active;
                    ZLog.Info($"设置 image颜色 {active} {transform.name}");
                    var transActive = transform.gameObject.activeInHierarchy;
                    //优化显示
                    image.color = !active && transActive ? noShowColor : oldColor  ;
                }
            }
            public Transform ui;
            private List<BlackData> blackDatas = new List<BlackData>();
            public string uiName;
            public bool isHaveBack = false;

            public Transform[] Blacks {set {
                    blackDatas.Clear();
                    foreach (var item in value)
                    {
                        var image = item.GetComponent<Image>();
                        if (image == null) continue;
                        var d = new BlackData();
                        d.transform = item;
                        d.image = image;
                        var c = d.oldColor = image.color;
                        d.noShowColor = new Color(c.r,c.g,c.b,0);
                        
                        blackDatas.Add(d);
                    }
                    isHaveBack = blackDatas.Count > 0;
                } }
            /// <summary>
            /// 一个ui组件里面多个黑底的显示.
            /// </summary>
            /// <param name="active"></param>
            public void UpdateActive(bool active)
            {
                if (blackDatas != null)
                {
                    var isShowedOne = false;
                    for (int i = blackDatas.Count-1; i >= 0; i--)
                    {
                        var item = blackDatas[i];
                        if (item != null)
                        {
                            if (active==false || isShowedOne == true)
                            {
                                item.SetActive(false);
                                continue;
                            }
                            var itemActive = item.transform.gameObject.activeInHierarchy;
                            if (itemActive)
                            {
                                isShowedOne = true;
                                item.SetActive(true);
                                continue;
                            }
                            item.SetActive(false);
                        }
                    }
                }
            }
        }

        public void UpdateUISelfBlack(int id)
        {
            if (!map.ContainsKey(id)) return;
            var item = map[id];
            //刷新一下
            item.UpdateActive(item == currentBlack);
        }

        public bool IsShowedBlack => CurrentBlack != null;

        public MaskData CurrentBlack { get => currentBlack;
            set
            {
                if (currentBlack == value) return;
                currentBlack?.UpdateActive(false);
                currentBlack = value;
                currentBlack?.UpdateActive(true);
            }
        }

        /// <summary>
        /// 当前黑底
        /// </summary>
        private MaskData currentBlack;
        /// <summary>
        /// 自增id
        /// </summary>
        private int index = 0;
        private Dictionary<int, MaskData> map = new Dictionary<int, MaskData>();
        private List<MaskData> datas = new List<MaskData>();

        //public int AddNewBlack(GameObject uiGameObject, Transform black)
        //{
        //    var d = new MaskData
        //    {
        //        black = black,
        //        ui = uiGameObject.transform,
        //        uiName = uiGameObject.name,
        //    };
        //    return AddBlackData(d);
        //}
        public int AddNewBlack(GameObject uiGameObject,params Transform[]  blacks)
        {
            var d = new MaskData
            {
                Blacks = blacks,
                ui = uiGameObject.transform,
                uiName = uiGameObject.name,
            };
            return AddBlackData(d);
        }

        private int AddBlackData(MaskData d)
        {
            if (!d.isHaveBack) return -1;
            index++;
            map.Add(index, d);
            datas.Add(d);
            return index;
        }

        public void RemoveBlack(int id)
        {
            if (!map.ContainsKey(id)) return;
            var item = map[id];
            map.Remove(id);
            datas.Remove(item);
            UpdateUI();
        }
        public void TryShowBlack(int id)
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            MaskData topShowItem = null;
            //遍历找到最上面的一个
            for (int i = 0; i < datas.Count; i++)
            {
                var item = datas[i];
                if (!item.ui.gameObject.activeInHierarchy) continue;
                if (topShowItem == null)
                {
                    topShowItem = item;
                    continue;
                }
                //先判断父级
                var r = item.ui.parent.GetSiblingIndex().CompareTo(topShowItem.ui.GetSiblingIndex());
                if (r != 0)
                {
                    if (r == 1)
                    {
                        topShowItem = item;
                    }
                    continue;
                }
                
                var newIndex = item.ui.GetSiblingIndex();
                var currTopIndex = topShowItem.ui.GetSiblingIndex();
                if (newIndex > currTopIndex)
                {
                    topShowItem = item;
                    continue;
                }
                else if (newIndex < currTopIndex)
                {
                    continue;
                }

                //if (item.black.GetSiblingIndex() > topShowItem.black.GetSiblingIndex())
                //{
                //    topShowItem = item;
                //    continue;
                //}
                //else if (item.black.GetSiblingIndex() < topShowItem.black.GetSiblingIndex())
                //{
                //    continue;
                //}
                ////相等不可能
                //ZLog.Error("err");
            }
            CurrentBlack = topShowItem;
        }
    }
}