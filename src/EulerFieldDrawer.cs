#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DCFApixels.Editors
{
    [CustomPropertyDrawer(typeof(EulerFieldAttribute))]
    internal class EulerFieldDrawer : VectorFieldDrawerBase<EulerFieldAttribute>
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
            Quaternion quat = default;

            var propertyClone = property.Copy();
            var i = 0;
            while (property.Next(x))
            {
                if (property.depth <= depth || i >= 4) { break; }

                quat[i] = ReadPropFloat(property);

                x = false;
                i++;
            }

            Vector3 euler = quat.eulerAngles;

            EditorGUI.BeginChangeCheck();
            euler = EditorGUI.Vector3Field(position, "", euler);

            if (EditorGUI.EndChangeCheck())
            {
                quat.eulerAngles = euler;
                x = true;
                depth = propertyClone.depth;
                i = 0;
                while (propertyClone.Next(x))
                {
                    if (propertyClone.depth <= depth || i >= 4) { break; }

                    WritePropFloat(propertyClone, quat[i]);

                    x = false;
                    i++;
                }
                propertyClone.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif