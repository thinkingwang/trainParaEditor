using System;
using System.Reflection;

namespace Service.Dto
{
    public static class DeepCopyHelper
    {
        public static object Copy(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            object targetDeepCopyObj;
            var targetType = obj.GetType();
            //值类型  
            if (targetType.IsValueType)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型   
            else
            {
                targetDeepCopyObj = Activator.CreateInstance(targetType); //创建引用对象   
                var memberCollection = obj.GetType().GetMembers();

                foreach (var member in memberCollection)
                {
                    if (member.MemberType == MemberTypes.Field)
                    {
                        var field = (FieldInfo) member;
                        var fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }
                    }
                    else if (member.MemberType == MemberTypes.Property)
                    {
                        var myProperty = (PropertyInfo) member;
                        var info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            var propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                            }
                        }
                    }
                }
            }
            return targetDeepCopyObj;
        }
    }
}