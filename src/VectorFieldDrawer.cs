#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Editors
{
    [CustomPropertyDrawer(typeof(VectorFieldAttribute))]
    internal class VectorFieldDrawer : VectorFieldDrawerBase<VectorFieldAttribute>
    {
        protected override bool IsHideDefaultDraw => !Attribute.IsShowDefaultDraw;
        protected override void DrawLine(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Error)
            {
                EditorGUI.LabelField(position, label, "ERROR");
                return;
            }

            EditorGUIUtility.labelWidth = 12f;
            EditorGUI.indentLevel = 0;

            position.width = position.width / Count;
            float width = position.width;
            position.xMin += 3f;

            bool x = true;
            int depth = property.depth;
            while (property.Next(x))
            {
                if (property.depth <= depth) { break; }

                label.text = property.displayName;
                label.tooltip = property.tooltip;
                EditorGUI.PropertyField(position, property, label);
                position.x += width;
                x = false;
            }
        }
    }
}
#endif