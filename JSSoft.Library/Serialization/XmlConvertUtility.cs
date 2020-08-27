//Released under the MIT License.
//
//Copyright (c) 2018 Ntreev Soft co., Ltd.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
//rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
//persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the 
//Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Xml;

namespace JSSoft.Library.Serialization
{
    public static class XmlConvertUtility
    {
        public static bool ToBoolean(string textValue)
        {
            return XmlConvert.ToBoolean(textValue);
        }

        public static Single ToSingle(string textValue)
        {
            return XmlConvert.ToSingle(textValue);
        }

        public static double ToDouble(string textValue)
        {
            return XmlConvert.ToDouble(textValue);
        }

        public static byte ToByte(string textValue)
        {
            return XmlConvert.ToByte(textValue);
        }

        public static sbyte ToSByte(string textValue)
        {
            return XmlConvert.ToSByte(textValue);
        }

        public static short ToInt16(string textValue)
        {
            return XmlConvert.ToInt16(textValue);
        }

        public static ushort ToUInt16(string textValue)
        {
            return XmlConvert.ToUInt16(textValue);
        }

        public static int ToInt32(string textValue)
        {
            return XmlConvert.ToInt32(textValue);
        }

        public static uint ToUInt32(string textValue)
        {
            return XmlConvert.ToUInt32(textValue);
        }

        public static long ToInt64(string textValue)
        {
            return XmlConvert.ToInt64(textValue);
        }

        public static ulong ToUInt64(string textValue)
        {
            return XmlConvert.ToUInt64(textValue);
        }

        public static DateTime ToDateTime(string textValue, XmlDateTimeSerializationMode mode)
        {
            try
            {
                return XmlConvert.ToDateTime(textValue, mode);
            }
            catch (Exception)
            {
                return DateTime.Parse(textValue);
            }
        }

        public static TimeSpan ToTimeSpan(string textValue)
        {
            return XmlConvert.ToTimeSpan(textValue);
        }

        public static bool IsBaseType(Type type)
        {
            if (type.IsEnum == true)
                return true;
            else if (type == typeof(bool))
                return true;
            else if (type == typeof(float))
                return true;
            else if (type == typeof(double))
                return true;
            else if (type == typeof(byte))
                return true;
            else if (type == typeof(sbyte))
                return true;
            else if (type == typeof(short))
                return true;
            else if (type == typeof(ushort))
                return true;
            else if (type == typeof(int))
                return true;
            else if (type == typeof(uint))
                return true;
            else if (type == typeof(long))
                return true;
            else if (type == typeof(ulong))
                return true;
            else if (type == typeof(DateTime))
                return true;
            else if (type == typeof(TimeSpan))
                return true;
            else if (type == typeof(Guid))
                return true;
            else if (type == typeof(string))
                return true;
            return false;
        }

        public static Guid ToGuid(string textValue)
        {
            return XmlConvert.ToGuid(textValue);
        }

        public static string ToString(bool value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(float value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(double value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(byte value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(sbyte value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(short value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(ushort value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(int value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(uint value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(long value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(ulong value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Utc);
            else if (value.Kind == DateTimeKind.Local)
                return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Local);
            return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Unspecified);
        }

        public static string ToString(TimeSpan value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(Guid value)
        {
            return XmlConvert.ToString(value);
        }

        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value.GetType().IsEnum == true)
                return value.ToString();
            else if (value is bool boolean)
                return ToString(boolean);
            else if (value is float single)
                return ToString(single);
            else if (value is double @double)
                return ToString(@double);
            else if (value is byte byte1)
                return ToString(byte1);
            else if (value is sbyte @byte)
                return ToString(@byte);
            else if (value is short int5)
                return ToString(int5);
            else if (value is ushort int4)
                return ToString(int4);
            else if (value is int int3)
                return ToString(int3);
            else if (value is uint int2)
                return ToString(int2);
            else if (value is long int1)
                return ToString(int1);
            else if (value is ulong @int)
                return ToString(@int);
            else if (value is DateTime time)
                return ToString(time);
            else if (value is TimeSpan span)
                return ToString(span);
            else if (value is Guid guid)
                return ToString(guid);
            else if (value is string)
                return value as string;

            throw new NotSupportedException();
        }

        public static string ToString(object value, Type dataType)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.GetType() != dataType)
            {
                var converter = TypeDescriptor.GetConverter(dataType);
                if (converter.CanConvertFrom(value.GetType()) == false)
                    throw new NotSupportedException();
                value = converter.ConvertFrom(value);
            }
            return ToString(value);
        }

        public static object ToValue(string textValue, Type dataType)
        {
            if (textValue == null)
                throw new ArgumentNullException(nameof(textValue));
            if (dataType.IsEnum == true)
                return Enum.Parse(dataType, textValue);
            else if (dataType == typeof(bool))
                return ToBoolean(textValue);
            else if (dataType == typeof(float))
                return ToSingle(textValue);
            else if (dataType == typeof(double))
                return ToDouble(textValue);
            else if (dataType == typeof(byte))
                return ToByte(textValue);
            else if (dataType == typeof(sbyte))
                return ToSByte(textValue);
            else if (dataType == typeof(short))
                return ToInt16(textValue);
            else if (dataType == typeof(ushort))
                return ToUInt16(textValue);
            else if (dataType == typeof(int))
                return ToInt32(textValue);
            else if (dataType == typeof(uint))
                return ToUInt32(textValue);
            else if (dataType == typeof(long))
                return ToInt64(textValue);
            else if (dataType == typeof(ulong))
                return ToUInt64(textValue);
            else if (dataType == typeof(DateTime))
                return ToDateTime(textValue, XmlDateTimeSerializationMode.Unspecified);
            else if (dataType == typeof(TimeSpan))
                return ToTimeSpan(textValue);
            else if (dataType == typeof(Guid))
                return ToGuid(textValue);
            else if (dataType == typeof(string))
                return textValue;

            throw new NotSupportedException();
        }
    }
}
