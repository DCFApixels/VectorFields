#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
using PlasticPipe;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

namespace DCFApixels.DataMath.Unity.Editors
{
    internal abstract class VectorFieldDrawerBase<TAttribute> : PropertyDrawer
    {
        private bool _error = false;
        private int _fieldCount = 0;

        private bool _isInit;
        private bool _isStructFieldType;

        private static GUIContent _tmpLabel;
        protected static GUIContent TmpLabel
        {
            get
            {
                if (_tmpLabel == null) { _tmpLabel = new GUIContent(); }
                return _tmpLabel;
            }
        }



        protected float Line => EditorGUIUtility.singleLineHeight;
        protected float Space => EditorGUIUtility.standardVerticalSpacing;

        protected bool Error => _error;
        protected int FieldCount => _fieldCount;

        protected abstract bool IsHideDefaultDraw { get; }

        protected bool IsAttribute
        {
            get { return attribute != null; }
        }
        protected TAttribute Attribute
        {
            get
            {
                object atr = attribute;
                return UnsafeUtility.As<object, TAttribute>(ref atr);
            }
        }

        private void Init(FieldInfo fieldInfo, SerializedProperty property)
        {
            if (_isInit) { return; }
            _isStructFieldType = fieldInfo.FieldType.IsValueType;
            if (_isStructFieldType)
            {
                (_error, _fieldCount) = CountFields(property);
            }
            OnInit(fieldInfo, property);
            _isInit = true;
        }
        protected virtual void OnInit(FieldInfo fieldInfo, SerializedProperty property) { }

        private (bool error, int fieldCount) CountFields(SerializedProperty property)
        {
            (bool error, int fieldCount) result = default;
            var propertyClone = property.Copy();
            bool x = true;
            int depth = propertyClone.depth;
            result.fieldCount = 0;
            result.error = false;
            while (propertyClone.Next(x))
            {
                if (propertyClone.depth <= depth) { break; }
                switch (propertyClone.propertyType)
                {
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.Boolean:
                    case SerializedPropertyType.Float:
                    case SerializedPropertyType.String:
                    case SerializedPropertyType.Color:
                    case SerializedPropertyType.ObjectReference:
                    case SerializedPropertyType.LayerMask:
                    case SerializedPropertyType.Enum:
                    case SerializedPropertyType.Gradient:
                    case SerializedPropertyType.Quaternion:
                    case SerializedPropertyType.AnimationCurve:
                        result.error |= false;
                        break;

                    case SerializedPropertyType.Bounds:
                    case SerializedPropertyType.ExposedReference:
                    case SerializedPropertyType.FixedBufferSize:
                    case SerializedPropertyType.Vector2Int:
                    case SerializedPropertyType.Vector3Int:
                    case SerializedPropertyType.RectInt:
                    case SerializedPropertyType.BoundsInt:
                    case SerializedPropertyType.ManagedReference:
                    case SerializedPropertyType.Hash128:
                    case SerializedPropertyType.RenderingLayerMask:
                    case SerializedPropertyType.Vector2:
                    case SerializedPropertyType.Vector3:
                    case SerializedPropertyType.Vector4:
                    case SerializedPropertyType.Rect:
                    case SerializedPropertyType.Generic:
                    case SerializedPropertyType.ArraySize:
                    case SerializedPropertyType.Character:
                    default:
                        result.error |= true;
                        break;
                }
                result.fieldCount++;
                x = false;
            }

            return result;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(fieldInfo, property);
            if (_isStructFieldType)
            {
                (_error, _fieldCount) = CountFields(property);
            }

            if (IsHideDefaultDraw || _error)
            {
                return Line;
            }
            return property.isExpanded ? _fieldCount * (Space + Line) + Line : Line;
        }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float defWidth = EditorGUIUtility.labelWidth;
            int defind = EditorGUI.indentLevel;

            SerializedProperty lineProperty = property.Copy();

            Rect rect = position;
            rect.xMin += defWidth;
            rect.yMax = rect.yMin + Line;

            label.text = property.displayName;
            label.tooltip = property.tooltip;

            if (IsHideDefaultDraw)
            {
                EditorGUI.LabelField(position, label);
            }
            else
            {
                position.height = Line;
                //EditorGUI.PropertyField(position, property, label, true);

                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

                if (property.isExpanded)
                {
                    bool x = true;
                    int depth = property.depth;
                    while (property.Next(x))
                    {
                        if (property.depth <= depth) { break; }

                        position.y += Line + Space;
                        label.text = property.displayName;
                        label.tooltip = property.tooltip;
                        EditorGUI.PropertyField(position, property, label, true);
                        x = false;
                    }
                }
            }

            DrawLine(rect, lineProperty, label);

            EditorGUIUtility.labelWidth = defWidth;
            EditorGUI.indentLevel = defind;
        }

        protected abstract void DrawLine(Rect position, SerializedProperty property, GUIContent label);

        #region RW Float
        protected float ReadPropFloat(SerializedProperty _property)
        {
            switch (_property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return _property.longValue;
                case SerializedPropertyType.Boolean:
                    return _property.boolValue ? 1 : 0;
                case SerializedPropertyType.Float:
                    return _property.floatValue;
                case SerializedPropertyType.String:
                    return float.Parse(_property.stringValue);
                case SerializedPropertyType.LayerMask:
                    return _property.intValue;
                case SerializedPropertyType.Enum:
                    return _property.enumValueFlag;
                default:
                    return 0f;
            }
        }
        protected void WritePropFloat(SerializedProperty _property, float _v)
        {
            switch (_property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    _property.longValue = (long)_v;
                    return;
                case SerializedPropertyType.Boolean:
                    _property.boolValue = _v != 0;
                    return;
                case SerializedPropertyType.Float:
                    _property.floatValue = _v;
                    return;
                case SerializedPropertyType.String:
                    _property.stringValue = _v.ToString();
                    return;
                case SerializedPropertyType.LayerMask:
                    _property.intValue = (int)_v;
                    return;
                case SerializedPropertyType.Enum:
                    _property.enumValueFlag = (int)_v;
                    return;
                default:
                    return;
            }
        }
        #endregion
    }
}
#endif