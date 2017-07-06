using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MvcPatterns.Controllers
{
    public class Goods
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }
    }

    public class VueController : Controller
    {
        // GET: Vue
        public ActionResult Index()
        {
            FreeSpireHelper.PPT2PDF();

            //Presentation presentation = new Presentation();

            //presentation.LoadFromFile("D:\\测试质控部十一月份第一周周报-高燕.pptx");

            ////traverse the slides of PPT files
            //for (int i = 0; i < presentation.Slides.Count; i++)
            //{
            //    //save the slide to Image
            //    Image image = presentation.Slides[i].SaveAsImage();

            //    String fileName = String.Format("result-img-{0}.png", i);
            //    image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

            //    //view the image
            //    System.Diagnostics.Process.Start(fileName);
            //}


            return View();
        }
        //Nuget  Microsoft.AspNet.WebApi.Cors
        //解决跨域问题
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetData()
        {
            Goods goods1 = new Goods();
            goods1.Id = 1;
            goods1.Barcode = "101";
            goods1.Name = "中华面粉";
            goods1.ShortName = "面粉";

            Goods goods2 = new Goods();
            goods2.Id = 1;
            goods2.Barcode = "102";
            goods2.Name = "中华面粉2";
            goods2.ShortName = "面粉2";

            Goods goods3 = new Goods();
            goods3.Id = 1;
            goods3.Barcode = "103";
            goods3.Name = "中华面粉3";
            goods3.ShortName = "面粉3";

            List<Goods> goodList = new List<Goods>();
            goodList.Add(goods1);
            goodList.Add(goods2);
            goodList.Add(goods3);

            string aa = JsonConvert.SerializeObject(goodList);

            return Json(goodList, JsonRequestBehavior.AllowGet);
        }
    }
}