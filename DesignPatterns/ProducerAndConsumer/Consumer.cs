/***************************************************************************** 
*        filename :Consumer 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Consumer 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ProducerAndConsumer 
*        文件名:             Consumer 
*        创建系统时间:       2015/11/24 17:09:50 
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
    /// 消费者代码
    /// </summary>
    public class Consumer
    {
        private Info info;

        public Consumer(Info info)
        {
            this.info = info;
        }

        public void Consume()
        {
            for (int i = 0; i < 100; i++)
            {
                lock (info.LockObj)
                {
                    if (!info.HasContent)
                        Monitor.Wait(info.LockObj);
                    Thread.Sleep(500);
                    Console.WriteLine("消费消息：" + this.info.Name + "," + this.info.Content);
                    info.HasContent = false;
                    Monitor.PulseAll(info.LockObj);
                }
            }
        }
    }
}
