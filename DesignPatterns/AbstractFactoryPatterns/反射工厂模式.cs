using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactoryPatterns
{
    class 反射工厂模式
    {
        /// <summary>
        /// 测试反射工厂模式
        /// </summary>
        private static void TestReflectFactory()
        {
            Console.WriteLine("反射工厂模式：");
            var productB = ReflectFactory.Create("FactoryPattern.ConcreateCarB");
            //productB.GetInfo();
            Console.ReadLine();
        }
    }

    /// <summary>
    /// 反射工厂模式
    /// 是针对简单工厂模式的一种改进
    /// </summary>
    public static class ReflectFactory
    {
        public static AbstractCar Create(string typeName)
        {
            Type type = Type.GetType(typeName, true, true);
            var instance = type?.Assembly.CreateInstance(typeName) as AbstractCar;
            return instance;
        }
    }
}
