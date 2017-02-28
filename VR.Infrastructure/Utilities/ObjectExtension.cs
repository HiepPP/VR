using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VR.Infrastructure.Utilities;
namespace VR.Infrastructure.Utilities
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            return false;
        }
        public static bool IsNotNull(this object obj)
        {
            return !IsNull(obj);
        }
        public static DateTime f_CDate(this object value)
        {
            DateTime l_out;
            value = value != null ? value : "";
            DateTime.TryParse(value.ToString(), out l_out);
            return l_out;
        }
        public static int f_CInt(this object a_Value)
        {
            int l_out;
            a_Value = a_Value != null ? a_Value : 0;
            int.TryParse(a_Value.ToString(), out l_out);
            return l_out;
        }
        public static int GetNumberInString(this string stringvalue)
        {
            try
            {
                var number = Regex.Match(stringvalue, @"\d+").Value;
                return Int32.Parse(number);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static TDestination TransformTo<TDestination>(this object sourceObject, string ExcludeProperties = "") where TDestination : class
        {
            if (!sourceObject.IsNull())
            {
                if (!ExcludeProperties.StringIsNullEmptyWhiteSpace())
                {
                    ExcludeProperties = ExcludeProperties.Trim().ToLower();
                    ExcludeProperties = "[" + ExcludeProperties + "]";
                }
                var destinationObject = Activator.CreateInstance(typeof(TDestination));
                typeof(TDestination).GetProperties().AsParallel().ForAll(destProperty =>
                {
                    PropertyInfo sourceProperty = sourceObject.GetType().GetProperty(destProperty.Name);
                    try
                    {
                        if (sourceProperty != null && !ExcludeProperties.Contains("[" + sourceProperty.Name.ToLower() + "]") && destProperty.CanWrite)
                        {
                            object sourceValue = sourceProperty.GetValue(sourceObject);
                            object desValue = destProperty.GetValue(destinationObject);
                            if (!object.Equals(sourceValue, desValue))
                                destProperty.SetValue(destinationObject, sourceValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Transform <TDestination>", ex);
                    }
                });
                return (destinationObject as TDestination);
            }
            return null;
        }
    }
}
