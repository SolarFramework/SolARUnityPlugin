using System;
using UnityEngine;
using XPCF.Properties;

namespace SolAR
{
    public static class PropertyExtensions
    {
        public static bool CanRead(this IProperty.AccessSpecifier specifier) { return specifier != IProperty.AccessSpecifier.IProperty_OUT; }
        public static bool CanWrite(this IProperty.AccessSpecifier specifier) { return specifier != IProperty.AccessSpecifier.IProperty_IN; }

        public static object Get(this IProperty property, uint itemIndex = 0)
        {
            switch (property.getType())
            {
                case IProperty.PropertyType.IProperty_CHARSTR:
                    return property.getStringValue(itemIndex);
                case IProperty.PropertyType.IProperty_DOUBLE:
                    return property.getDoubleValue(itemIndex);
                case IProperty.PropertyType.IProperty_FLOAT:
                    return property.getFloatingValue(itemIndex);
                case IProperty.PropertyType.IProperty_INTEGER:
                    return property.getIntegerValue(itemIndex);
                case IProperty.PropertyType.IProperty_LONG:
                    return property.getLongValue(itemIndex);
                case IProperty.PropertyType.IProperty_STRUCTURE:
                    return property.getStructureValue(itemIndex);
                case IProperty.PropertyType.IProperty_UINTEGER:
                    return property.getUnsignedIntegerValue(itemIndex);
                case IProperty.PropertyType.IProperty_ULONG:
                    return property.getUnsignedLongValue(itemIndex);
                case IProperty.PropertyType.IProperty_UNICODESTR:
                    return property.getUnicodeStringValue(itemIndex);
                default:
                    throw new System.Exception();
            }
        }

        public static void Set(this IProperty property, object value, uint itemIndex = 0)
        {
            switch (property.getType())
            {
                case IProperty.PropertyType.IProperty_CHARSTR:
                    property.setStringValue((string)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_DOUBLE:
                    property.setDoubleValue((double)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_FLOAT:
                    property.setFloatingValue((float)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_INTEGER:
                    property.setIntegerValue((int)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_LONG:
                    property.setLongValue((long)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_STRUCTURE:
                    property.setStructureValue((IPropertyMap)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_UINTEGER:
                    property.setUnsignedIntegerValue((uint)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_ULONG:
                    property.setUnsignedLongValue((ulong)value, itemIndex);
                    break;
                case IProperty.PropertyType.IProperty_UNICODESTR:
                    property.setUnicodeStringValue((string)value, itemIndex);
                    break;
                default:
                    throw new Exception();
            }
        }

        public static Type ToCSharp(this IProperty.PropertyType propertyType)
        {
            switch (propertyType)
            {
                case IProperty.PropertyType.IProperty_CHARSTR:
                case IProperty.PropertyType.IProperty_UNICODESTR:
                    return typeof(string);
                case IProperty.PropertyType.IProperty_DOUBLE:
                    return typeof(double);
                case IProperty.PropertyType.IProperty_FLOAT:
                    return typeof(float);
                case IProperty.PropertyType.IProperty_INTEGER:
                    return typeof(int);
                case IProperty.PropertyType.IProperty_LONG:
                    return typeof(long);
                case IProperty.PropertyType.IProperty_STRUCTURE:
                    return typeof(IPropertyMap);
                case IProperty.PropertyType.IProperty_UINTEGER:
                    return typeof(uint);
                case IProperty.PropertyType.IProperty_ULONG:
                    return typeof(ulong);
                default:
                    throw new Exception();
            }
        }

        public static object Default(this IProperty.PropertyType propertyType)
        {
            switch (propertyType)
            {
                case IProperty.PropertyType.IProperty_CHARSTR:
                case IProperty.PropertyType.IProperty_UNICODESTR:
                    return default(string);
                case IProperty.PropertyType.IProperty_DOUBLE:
                    return default(double);
                case IProperty.PropertyType.IProperty_FLOAT:
                    return default(float);
                case IProperty.PropertyType.IProperty_INTEGER:
                    return default(int);
                case IProperty.PropertyType.IProperty_LONG:
                    return default(long);
                case IProperty.PropertyType.IProperty_STRUCTURE:
                    return default(IPropertyMap);
                case IProperty.PropertyType.IProperty_UINTEGER:
                    return default(uint);
                case IProperty.PropertyType.IProperty_ULONG:
                    return default(ulong);
                default:
                    throw new Exception();
            }
        }

        public static object OnGUI(this IProperty.PropertyType propertyType, object value)
        {
            switch (propertyType)
            {
                case IProperty.PropertyType.IProperty_CHARSTR:
                case IProperty.PropertyType.IProperty_UNICODESTR:
                    return GUILayout.TextField((string)value);
                case IProperty.PropertyType.IProperty_DOUBLE:
                    {
                        var v = (double)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (double.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                case IProperty.PropertyType.IProperty_FLOAT:
                    {
                        var v = (float)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (float.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                case IProperty.PropertyType.IProperty_INTEGER:
                    {
                        var v = (int)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (int.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                case IProperty.PropertyType.IProperty_LONG:
                    {
                        var v = (long)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (long.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                case IProperty.PropertyType.IProperty_STRUCTURE:
                    return default(IPropertyMap);
                case IProperty.PropertyType.IProperty_UINTEGER:
                    {
                        var v = (uint)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (uint.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                case IProperty.PropertyType.IProperty_ULONG:
                    {
                        var v = (ulong)value;
                        using (GUIScope.ChangeCheck)
                        {
                            var text = GUILayout.TextField(value.ToString());
                            if (GUI.changed)
                            {
                                if (ulong.TryParse(text, out v))
                                {
                                    value = v;
                                }
                            }
                        }
                    }
                    return value;
                default:
                    GUILayout.TextField(value.ToString());
                    return value;
            }
        }
    }
}
