/***************************************************************************** 
*        filename :Producer 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Producer 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ProducerAndConsumer 
*        文件名:             Producer 
*        创建系统时间:       2015/11/24 17:11:03 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    /// <summary>
    /// 生产者代码
    /// </summary>
    public class Producer
    {
        private Info info;

        public Producer(Info info)
        {
            this.info = info;
        }

        public void Produce()
        {
            for (int i = 0; i < 100; i++)
            {
                lock (info.LockObj)
                {
                    if (info.HasContent)
                        Monitor.Wait(info.LockObj);

                    info.Name = "Name" + i;
                    Thread.Sleep(500);
                    info.Content = "Content" + i;

                    Console.WriteLine("生产消息：" + this.info.Name + "," + this.info.Content);

                    info.HasContent = true;
                    Monitor.PulseAll(info.LockObj);
                }
            }
        }
    }
}
