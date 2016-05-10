/***************************************************************************** 
*        filename :Accountant 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Accountant 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ObserverPattern 
*        文件名:             Accountant 
*        创建系统时间:       2015/11/26 10:30:01 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 财务，已经不需要实现抽象的观察者类，并且不用引用具体的主题
    /// </summary>
    public class Accountant
    {
        private string accountantState;

        public Accountant()
        { }

        /// <summary>
        /// 开发票
        /// </summary>
        public void GiveInvoice()
        {
            Console.WriteLine("我是会计，我来开具发票。");
            accountantState = "已开发票";
        }
    }
}
