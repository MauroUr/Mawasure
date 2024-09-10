using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ConditionalData
{
    public enum castingStrategy
    {
        Single,
        Multiple,
        AOE,
        Chains
    }

    public castingStrategy strategy;

    public int maxEnemiesAffected;
    public float rangeBetweenEnemies;

    public float length;
    public float width;
}

[CustomPropertyDrawer(typeof(ConditionalData))]
public class ConditionalDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty selectedOptionProp = property.FindPropertyRelative("strategy");

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), selectedOptionProp, new GUIContent("Casting type"));

        if (selectedOptionProp.enumValueIndex == (int)ConditionalData.castingStrategy.Chains)
        {
            SerializedProperty maxEnemiesAffected = property.FindPropertyRelative("maxEnemiesAffected");
            SerializedProperty rangeBetweenEnemies = property.FindPropertyRelative("rangeBetweenEnemies");

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width,
                                        EditorGUIUtility.singleLineHeight), maxEnemiesAffected,
                                        new GUIContent("Max Enemies Affected"));

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width,
                                        EditorGUIUtility.singleLineHeight), rangeBetweenEnemies,
                                        new GUIContent("Range Between Enemies"));
        }
        else if (selectedOptionProp.enumValueIndex == (int)ConditionalData.castingStrategy.AOE)
        {
            SerializedProperty length = property.FindPropertyRelative("length");
            SerializedProperty width = property.FindPropertyRelative("width");

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width,
                                        EditorGUIUtility.singleLineHeight), length,
                                        new GUIContent("Length"));

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width,
                                        EditorGUIUtility.singleLineHeight), width,
                                        new GUIContent("Width"));
        }

        EditorGUI.EndProperty();
    }
}
