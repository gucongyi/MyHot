using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Company
{
	[CustomEditor(typeof(Transform))]

	public class TransformInspector : Editor {

	    static Vector3 clipBoardPos = new Vector3();
	    static Vector3 clipBoardRot = new Vector3();
	    static Vector3 clipBoardSca = new Vector3(); 

	    public override void OnInspectorGUI()
	    {
	        Transform trans = target as Transform;
	        //EditorGUIUtility.LookLikeControls(15f);
			EditorGUIUtility.labelWidth = 15f;

	        DrawContent(trans);
	        
	    }

	    void DrawContent(Transform trans)
	    {
	        Vector3 position = new Vector3();
	        Vector3 rotation = new Vector3();
	        Vector3 scale = new Vector3();



	        EditorGUILayout.BeginHorizontal();
	        {
	            if (DrawButton("P", "Reset Position", IsResetPositionValid(trans), 20f))
	            {
	                Undo.RecordObject(trans, "Reset Position");

	                trans.localPosition = Vector3.zero;

	            }
	            position = DrawValue(trans.localPosition);
	        }
	        EditorGUILayout.EndHorizontal();

	        EditorGUILayout.BeginHorizontal();
	        {
	            if (DrawButton("R", "Reset Rotation", IsResetRotationValid(trans), 20f))
	            {
	                Undo.RecordObject(trans, "Reset Position");

	                trans.localEulerAngles = Vector3.zero;
	            }
	            rotation = DrawValue(trans.localEulerAngles);
	        }
	        EditorGUILayout.EndHorizontal();

	        EditorGUILayout.BeginHorizontal();
	        {
	            if (DrawButton("S", "Reset Scale", IsResetScaleValid(trans), 20f))
	            {
	                Undo.RecordObject(trans, "Reset Position");

	                trans.localScale = Vector3.one;
	            }
	            scale = DrawValue(trans.localScale);
	        }
	        EditorGUILayout.EndHorizontal();

	        if (GUI.changed)
	        {
	            Undo.RecordObject(trans, "Value Changed");

	            trans.localPosition = Validate(position);
	            trans.localEulerAngles = Validate(rotation);
	            trans.localScale = Validate(scale);
	        }

	        EditorGUILayout.BeginHorizontal();
	        {
	            GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
	            if (GUILayout.Button("Copy Local"))
	            {
	                clipBoardPos = position;

	                clipBoardRot = rotation;

	                clipBoardSca = scale;
	            }


	            GUI.color = new Color(0.5f, 0.75f, 1f, 1f);
	            if (GUILayout.Button("Paste Local"))
	            {
	                Undo.RecordObject(trans, "Paste Local");
	                trans.localPosition = clipBoardPos;
	                trans.localEulerAngles = clipBoardRot;
	                trans.localScale = clipBoardSca;
	            }
	        }
	        EditorGUILayout.EndHorizontal();

	        EditorGUILayout.BeginHorizontal();
	        {
	            GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
	            if (GUILayout.Button("Paste Position"))
	            {
	                Undo.RecordObject(trans, "Paste Position");
	                trans.localPosition = clipBoardPos;
	            }

	            GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
	            if (GUILayout.Button("Paste Rotation"))
	            {
	                Undo.RecordObject(trans, "Paste Rotation");
	                trans.localEulerAngles = clipBoardRot;
	            }


	            GUI.color = new Color(0.5f, 0.75f, 1f, 1f);
	            if (GUILayout.Button("Paste Scale"))
	            {
	                Undo.RecordObject(trans, "Paste Scale");
	                trans.localScale = clipBoardSca;
	            }
	        }
	        EditorGUILayout.EndHorizontal();
	    }

	    bool DrawButton(string title, string tooltip, bool enabled, float width)
	    {
	        if (enabled)
	        {
	            GUI.color = Color.green;

	            bool isClick = GUILayout.Button(new GUIContent(title,tooltip),GUILayout.Width(width));

	            return isClick;
	        }
	        else
	        {
	            GUI.color = Color.red;

	            GUILayout.Button(new GUIContent(title,tooltip),GUILayout.Width(width));

	            return false;
	        }
	    }

	    bool IsResetPositionValid(Transform target)
	    {
	        Vector3 v3 = target.localPosition;

	        return (v3.x != 0f || v3.y != 0f || v3.z != 0f);
	    }

	    bool IsResetRotationValid(Transform target)
	    {
	        Vector3 v3 = target.localEulerAngles;

	        return (v3.x != 0f || v3.y != 0f || v3.z != 0f);
	    }

	    bool IsResetScaleValid(Transform target)
	    {
	        Vector3 v3 = target.localScale;

	        return (v3.x != 1f || v3.y != 1f || v3.z != 1f);
	    }

	    Vector3 DrawValue(Vector3 value)
	    {
	        GUILayoutOption opt = GUILayout.MinWidth(30f);

	        GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
	        value.x = EditorGUILayout.FloatField("X", value.x, opt);
	        GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
	        value.y = EditorGUILayout.FloatField("Y", value.y, opt);
	        GUI.color = new Color(0.5f, 0.75f, 1f, 1f);
	        value.z = EditorGUILayout.FloatField("Z", value.z, opt);

	        return value;
	    }

	    Vector3 Validate(Vector3 v3)
	    {
	        v3.x = float.IsNaN(v3.x) ? 0f : v3.x;
	        v3.y = float.IsNaN(v3.x) ? 0f : v3.y;
	        v3.z = float.IsNaN(v3.x) ? 0f : v3.z;

	        return v3;
	    }
	}
}