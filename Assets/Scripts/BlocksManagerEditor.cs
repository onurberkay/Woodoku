
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(BlocksManager))]
public class BlocksManagerEditor : Editor
{
    
    GameObject block;
    
    int width =2, height=2;
    BlocksManager blocksManager;

    public override void OnInspectorGUI()
    {
        blocksManager = (BlocksManager)target;
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage!=null && prefabStage == PrefabStageUtility.GetPrefabStage(blocksManager.gameObject))
        {
            
            if (blocksManager.block != null)
            {
                block = blocksManager.block;

            }
            width = blocksManager.width;
            height = blocksManager.height;

            block = (GameObject)EditorGUILayout.ObjectField("Prefab", block, typeof(GameObject), false);
            if (block != blocksManager.block)
            {
                blocksManager.block = block;
               // EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                EditorSceneManager.MarkSceneDirty(prefabStage.scene);

            }
            width = EditorGUILayout.IntField("Width", width);
            height = EditorGUILayout.IntField("Height", height);
            if (width != blocksManager.width || height != blocksManager.height)
            {
                blocksManager.height = height;
                blocksManager.width = width;

                blocksManager.array = new bool[width, height];
                //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                EditorSceneManager.MarkSceneDirty(prefabStage.scene);

            }
            for (int j = 0; j < height; j++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < width; i++)
                {
                    blocksManager.array[i, j] = EditorGUILayout.Toggle(blocksManager.array[i, j]);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Generate"))
            {
                blocksManager.Generate();

                //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());



                EditorSceneManager.MarkSceneDirty(prefabStage.scene);

            }

        }
    }

   
}
