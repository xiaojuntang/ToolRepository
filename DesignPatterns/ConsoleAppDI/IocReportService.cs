using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppDI
{
    public class IocReportService
    {
        private string _data;

        public IocReportService(string data)
        {
            _data = data;
        }

        public void Trim(string data)
        {
            _data = data.Trim();
        }

        public void Clean()
        {
            _data = _data.Replace("@", "");
            _data = _data.Replace("-", "");        
        }

        public void Show()
        {
            Console.WriteLine(_data);

            //过程式的思考方式 客户端拥有流程控制权
            //var IocReportService = new IocReportService("");
            //IocReportService.Trim("");
            //IocReportService.Clean();
            //IocReportService.Show();
        }
    }


    public class ReportEngine
    {
        private string _data;
        public ReportEngine(string data)
        {
            _data = data;
        }
        public void Show()
        {
            Trim();
            Clean();
            Display();
        }
        public virtual void Trim()
        {
            _data = _data.Trim();
        }
        public virtual void Clean()
        {
            _data = _data.Replace("@", "");
            _data = _data.Replace("-", "");
        }
        public virtual void Display()
        {
            Console.WriteLine(_data);


            //var reportEngine = new StringReportEngine(input);
            //reportEngine.Show();
        }

    }
}
