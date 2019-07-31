using ETModel;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// ui黑底显示处理系统.
    /// </summary>
    [ETModel.ObjectSystem]
    public class UiBgMaskAwakeSystem : AwakeSystem<UI, GameObject>
    {
        public override void Awake(UI self, GameObject gameObject)
        {
            //ZLog.Error($"UiBgMaskSystem Awake {gameObject.name}");
            var canvasConfig = gameObject.GetComponent<CanvasConfig>();
            if (canvasConfig == null) return;
            var blacks = FindAllBlack(gameObject.transform)?.ToArray();
            if (blacks == null) return;
            var bgMaskManager = Game.Scene.GetComponent<UIComponent>().BgMaskmanager;
            //if (bgMaskManager.IsShowedBlack)
            //{
            //    black.gameObject.SetActive(false);
            //}
            var id = bgMaskManager.AddNewBlack(gameObject, blacks);
            if (id == -1)
            {
                ZLog.Info($"不合法的ui..{gameObject.name}");
                return;
            }
            self.OnUIShowed = () => bgMaskManager.TryShowBlack(id);
            self.OnUIDisposeBefore = () => bgMaskManager.RemoveBlack(id);
            self.OnChangeBlack = () => bgMaskManager.UpdateUISelfBlack(id);
            //查找bgmask
            //var isTop  = gameObject.transform.GetSiblingIndex()== (gameObject.transform.childCount-1);

        }

        public List<Transform> FindAllBlack(Transform transform)
        {
            List<Transform> blacks = null;
            var childs = transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform item in childs)
            {
                if (item.tag == "BgBlack")
                {
                    if (blacks == null) blacks = new List<Transform>();
                    blacks.Add(item);
                    //print($"查找方法第2种 {item.name}");
                }
            }
            return blacks;
        }
    }
}