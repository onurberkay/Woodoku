using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BaseManager))]
public class BaseManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BaseManager baseManager = (BaseManager)target;
        if (GUILayout.Button("Generate"))
        {
            baseManager.Generate();
            EditorUtility.SetDirty(baseManager.gameObject);
        }
    }
}
