using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#else
using DCFApixels.DataMath.Internal;
namespace DCFApixels.DataMath.Internal
{
    public abstract class PropertyAttribute : Attribute { }
}
#endif

namespace DCFApixels.DataMath
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class VectorFieldAttribute : PropertyAttribute
    {
        public bool IsShowDefaultDraw;
        public VectorFieldAttribute(bool isShowDefaultDraw = false)
        {
            IsShowDefaultDraw = isShowDefaultDraw;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EulerFieldAttribute : PropertyAttribute
    {
        public bool IsShowDefaultDraw;
        public EulerFieldAttribute(bool isShowDefaultDraw = false)
        {
            IsShowDefaultDraw = isShowDefaultDraw;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ColorFieldAttribute : PropertyAttribute
    {
        public bool IsShowDefaultDraw;
        public ColorFieldAttribute(bool isShowDefaultDraw = false)
        {
            IsShowDefaultDraw = isShowDefaultDraw;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ColorHSVFieldAttribute : PropertyAttribute
    {
        public bool IsShowDefaultDraw;
        public ColorHSVFieldAttribute(bool isShowDefaultDraw = false)
        {
            IsShowDefaultDraw = isShowDefaultDraw;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class Color32FieldAttribute : PropertyAttribute
    {
        public bool IsShowDefaultDraw;
        public Color32FieldAttribute(bool isShowDefaultDraw = false)
        {
            IsShowDefaultDraw = isShowDefaultDraw;
        }
    }
}