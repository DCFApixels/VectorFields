#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.VectorFields.Editors
{
    [CustomPropertyDrawer(typeof(VectorFieldAttribute))]
    public class VectorFieldDrawer : VectorFieldDrawerBase<VectorFieldAttribute>
    {
        protected override bool IsHideDefaultDraw
        {
            get
            {
                if (!IsAttribute) { return true; }
                return !Attribute.IsShowDefaultDraw;
            }
        }
        protected override void DrawLine(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Error)
            {
                EditorGUI.LabelField(position, label, "ERROR");
                return;
            }

            EditorGUIUtility.labelWidth = 12f;
            EditorGUI.indentLevel = 0;

            position.width = position.width / FieldCount;
            float width = position.width;
            position.xMin += 3f;

            bool x = true;
            int depth = property.depth;
            while (property.Next(x))
            {
                if (property.depth <= depth) { break; }

                label.text = property.displayName;
                label.tooltip = property.tooltip;
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    Color c = property.boolValue ? Color.white : Color.black;
                    c.a = 0.1f;
                    EditorGUI.DrawRect(position, c);
                }
                EditorGUI.PropertyField(position, property, label);
                position.x += width;
                x = false;
            }
        }
    }
}
#endif