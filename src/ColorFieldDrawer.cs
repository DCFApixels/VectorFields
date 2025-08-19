#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.VectorFields.Editors
{
    [CustomPropertyDrawer(typeof(ColorFieldAttribute))]
    public class ColorFieldDrawer : VectorFieldDrawerBase<ColorFieldAttribute>
    {
        protected override bool IsHideDefaultDraw
        {
            get
            {
                if (!IsAttribute) { return true; }
                return !Attribute.IsShowDefaultDraw;
            }
        }
        private ColorUsageAttribute _colorUsageAttribute;
        protected override void OnInit(FieldInfo fieldInfo, SerializedProperty property)
        {
            _colorUsageAttribute = fieldInfo.GetCustomAttribute<ColorUsageAttribute>();
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

                color[i] = ReadPropFloat(property);

                x = false;
                i++;
            }

            bool isShowAlpha = true;
            bool IsHDR = false;
            if (_colorUsageAttribute != null)
            {
                isShowAlpha = _colorUsageAttribute.showAlpha;
                IsHDR = _colorUsageAttribute.hdr;
            }
            EditorGUI.BeginChangeCheck();
            TmpLabel.text = "";
            color = EditorGUI.ColorField(position, TmpLabel, color, true, i == 4 && isShowAlpha, IsHDR);

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