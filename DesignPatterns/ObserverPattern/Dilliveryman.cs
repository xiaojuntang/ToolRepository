/***************************************************************************** 
*        filename :Dilliveryman 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Dilliveryman 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ObserverPattern 
*        文件名:             Dilliveryman 
*        创建系统时间:       2015/11/26 10:35:10 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 配送员，已经不需要实现抽象的观察者类，并且不用引用具体的主题
    /// </summary>
    public class Dilliveryman
    {
        private string dillivierymanState;

        public void Dilliver()
        {
            Console.WriteLine("我是配送员，我来发货。");
            dillivierymanState = "已发货";
        }
    }

    public class UserInfo
    {
        private string userName;
        public UserInfo(string userName)
        {
            this.userName = userName;
        }
        public void AcceptMsg()
        {
            Console.WriteLine("用户{0}收到通知", this.userName);
        }
    }
}
