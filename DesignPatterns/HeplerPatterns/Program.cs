﻿using Common.Net.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeplerPatterns
{
    class Program
    {
        public class TeachingMaterial
        {

        }

        static void Main(string[] args)
        {
            //Common.Net.Helper.LogHelper.GetLogByName("DebugFileAppender").Debug("1111111111");
            //Common.Net.Helper.LogHelper.GetLogByName().Error("2222222222");
            //Common.Net.Helper.LogHelper.GetLogByName().Info("33333333333");
            List<TestA> list = new List<TestA>();
            for (int i = 0; i < 10; i++)
            {
                TestA test = new TestA();
                test.A = i.ToString();
                test.B = i * 3;
                list.Add(test);
            }

            CacheHelper.Add("C:U:001", list);

            if (CacheHelper.IsExist("C:U:001"))
            {
                List<TestA> tes = CacheHelper.Get("C:U:001") as List<TestA>;
            }




            //for (int i = 0; i < 1000 ; i++)
            //{
            //    LogHelper.Error("测试");
            //    LogHelper.Debug("测试");
            //}

            //HttpHelper ht = new HttpHelper();
            //ht.GetHtml(new HttpItem() {                    
            //});

            Console.ReadLine();
        }
    }

    public class TestA
    {
        public string A { get; set; }

        public int B { get; set; }
    }


   
}