
/**************************************************
* 文 件 名：ReaderToEntity.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/22 13:53:08
* 文件说明：
* 修 改 人：
* 修改日期：
* 备注描述：
*           
*************************************************/
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Climb.Data
{
    /// <summary>
    /// 动态填写实体类的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReaderToEntity<T>
    {
        private static readonly MethodInfo GetValueMethod = typeof(IDataRecord).GetMethod("get_Item", new[] { typeof(int) });

        private static readonly MethodInfo IsDbNullMethod = typeof(IDataRecord).GetMethod("IsDBNull", new[] { typeof(int) });
        private delegate T Load(IDataRecord dataRecord);
        private Load _handler;

        private ReaderToEntity() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public T Build(IDataRecord dataRecord)
        {
            return _handler(dataRecord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public static ReaderToEntity<T> CreateBuilder(IDataRecord dataRecord)
        {
            ReaderToEntity<T> dynamicBuilder = new ReaderToEntity<T>();
            DynamicMethod method = new DynamicMethod("DynamicCreate", typeof(T), new[] { typeof(IDataRecord) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(typeof(T));
            if (Type.EmptyTypes != null) generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.GetName(i));
                Label endIfLabel = generator.DefineLabel();

                if (propertyInfo == null || propertyInfo.GetSetMethod() == null) continue;
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Callvirt, IsDbNullMethod);
                generator.Emit(OpCodes.Brtrue, endIfLabel);
                generator.Emit(OpCodes.Ldloc, result);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Callvirt, GetValueMethod);
                generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                generator.MarkLabel(endIfLabel);
            }

            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            dynamicBuilder._handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}
