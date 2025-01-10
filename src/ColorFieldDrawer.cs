#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Editors
{
    [CustomPropertyDrawer(typeof(ColorFieldAttribute))]
    internal class ColorFieldDrawer : VectorFieldDrawerBase<ColorFieldAttribute>
    {
        protected override bool IsHideDefaultDraw => !Attribute.IsShowDefaultDraw;
        protected override void DrawLine(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Error && Count > 4)
            {
                EditorGUI.LabelField(position, "ERROR");
                return;
            }
            EditorGUIUtility.labelWidth = 0f;

            bool x = true;
            int depth = property.depth;
            Color color = new Color(0, 0, 0, 1);

            var propertyClone = property.Copy();
            var i = 0;
            while (property.Next(x))
            {
                if (property.depth <= depth || i >= 4) { break; }

                color[i] = ReadPropFloat(property);

                x = false;
                i++;
            }

            EditorGUI.BeginChangeCheck();
            color = EditorGUI.ColorField(position, color);

            if (EditorGUI.EndChangeCheck())
            {
                x = true;
                depth = propertyClone.depth;
                i = 0;
                while (propertyClone.Next(x))
                {
                    if (propertyClone.depth <= depth || i >= 4) { break; }

                    WritePropFloat(propertyClone, color[i]);

                    x = false;
                    i++;
                }
                propertyClone.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif