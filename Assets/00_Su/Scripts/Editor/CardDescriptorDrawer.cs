using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CardDescriptor), useForChildren: true)]
public class CardDescriptorDrawer : PropertyDrawer
{
    private static List<Type> _types;

    private static List<Type> GetTypes() =>
        _types ??= AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && typeof(CardDescriptor).IsAssignableFrom(t))
            .ToList();

    private List<Type> GetFilteredTypes(SerializedProperty property)
    {
        var excluded = fieldInfo?.GetCustomAttributes(typeof(ExcludeDescriptorAttribute), true)
            .Cast<ExcludeDescriptorAttribute>()
            .SelectMany(a => a.Excluded)
            .ToHashSet();
        return GetTypes().Where(t => excluded == null || !excluded.Contains(t)).ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        if (property.managedReferenceValue == null) return height;

        var copy = property.Copy();
        var end  = property.GetEndProperty();
        if (!copy.NextVisible(true)) return height;
        while (!SerializedProperty.EqualContents(copy, end))
        {
            height += EditorGUI.GetPropertyHeight(copy, true) + EditorGUIUtility.standardVerticalSpacing;
            if (!copy.NextVisible(false)) break;
        }
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        string current = property.managedReferenceValue?.GetType().Name ?? "— 选择类型 —";

        if (EditorGUI.DropdownButton(typeRect, new GUIContent(current), FocusType.Keyboard))
        {
            var menu = new GenericMenu();
            foreach (var t in GetFilteredTypes(property))
            {
                var captured = t;
                bool selected = property.managedReferenceValue?.GetType() == t;
                menu.AddItem(new GUIContent(t.Name), selected, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(captured);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        if (property.managedReferenceValue == null)
        {
            EditorGUI.EndProperty();
            return;
        }

        float y    = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        var copy   = property.Copy();
        var end    = property.GetEndProperty();
        if (!copy.NextVisible(true)) { EditorGUI.EndProperty(); return; }

        EditorGUI.indentLevel++;
        while (!SerializedProperty.EqualContents(copy, end))
        {
            float h    = EditorGUI.GetPropertyHeight(copy, true);
            var field  = new Rect(position.x, y, position.width, h);
            EditorGUI.PropertyField(field, copy, true);
            y += h + EditorGUIUtility.standardVerticalSpacing;
            if (!copy.NextVisible(false)) break;
        }
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}
