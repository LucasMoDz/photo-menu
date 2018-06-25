using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NotEditableOnInspector))]
public class VarNotEditableOnInspector : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.AnimationCurve:
                break;
            case SerializedPropertyType.ArraySize:
                EditorGUI.LabelField(position, label, new GUIContent(property.arraySize.ToString()));
                break;
            case SerializedPropertyType.Boolean:
                EditorGUI.LabelField(position, label, new GUIContent((Texture)Resources.Load("TextureInspector/" + (property.boolValue ? "CheckBoxTrue" : "CheckBoxFalse"))));
                break;
            case SerializedPropertyType.Bounds:
                break;
            case SerializedPropertyType.Character:
                break;
            case SerializedPropertyType.Color:
                break;
            case SerializedPropertyType.Enum:
                EditorGUI.LabelField(position, label, new GUIContent(property.enumDisplayNames[property.enumValueIndex]));
                break;
            case SerializedPropertyType.Float:
                EditorGUI.LabelField(position, label, new GUIContent(property.floatValue.ToString()));
                break;
            case SerializedPropertyType.Generic:
                break;
            case SerializedPropertyType.Gradient:
                break;
            case SerializedPropertyType.Integer:
                EditorGUI.LabelField(position, label, new GUIContent(property.intValue.ToString()));
                break;
            case SerializedPropertyType.LayerMask:
                break;
            case SerializedPropertyType.ObjectReference:
                break;
            case SerializedPropertyType.Quaternion:
                EditorGUI.LabelField(position, label, new GUIContent(property.quaternionValue.ToString()));
                break;
            case SerializedPropertyType.Rect:
                break;
            case SerializedPropertyType.String:
                EditorGUI.LabelField(position, label, new GUIContent(property.stringValue));
                break;
            case SerializedPropertyType.Vector2:
                EditorGUI.LabelField(position, label, new GUIContent(property.vector2Value.ToString()));
                break;
            case SerializedPropertyType.Vector3:
                EditorGUI.LabelField(position, label, new GUIContent(property.vector3Value.ToString()));
                break;
            case SerializedPropertyType.Vector4:
                EditorGUI.LabelField(position, label, new GUIContent(property.vector4Value.ToString()));
                break;
        }
    }
}