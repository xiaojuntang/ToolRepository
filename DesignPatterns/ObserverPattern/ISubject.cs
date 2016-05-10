/***************************************************************************** 
*        filename :ISubject 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ISubject 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ObserverPattern 
*        文件名:             ISubject 
*        创建系统时间:       2015/11/26 10:25:39 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObserverPattern
{
    /// <summary>
    /// 声名委托
    /// </summary>
    public delegate void CustomerEventHandler();

    /// <summary>
    /// 抽象主题
    /// </summary>
    public interface ISubject
    {
        void Notify();
    }
}
