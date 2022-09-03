using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DevourDev.Base.Reflections
{
    public static class ReflextionHelpers
    {
        private const int _getFieldFlags = 36; // BindingFlags.Instance | BindingFlags.NonPublic
        public static FieldInfo FI(this object target, string fieldName)
        {
            if (target is Type)
                throw new Exception("target is TYPE");

            return target.GetType().GetField(fieldName, (BindingFlags)_getFieldFlags);
        }

        public static FieldInfo FI(this Type type, string fieldName)
        {
            return type.GetField(fieldName, (BindingFlags)_getFieldFlags);
        }

        public static void SetField(this object target, string fieldName, object value)
        {
            var field = target.FI(fieldName);
            field.SetValue(target, value);
        }

        public static T GetFieldValue<T>(this object target, string fieldName)
        {
            var fi = target.FI(fieldName);

            if (fi == null)
                return default!;

            var fv = fi.GetValue(target);

            return fv is T t ? t : default;
        }

        public static T FullCopy<T>(this T comp, T other)
        {
            Type type = comp.GetType();
            Type othersType = other.GetType();

            if (type != othersType)
            {
                throw new Exception($"The type \"{type.AssemblyQualifiedName}\" of \"{comp}\" does not " +
                    $"match the type \"{othersType.AssemblyQualifiedName}\" of \"{other}\"!");
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
            PropertyInfo[] pinfos = type.GetProperties(flags);

            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other));
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            FieldInfo[] finfos = type.GetFields(flags);

            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp;
        }

    }
}
