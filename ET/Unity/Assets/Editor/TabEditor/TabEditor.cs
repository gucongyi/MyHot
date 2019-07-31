using UnityEngine;
using System.Collections;
using UnityEditor;
using Quick.UI;
using UnityEditor.UI;

[CustomEditor(typeof(Tab), true)]
[CanEditMultipleObjects]
public class TabEditor : ToggleEditor
{

    SerializedProperty m_PageProperty;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Tab tab = target as Tab;

        EditorGUILayout.Space();       
        tab.page = (Page)EditorGUILayout.ObjectField("Page", tab.page, typeof(Page), true);
        if (tab.page) tab.page.IsOn = tab.isOn;

        string tag = EditorGUILayout.TextField("Tag", tab.tag);
        if (tab.tag != tag)
        {
            tab.tag = tag;
        }

        EditorGUILayout.Space();

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
