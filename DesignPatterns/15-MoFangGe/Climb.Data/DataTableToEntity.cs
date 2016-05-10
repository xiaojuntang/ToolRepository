
/**************************************************
* 文 件 名：DataTableToEntity.cs
* 文件版本：1.0
* 创 建 人：mxk
* 联系方式：QQ:84664969   Email:84664969@qq.com   Phone:18513950591
* 创建日期：2014/9/22 13:53:50
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
    /// datatable 转换实体类信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableToEntity<T>
    {
        private static readonly MethodInfo GetValueMethod = typeof(DataRow).GetMethod("get_Item", new[] { typeof(int) });
        private static readonly MethodInfo IsDbNullMethod = typeof(DataRow).GetMethod("IsNull", new[] { typeof(int) });
        private delegate T Load(DataRow dataRecord); 

        private Load _handler;
        private DataTableToEntity() { }

        public T Build(DataRow dataRecord)
        {
            return _handler(dataRecord);
        }
        public static DataTableToEntity<T> CreateBuilder(DataRow dataRecord)
        {
            DataTableToEntity<T> dynamicBuilder = new DataTableToEntity<T>();
            DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(T), new[] { typeof(DataRow) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.Table.Columns[i].ColumnName);
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, IsDbNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, GetValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder._handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}
