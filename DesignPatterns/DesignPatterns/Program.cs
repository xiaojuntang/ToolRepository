using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {

            var m16 = Tra();



            Console.WriteLine("0x{0:x}", 2 << 6);


            Stack sta = new Stack();
            sta.Push("a");
            sta.Push("b");
            sta.Push("c");
            sta.Push("d");
            sta.Push("e");

            var m11 = sta.Pop();




            //Array m2 = Array.CreateInstance();
            ArrayList m3 = new ArrayList();
            SortedList m4 = new SortedList();
            HashSet<int> m5 = new HashSet<int>();
            Hashtable m6 = new Hashtable();
            Queue que = new Queue();
            que.Enqueue("a");
            var m7 = que.Dequeue();
            var m9 = que.Count;
            var m8 = que.Dequeue();


            LinkedList<int> m10 = new LinkedList<int>();

            var m1 = 2 << 2;

            JArrayTest();

            Convert.ToInt32(4.5);//4 获得离该值最近的偶数
            Convert.ToInt32(5.5);//6
            Convert.ToInt32("4.5");//异常

            //Expression<Func<int, int, int, int>> expr = (x, y, z) => (x + y) / z;
            //Console.WriteLine(expr.Compile()(1, 2, 3));

            //Person t = new Person();
            //t.No = 1;
            //t.Name = "测试";
            //t.Birthday = DateTime.Now;
            //Expression<Func<Person>> expr2 = (x, y, z) => (x + y) / z;

            //var aaaaa = FindList(m => m.Name == "yvh" && m.No == 2, true);



            //string mid = ",middle part,";
            /////匿名写法
            //Func<string, string> anonDel = delegate (string param)
            //{
            //    param += mid;
            //    param += " And this was added to the string.";
            //    return param;
            //};
            /////λ表达式写法
            //Func<string, string> lambda = param =>
            //{
            //    param += mid;
            //    param += " And this was added to the string.";
            //    return param;
            //};
            /////λ表达式写法(整形)
            //Func<int, int> lambdaint = paramint =>
            //{
            //    paramint = 5;
            //    return paramint;
            //};
            /////λ表达式带有两个参数的写法
            //Func<int, int, int> twoParams = (x, y) =>
            //{
            //    return x * y;
            //};
            //Console.WriteLine("匿名方法：" + anonDel("Start of string"));
            //Console.WriteLine("λ表达式写法:" + lambda("Lambda expression"));
            //Console.WriteLine("λ表达式写法(整形):" + lambdaint(4).ToString());
            //Console.WriteLine("λ表达式带有两个参数:" + twoParams(10, 20).ToString());

            //int a = 123;
            //int b = 20;
            //var atype = a.GetType();
            //var btype = b.GetType();
            //Console.WriteLine(System.Object.Equals(atype, btype));
            //Console.WriteLine(System.Object.ReferenceEquals(atype, btype));


            //DoTest();

            //object a = "123";
            //object b = "123";
            //Console.WriteLine(System.Object.Equals(a, b));
            //Console.WriteLine(System.Object.ReferenceEquals(a, b));
            //string sa = "123";
            //Console.WriteLine(System.Object.Equals(a, sa));
            //Console.WriteLine(System.Object.ReferenceEquals(a, sa));

            //StringBuilder sb1 = new StringBuilder();
            //Console.WriteLine("Capacity={0}; Length={1};", sb1.Capacity, sb1.Length); //输出：Capacity=16; Length=0;   //初始容量为16 

            //List<Action> acs = new List<Action>(5);
            //for (int i = 0; i < 5; i++)
            //{
            //    acs.Add(() => { Console.WriteLine(i); });
            //}
            //acs.ForEach(ac => ac());

            Console.Read();
        }

        public static IQueryable<Person> FindList<TKey>(Expression<Func<Person, TKey>> order, bool asc)
        {
            var a = order.ToString();
            return null;
        }

        public static void T2() {
        }

        public static void DoTest()
        {
            B1 b1 = new B1();
            B2 b2 = new B2();
            b1.Print();
            b2.Print();      //按预期应该输出 B1、B2

            A ab1 = new B1();
            A ab2 = new B2();
            ab1.Print();
            ab2.Print();   //这里应该输出什么呢？
        }

        public static void JArrayTest()
        {
            //2.1 数组用JArray加载

            string jsonText = "[{'a':'aaa','b':'bbb','c':'ccc'},{'a':'aa','b':'bb','c':'cc'}]";

            var mJObj = JArray.Parse(jsonText, new JsonLoadSettings());
            //var mJObj = (JArray)JsonConvert.DeserializeObject(jsonText);

            //需求，删除列表里的a节点的值为'aa'的项

            IList<JToken> delList = new List<JToken>(); //存储需要删除的项

            foreach (var ss in mJObj)  //查找某个字段与值
            {
                if (((JObject)ss)["a"].ToString() == "aa")
                    delList.Add(ss);
            }

            foreach (var item in delList)  //移除mJObj  在delList 里的项
            {

                mJObj.Remove(item);
            }



            //2.2 非数组用JObject加载 （这里主要以这个为例子）

            //            string jsonText = "[{'a':'aaa','b':'bbb','c':'ccc'}]";

            //            var mJObj = JObject.Parse(jsonText t);

            //            mJObj.Add() //新增，没试过


            //var v1 = mJObj[a].ToString()  //得到'aaa'的值
        }

        public static string Tra()
        {
            string a = "123";
            try
            {
                return a;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                a = a + "1456";
            }
        }
    }
    public class A
    {
        public virtual void Print() { Console.WriteLine("A"); }
    }
    public class B1 : A
    {
        public override void Print() { Console.WriteLine("B1"); }
    }
    public class B2 : A
    {
        public new void Print() { Console.WriteLine("B2"); }
    }
    public class Person
    {
        public int No { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
    public interface ITest
    {
        void MyTest();
    }
    public abstract class My : Person
    {
        public void MyTest()
        {
            throw new NotImplementedException();
        }
    }
}
