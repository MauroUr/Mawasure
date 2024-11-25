using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalData))]
public class ConditionalDataDrawer : PropertyDrawer
{
    private string[] strategyOptions = new string[] { "Single","Multiple", "Chain", "AOE" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty strategyProp = property.FindPropertyRelative("strategy");

        int selectedIndex = 0;

        if (strategyProp.managedReferenceValue is SingleStrategy)
            selectedIndex = 0;
        else if (strategyProp.managedReferenceValue is MultipleStrategy)
            selectedIndex = 1;
        else if (strategyProp.managedReferenceValue is ChainStrategy)
            selectedIndex = 2;
        else if (strategyProp.managedReferenceValue is AOEStrategy)
            selectedIndex = 3;


        selectedIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                                        "Casting Type",
                                        selectedIndex,
                                        strategyOptions);

        if (selectedIndex == 0)
            strategyProp.managedReferenceValue = new SingleStrategy();
        else if (selectedIndex == 1)
            strategyProp.managedReferenceValue = new MultipleStrategy();
        else if (selectedIndex == 2)
            strategyProp.managedReferenceValue = new ChainStrategy();
        else if (selectedIndex == 3)
            strategyProp.managedReferenceValue = new AOEStrategy();

        if (strategyProp.managedReferenceValue != null)
        {
            if (strategyProp.managedReferenceValue is ChainStrategy chainStrategy)
            {
                chainStrategy.maxEnemiesAffected = EditorGUI.IntField(
                    new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight),
                    "Max Enemies Affected",
                    chainStrategy.maxEnemiesAffected);

                chainStrategy.rangeBetweenEnemies = EditorGUI.FloatField(
                    new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, EditorGUIUtility.singleLineHeight),
                    "Range Between Enemies",
                    chainStrategy.rangeBetweenEnemies);
            }
            else if (strategyProp.managedReferenceValue is AOEStrategy aoeStrategy)
            {
                aoeStrategy.length = EditorGUI.FloatField(
                    new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight),
                    "Length",
                    aoeStrategy.length);

                aoeStrategy.width = EditorGUI.FloatField(
                    new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, EditorGUIUtility.singleLineHeight),
                    "Width",
                    aoeStrategy.width);
            }
        }

        EditorGUI.EndProperty();
    }
}
