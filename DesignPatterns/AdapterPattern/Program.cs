using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdapterPattern
{
    class Program
    {
        static void Main(string[] args)
        {

//适用性
//在以下各种情况下使用适配器模式：
//1．系统需要使用现有的类，而此类的接口不符合系统的需要。
//2．想要建立一个可以重复使用的类，用于与一些彼此之间没有太大关联的一些类，包括一些可能在将来引进的类一起工作。这些源类不一定有很复杂的接口。
//3．（对对象适配器而言）在设计里，需要改变多个已有子类的接口，如果使用类的适配器模式，就要针对每一个子类做一个适配器，而这不太实际。
//4.从代码角度来说， 如果需要调用的类所遵循的接口并不符合系统的要求或者说并不是客户所期望的，那么可以考虑使用适配器。
//5.从应用角度来说， 如果因为产品迁移、合作模块的变动，导致双方一致的接口产生了不一致，或者是希望在两个关联不大的类型之间建立一种关系的情况下可以考虑适配器模式。
//总结
//总之，通过运用Adapter模式，就可以充分享受进行类库迁移、类库重用所带来的乐趣。 

            DataContent dataContent = new DataContent();
            var aa = dataContent.DBConnectString();

            //ITarget t = new Adapter();
            //t.Request("test");
        }
    }

    public interface ITarget
    {
        string Request(string param);
    }

    /// <summary>
    /// 需要适配的类接口
    /// </summary>
    public class Cdaptee
    {
        public string SpecificRequest()
        {
            Console.WriteLine("Called SpecificRequest()");
            return "Called SpecificRequest()";
        }
    }

    public class Adapter : Cdaptee, ITarget
    {
        public string Request(string param)
        {
            return base.SpecificRequest();
        }
    }
}
