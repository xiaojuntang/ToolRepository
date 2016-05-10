/***************************************************************************** 
*        filename :Info 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Info 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ProducerAndConsumer 
*        文件名:             Info 
*        创建系统时间:       2015/11/24 17:10:33 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    public class Info
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String content;

        public String Content
        {
            get { return content; }
            set { content = value; }
        }

        private Boolean hasContent;

        public Boolean HasContent
        {
            get { return hasContent; }
            set { hasContent = value; }
        }
        private Object lockObj = new Object();

        public Object LockObj
        {
            get { return lockObj; }
        }

    }
}
