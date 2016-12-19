using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class OutRef
    {


        //        out特点：

        //1、方法定义和调用方法都必须显示使用out关键字。（如上代码显然易见）

        //2、out只出不进，即具有清空方法体外参数功能。（如上代码，读者可以任意改变n1和n2的值，只要不改变GetSum()方法体，输出的值均为13）

        //3、为引用类型。（直接调用而不事先定义n1和n2，编译不通过）

        //4、同名函数，out不与ref同时存在，可以重载。
        static void Main2(string[] args)
        {
            int n1, n2;
            Console.WriteLine(GetSum(out n1, out n2));
            Console.Read();
        } //out参数 
        static public int GetSum(out int numberFirst, out int numberSecond)
        {
            numberFirst = 10; numberSecond = 3;
            return numberFirst + numberSecond;
        }


        //        ref特点：

        //1、方法定义和调用方法都必须显示使用ref关键字。（如上代码显然易见）

        //2、ref有进有出，即可以把值传入方法体内。（如上代码，读者可以任意改变n1和n2的值）

        //四、out与ref异同
        static void Main(string[] args)
        {
            int n1 = 1, n2 = 3;
            Console.WriteLine(refGetSum(ref n1, ref n2));
            Console.Read();
        } //ref参数 
        static public int refGetSum(ref int numberFirst, ref int numberSecond)
        {
            numberFirst = 10; numberSecond = 3;
            return numberFirst + numberSecond;
        }
    }
}
