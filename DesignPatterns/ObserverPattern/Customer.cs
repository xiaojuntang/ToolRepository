/***************************************************************************** 
*        filename :Customer 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Customer 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ObserverPattern 
*        文件名:             Customer 
*        创建系统时间:       2015/11/26 10:27:29 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 具体主题
    /// </summary>
    public class Customer : ISubject
    {
        public event CustomerEventHandler Update;
        public void Notify()
        {
            if (Update != null)
                Update();//使用事件来通知给订阅者
        }
    }
}
