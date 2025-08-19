#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.DataMath.Unity.Editors
{
    [CustomPropertyDrawer(typeof(Color32FieldAttribute))]
    internal class Color32FieldDrawer : VectorFieldDrawerBase<Color32FieldAttribute>
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
            if (Error && FieldCount > 4)
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

                color[i] = ReadPropFloat(property) / byte.MaxValue;

                x = false;
                i++;
            }

            EditorGUI.BeginChangeCheck();
            TmpLabel.text = "";
            color = EditorGUI.ColorField(position, TmpLabel, color, true, i == 4, false);

            if (EditorGUI.EndChangeCheck())
            {
                x = true;
                depth = propertyClone.depth;
                i = 0;
                while (propertyClone.Next(x))
                {
                    if (propertyClone.depth <= depth || i >= 4) { break; }

                    WritePropFloat(propertyClone, color[i] * byte.MaxValue);

                    x = false;
                    i++;
                }
                propertyClone.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif