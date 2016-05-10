/***************************************************************************** 
*        filename :Cashier 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Cashier 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ObserverPattern 
*        文件名:             Cashier 
*        创建系统时间:       2015/11/26 10:34:51 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 出纳，已经不需要实现抽象的观察者类，并且不用引用具体的主题
    /// </summary>
    public class Cashier
    {
        private string cashierState;

        public void Recoded()
        {
            Console.WriteLine("我是出纳员，我给登记入账。");
            cashierState = "已入账";
        }
    }
}
