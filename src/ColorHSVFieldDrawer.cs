#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.VectorFields.Editors
{
    [CustomPropertyDrawer(typeof(ColorHSVFieldAttribute))]
    public unsafe class ColorHSVFieldDrawer : VectorFieldDrawerBase<ColorHSVFieldAttribute>
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

            float* RWBuffer = stackalloc float[4];
            for (int j = 0; j < 4; j++)
            {
                RWBuffer[j] = 0;
            }

            bool x = true;
            int depth = property.depth;

            var propertyClone = property.Copy();
            var i = 0;
            while (property.Next(x))
            {
                if (property.depth <= depth || i >= 4) { break; }

                RWBuffer[i] = ReadPropFloat(property);

                x = false;
                i++;
            }
            Color color = Color.HSVToRGB(RWBuffer[0], RWBuffer[1], RWBuffer[2]);
            color.a = RWBuffer[3];

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

                RWBuffer[3] = color.a;
                Color.RGBToHSV(color, out RWBuffer[0], out RWBuffer[1], out RWBuffer[2]);

                while (propertyClone.Next(x))
                {
                    if (propertyClone.depth <= depth || i >= 4) { break; }

                    WritePropFloat(propertyClone, RWBuffer[i]);

                    x = false;
                    i++;
                }
                propertyClone.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif