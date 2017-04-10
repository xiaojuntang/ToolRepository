using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsibilityPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractComunication communication1 = new EDMComunication();
            AbstractComunication communication2 = new SMSComunication();
            AbstractComunication communication3 = new MMSComunication();



            AbstractHandler hander1 = new ConcreteHandler1();
            AbstractHandler hander2 = new ConcreteHandler2();
            AbstractHandler hander3 = new ConcreteHandler3();
            hander1.SetHandler(hander2);
            hander2.SetHandler(hander3);
            hander1.HandleRequest(3);
        }
    }

    public enum Leaflet
    {
        短信,
        彩信,
        邮件
    }

    public class LeafletEntity
    {
        public int MMSContent { get; set; }

        public Leaflet CommunicationtypeEnum { get; set; }
    }

    public class EventmarketingSmsEdmContentInfo { }


    public abstract class AbstractComunication
    {
        protected AbstractComunication abstractComunication = null;
        public void SetHandler(AbstractComunication abstractComunication)
        {
            this.abstractComunication = abstractComunication;
        }

        public abstract void HanderRequest(LeafletEntity leaflet, EventmarketingSmsEdmContentInfo communicationInfo);
    }

    public class MMSComunication : AbstractComunication
    {
        public override void HanderRequest(LeafletEntity leaflet, EventmarketingSmsEdmContentInfo communicationInfo)
        {
            if (leaflet.CommunicationtypeEnum.HasFlag(Leaflet.彩信))
            {
                leaflet.MMSContent = 0;
            }
            else
            {
                abstractComunication.HanderRequest(leaflet, communicationInfo);
            }
        }
    }

    public class EDMComunication : AbstractComunication
    {
        public override void HanderRequest(LeafletEntity leaflet, EventmarketingSmsEdmContentInfo communicationInfo)
        {
            if (leaflet.CommunicationtypeEnum.HasFlag(Leaflet.邮件))
            {
                //第三步：动态生成邮件模板
                //var styleInfo = CacheUtil.GetRandomEmailStyle();

                //var tuple = new EdmDraftBoxBLL().GetEdmHtmlTitle(communicationInfo.EDMJson, styleInfo.StyleId);

                //leaflet.Title = tuple.Item1;
                //leaflet.EDMContent = tuple.Item2;
                //leaflet.Header = tuple.Item3;
                //leaflet.SendSMSCount = 1;
            }
            else
            {
                abstractComunication.HanderRequest(leaflet, communicationInfo);
            }
        }
    }

    public class SMSComunication : AbstractComunication
    {
        public override void HanderRequest(LeafletEntity leaflet, EventmarketingSmsEdmContentInfo communicationInfo)
        {
            if (leaflet.CommunicationtypeEnum.HasFlag(Leaflet.短信))
            {
                //leaflet.SMSContent = communicationInfo.SMSContent;
                //leaflet.SendSMSCount = communicationInfo.SMSCount;
            }
            else
            {
                abstractComunication.HanderRequest(leaflet, communicationInfo);
            }
        }
    }









    public abstract class AbstractHandler
    {
        protected AbstractHandler abstractHandler = null;

        public void SetHandler(AbstractHandler abstractHandler)
        {
            this.abstractHandler = abstractHandler;
        }

        public virtual void HandleRequest(int request) { }
    }

    public class ConcreteHandler1 : AbstractHandler
    {
        public override void HandleRequest(int request)
        {
            if (request == 1)
            {
                Console.WriteLine("handler1 给你处理了");
            }
            else
            {
                abstractHandler.HandleRequest(request);
            }
        }
    }

    public class ConcreteHandler2 : AbstractHandler
    {
        public override void HandleRequest(int request)
        {
            if (request == 2)
            {
                Console.WriteLine("handler2 给你处理了");
            }
            else
            {
                abstractHandler.HandleRequest(request);
            }
        }
    }

    public class ConcreteHandler3 : AbstractHandler
    {
        public override void HandleRequest(int request)
        {
            if (request == 3)
            {
                Console.WriteLine("handler3 给你处理了");
            }
            else
            {
                abstractHandler.HandleRequest(request);
            }
        }
    }




}
