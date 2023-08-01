using UnityEditor;

[CustomEditor(typeof(OxygenController))]
public class OxygenControllerEditor : Editor
{
    SerializedProperty useOxygenLimitProperty;
    SerializedProperty minOxygenLimitProperty;

    private void OnEnable()
    {
        useOxygenLimitProperty = serializedObject.FindProperty("useOxygenLimit");
        minOxygenLimitProperty = serializedObject.FindProperty("minOxygenLimit");
    }

    private void OnDisable()
    {
        useOxygenLimitProperty = null;
        minOxygenLimitProperty = null;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (useOxygenLimitProperty.boolValue)
        {
            DrawMinOxygenLimitProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawMinOxygenLimitProperties()
    {
        minOxygenLimitProperty.floatValue = EditorGUILayout.DelayedFloatField("Min Oxygen Limit", minOxygenLimitProperty.floatValue);
    }
}
