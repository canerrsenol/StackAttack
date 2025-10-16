using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(CircleRotator))]
public class CircleRotatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        DrawPropertiesExcluding(serializedObject, "m_Script");
        bool changed = EditorGUI.EndChangeCheck();

        serializedObject.ApplyModifiedProperties();

        if (changed)
        {
            RebuildSelectedTargets();
        }

        if (GUILayout.Button("Rebuild Targets"))
        {
            RebuildSelectedTargets();
        }

        serializedObject.Update();
        var hexaTargetProperty = serializedObject.FindProperty("hexaTarget");
        if (!hexaTargetProperty.hasMultipleDifferentValues && hexaTargetProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Assign a HexaTarget prefab before spawning targets.", MessageType.Warning);
        }
    }

    private void RebuildSelectedTargets()
    {
        foreach (Object selected in targets)
        {
            var rotator = selected as CircleRotator;
            if (rotator == null)
            {
                continue;
            }

            Undo.RegisterFullObjectHierarchyUndo(rotator.gameObject, "Rebuild Circle Targets");
            rotator.RebuildTargets();
            EditorUtility.SetDirty(rotator);

            if (rotator.gameObject.scene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(rotator.gameObject.scene);
            }
        }
    }
}
