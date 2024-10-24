using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalData))]
public class ConditionalDataDrawer : PropertyDrawer
{
    private string[] strategyOptions = new string[] { "Single","Multiple", "Chain", "AOE" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Find the 'strategy' property which holds the polymorphic reference
        SerializedProperty strategyProp = property.FindPropertyRelative("strategy");

        // Create a dropdown for selecting the strategy type
        int selectedIndex = 0;

        // Determine the current index based on the type of the strategy
        if (strategyProp.managedReferenceValue is SingleStrategy)
            selectedIndex = 0;
        else if (strategyProp.managedReferenceValue is MultipleStrategy)
            selectedIndex = 1;
        else if (strategyProp.managedReferenceValue is ChainStrategy)
            selectedIndex = 2;
        else if (strategyProp.managedReferenceValue is AOEStrategy)
            selectedIndex = 3;

            // Show the dropdown
            selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            "Casting Type",
            selectedIndex,
            strategyOptions);

        // Set the selected strategy based on the dropdown
        if (selectedIndex == 0)
            strategyProp.managedReferenceValue = new SingleStrategy();  // Create a new ChainStrategy
        else if (selectedIndex == 1)
            strategyProp.managedReferenceValue = new MultipleStrategy();  // Create a new AOEStrategy
        else if (selectedIndex == 2)
            strategyProp.managedReferenceValue = new ChainStrategy();
        else if (selectedIndex == 3)
            strategyProp.managedReferenceValue = new AOEStrategy();

        // Draw the fields based on the selected strategy
        if (strategyProp.managedReferenceValue != null)
        {
            if (strategyProp.managedReferenceValue is ChainStrategy chainStrategy)
            {
                // Draw fields for ChainStrategy
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
                // Draw fields for AOEStrategy
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
