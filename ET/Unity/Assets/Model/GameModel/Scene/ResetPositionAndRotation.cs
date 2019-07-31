using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Company;

public class ResetPositionAndRotation : MonoBehaviour { 
    private void LateUpdate()
    {
        var tParent = this.transform.parent;
        this.transform.parent = null;
        this.transform.position = Vector3.zero;
        this.transform.localScale = Vector3.one;
        Vector3 vector3To = tParent.TransformPoint(Vector3.forward).CopyChangeY(0);
        Vector3 vector3From = tParent.transform.position.CopyChangeY(0);
        this.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector3To - vector3From  );

        this.transform.parent = tParent;
        this.transform.localPosition = Vector3.zero;

        //注意：如果位置有问题这里可以不用摧毁，而使用每帧更新位置
        GameObject.Destroy(this);
    }
}
