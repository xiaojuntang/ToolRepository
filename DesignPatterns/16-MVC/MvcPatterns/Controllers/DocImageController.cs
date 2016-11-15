using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Net.Core;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Aspose.Pdf.Text;
using Aspose.Slides.Export;
using Aspose.Words;
using Aspose.Words.Saving;
using NPOI.SS.Formula.Functions;
using SaveFormat = Aspose.Words.SaveFormat;
using Section = NPOI.HPSF.Section;

namespace MvcPatterns.Controllers
{
    public class DocImageController : Controller
    {
        // GET: DocImage
        public ActionResult Index()
        {
            //ImageConverters.GE();

            IImageConverter   img=new Ppt2ImageConverter();
            img.ConvertToImage("D:\\测试质控部十一月份第一周周报-高燕.pptx", "D:/t");
            return View();
        }



        public string AddDocument()
        {

            string fileToUpload = "D:\\测试质控部十一月份第一周周报-高燕.pptx";
            string FileName = "测试质控部十一月份第一周周报-高燕.pptx";
            string Pattern = FileName.Substring(FileName.LastIndexOf(".") + 1);
            //转化文件为图片
            bool isOK = false;
            if (Pattern.ToLower() == "docx" || Pattern.ToLower() == "doc")
            {
                isOK = ImageConverters.ConvertWordToImage(fileToUpload, "", 1, 0, 128);
            }
            else if (Pattern.ToLower() == "pdf")
            {
                isOK = ImageConverters.ConvertPDFToImage(fileToUpload, "", 1, 0, 128);
            }
            else if (Pattern.ToLower() == "ppt" || Pattern.ToLower() == "pptx")
            {
                isOK = ImageConverters.ConvertPPTToImage(fileToUpload, "", 1, 0, 128);
            }
            else if (Pattern.ToLower() == "jpg" || Pattern.ToLower() == "png")
            {

            }
            else
            {
                return "0|不支持此格式的文件";
            }

            if (isOK)
            {
                //找到所有转化的图片，上传OSS
                string imageDirPath = Path.GetDirectoryName(fileToUpload);
                DirectoryInfo imageFolder = new DirectoryInfo(imageDirPath);
                string imageNamePrefix = Path.GetFileNameWithoutExtension(fileToUpload);
                foreach (var f in imageFolder.GetFiles())
                {
                    if (f.Extension == ".jpg" && f.Name.StartsWith(imageNamePrefix))
                    {
                        //OSSHelper.PutObjectFromFile(f.Name, f.FullName);
                        //HW_DocumentOSSFile dossf = new HW_DocumentOSSFile();
                        //dossf.FileOrderNumber = fileOrderNumber;
                        //dossf.OrderNumber = int.Parse(f.Name.Replace(imageNamePrefix + "_", "").Replace(f.Extension, ""));
                        //dossf.OSSKey = f.Name;
                        //dfs.OSSFiles.Add(dossf);
                    }
                }
            }
            else
            {
                return "0|文件转图片失败！";
            }
            return "|上传成功！";
        }

    }

    public class ImageConverters
    {

        public static void GE()
        {
            Document doc = new Document("D:\\测试质控部十一月份第一周周报-高燕.pptx");
            ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Jpeg);
            iso.Resolution = 128;
            iso.PrettyFormat = true;
            iso.UseAntiAliasing = true;
            for (int i = 0; i < doc.PageCount; i++)
            {
                iso.PageIndex = i; doc.Save("D:/t/test" + i + ".jpg", iso);
            }



            //string filePath = string.Empty;
            //Document doc = new Document();
            //int ran = new Random().Next(100);
            //fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ran + strExt.Trim('*');
            //filePath = Path.Combine(path, fileName);

            //fileUpload.SaveAs(filePath);
            //doc = new Document(filePath);
            //foreach (Section section in doc)
            //{
            //    section.PageSetup.PaperSize = PaperSize.A4;
            //    section.PageSetup.Orientation = Aspose.Words.Orientation.Portrait;
            //    section.PageSetup.VerticalAlignment = Aspose.Words.PageVerticalAlignment.Top;
            //}

            //string imageName = fileName.Replace(strExt.Trim('*'), "");
            //this.Session["ChangeImage"] = new CLS_ChangeImage { Index = 0, SumCount = doc.PageCount, NumState = 0 };

            //ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Jpeg);
            //iso.PrettyFormat = true;
            //iso.UseAntiAliasing = true;
            //ci.NumState = 2;
            //this.Session["ChangeImage"] = ci;
            //System.Text.StringBuilder sbImg = new StringBuilder();
            //string imgSavePath = ConfigurationManager.AppSettings["UploadPath"];
            //for (int i = 0; i < doc.PageCount; i++)
            //{
            //    ci.Index = i + 1;
            //    ci.SumCount = doc.PageCount;

            //    iso.PageIndex = i;
            //    string iName = imageName + "_" + i + ".jpg";

            //    doc.Save(path + "/" + iName, iso);
            //    FileStream file = new FileStream(path + "/" + iName, FileMode.Open);

            //    AF.UploadFileBinary(file, iName);//吧文件保存到另外一个地方
            //    sbImg.Append(iName + "|");
            //    this.Session["ChangeImage"] = ci;
            //    //System.Threading.Thread.Sleep(5000);

            //}
        }


        /// <summary>
        /// 将Word文档转换为图片的方法      
        /// </summary>
        /// <param name="wordInputPath">Word文件路径</param>
        /// <param name="imageOutputDirPath">图片输出路径，如果为空，默认值为Word所在路径</param>      
        /// <param name="startPageNum">从PDF文档的第几页开始转换，如果为0，默认值为1</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换，如果为0，默认值为Word总页数</param>
        /// <param name="imageFormat">设置所需图片格式，如果为null，默认格式为PNG</param>
        /// <param name="resolution">设置图片的像素，数字越大越清晰，如果为0，默认值为128，建议最大值不要超过1024</param>
        public static bool ConvertWordToImage(string wordInputPath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            bool isOK = false;
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(wordInputPath);

                if (doc == null)
                {
                    throw new Exception("Word文件无效或者Word文件被加密！");
                }

                if (imageOutputDirPath.Trim().Length == 0)
                {
                    imageOutputDirPath = Path.GetDirectoryName(wordInputPath);
                }

                if (!Directory.Exists(imageOutputDirPath))
                {
                    Directory.CreateDirectory(imageOutputDirPath);
                }

                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > doc.PageCount || endPageNum <= 0)
                {
                    endPageNum = doc.PageCount;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }


                if (resolution <= 0)
                {
                    resolution = 128;
                }

                string imageName = Path.GetFileNameWithoutExtension(wordInputPath);
                Aspose.Words.Saving.ImageSaveOptions imageSaveOptions = new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Png);
                imageSaveOptions.Resolution = resolution;
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    imageSaveOptions.PageIndex = i - 1;
                    string imgPath = Path.Combine(imageOutputDirPath, imageName) + "_" + i.ToString() + ".jpg";
                    doc.Save(stream, imageSaveOptions);
                    Image img = Image.FromStream(stream);
                    Bitmap bm = new Bitmap(img);
                    bm.Save(imgPath, ImageFormat.Jpeg);
                    img.Dispose();
                    stream.Dispose();
                    bm.Dispose();

                }
                isOK = true;
            }
            catch (Exception ex)
            {
                isOK = false;
            }
            return isOK;
        }


        /// <summary>
        /// 将pdf文档转换为图片的方法      
        /// </summary>
        /// <param name="originFilePath">pdf文件路径</param>
        /// <param name="imageOutputDirPath">图片输出路径，如果为空，默认值为pdf所在路径</param>       
        /// <param name="startPageNum">从PDF文档的第几页开始转换，如果为0，默认值为1</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换，如果为0，默认值为pdf总页数</param>       
        /// <param name="resolution">设置图片的像素，数字越大越清晰，如果为0，默认值为128，建议最大值不要超过1024</param>
        public static bool ConvertPDFToImage(string originFilePath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            bool isOK = false;
            try
            {
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document(originFilePath);

                if (doc == null)
                {
                    throw new Exception("pdf文件无效或者pdf文件被加密！");
                }

                if (imageOutputDirPath.Trim().Length == 0)
                {
                    imageOutputDirPath = Path.GetDirectoryName(originFilePath);
                }

                if (!Directory.Exists(imageOutputDirPath))
                {
                    Directory.CreateDirectory(imageOutputDirPath);
                }

                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > doc.Pages.Count || endPageNum <= 0)
                {
                    endPageNum = doc.Pages.Count;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }

                if (resolution <= 0)
                {
                    resolution = 128;
                }

                string imageNamePrefix = Path.GetFileNameWithoutExtension(originFilePath);
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    string imgPath = Path.Combine(imageOutputDirPath, imageNamePrefix) + "_" + i.ToString() + ".jpg";
                    Aspose.Pdf.Devices.Resolution reso = new Aspose.Pdf.Devices.Resolution(resolution);
                    Aspose.Pdf.Devices.JpegDevice jpegDevice = new Aspose.Pdf.Devices.JpegDevice(reso, 100);
                    jpegDevice.Process(doc.Pages[i], stream);

                    Image img = Image.FromStream(stream);
                    Bitmap bm = new Bitmap(img);
                    bm.Save(imgPath, ImageFormat.Jpeg);
                    img.Dispose();
                    stream.Dispose();
                    bm.Dispose();

                }
                isOK = true;
            }
            catch (Exception ex)
            {
                isOK = false;
            }
            return isOK;
        }

        /// <summary>
        /// PPT转图片
        /// </summary>
        /// <param name="originFilePath"></param>
        /// <param name="imageOutputDirPath"></param>
        /// <param name="startPageNum"></param>
        /// <param name="endPageNum"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static bool ConvertPPTToImage(string originFilePath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            bool isOK = false;
            try
            {
                Aspose.Slides.Presentation doc = new Aspose.Slides.Presentation(originFilePath);

                if (doc == null)
                {
                    throw new Exception("ppt文件无效或者ppt文件被加密！");
                }

                //先将ppt转换为pdf临时文件
                string tmpfolder = Path.GetDirectoryName(originFilePath);
                string tmpPrefix = Path.GetFileNameWithoutExtension(originFilePath);
                string tmpPdfPath = tmpfolder + "\\" + tmpPrefix + ".pdf";
                doc.Save(tmpPdfPath, Aspose.Slides.Export.SaveFormat.Pdf);

                //再将pdf转换为图片
                ConvertPDFToImage(tmpPdfPath, imageOutputDirPath, startPageNum, endPageNum, resolution);

                //删除pdf临时文件
                File.Delete(tmpPdfPath);
                isOK = true;
            }
            catch (Exception ex)
            {
                isOK = false;
            }
            return isOK;
        }
    }

    public interface IImageConverter
    {
        void ConvertToImage(string originFilePath, string imageOutputDirPath);
    }


    public class ImageConverterFactory
    {
        public IImageConverter CreateImageConverter(string extendName)
        {
            if (extendName == ".doc" || extendName == ".docx")
            {
                return new Word2ImageConverter();
            }

            if (extendName == ".pdf")
            {
                return new Pdf2ImageConverter();
            }

            if (extendName == ".ppt" || extendName == ".pptx")
            {
                return new Ppt2ImageConverter();
            }

            if (extendName == ".rar")
            {
                return new Rar2ImageConverter();
            }

            return null;
        }

        public bool Support(string extendName)
        {
            return extendName == ".doc" || extendName == ".docx" || extendName == ".pdf" || extendName == ".ppt" || extendName == ".pptx" || extendName == ".rar";
        }
    }

    public delegate void CbGeneric<T, T1>(T item, T1 item2);

    public delegate void CbGeneric();

    public delegate void CbGeneric<T>(T item);
    #region 将word文档转换为图片  
    public class Word2ImageConverter : IImageConverter
    {
        private bool cancelled = false;
        public event CbGeneric<int, int> ProgressChanged;
        public event CbGeneric ConvertSucceed;
        public event CbGeneric<string> ConvertFailed;

        public void Cancel()
        {
            if (this.cancelled)
            {
                return;
            }

            this.cancelled = true;
        }

        public void ConvertToImage(string originFilePath, string imageOutputDirPath)
        {
            this.cancelled = false;
            ConvertToImage(originFilePath, imageOutputDirPath, 0, 0, null, 200);
        }

        /**//// <summary>  
            /// 将Word文档转换为图片的方法        
            /// </summary>  
            /// <param name="wordInputPath">Word文件路径</param>  
            /// <param name="imageOutputDirPath">图片输出路径，如果为空，默认值为Word所在路径</param>        
            /// <param name="startPageNum">从PDF文档的第几页开始转换，如果为0，默认值为1</param>  
            /// <param name="endPageNum">从PDF文档的第几页开始停止转换，如果为0，默认值为Word总页数</param>  
            /// <param name="imageFormat">设置所需图片格式，如果为null，默认格式为PNG</param>  
            /// <param name="resolution">设置图片的像素，数字越大越清晰，如果为0，默认值为128，建议最大值不要超过1024</param>  
        private void ConvertToImage(string wordInputPath, string imageOutputDirPath, int startPageNum, int endPageNum, ImageFormat imageFormat, int resolution)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(wordInputPath);

                if (doc == null)
                {
                    throw new Exception("Word文件无效或者Word文件被加密！");
                }

                if (imageOutputDirPath.Trim().Length == 0)
                {
                    imageOutputDirPath = Path.GetDirectoryName(wordInputPath);
                }

                if (!Directory.Exists(imageOutputDirPath))
                {
                    Directory.CreateDirectory(imageOutputDirPath);
                }

                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > doc.PageCount || endPageNum <= 0)
                {
                    endPageNum = doc.PageCount;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }

                if (imageFormat == null)
                {
                    imageFormat = ImageFormat.Png;
                }

                if (resolution <= 0)
                {
                    resolution = 128;
                }

                string imageName = Path.GetFileNameWithoutExtension(wordInputPath);
                Aspose.Words.Saving.ImageSaveOptions imageSaveOptions = new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Png);
                imageSaveOptions.Resolution = resolution;
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    if (this.cancelled)
                    {
                        break;
                    }

                    MemoryStream stream = new MemoryStream();
                    imageSaveOptions.PageIndex = i - 1;
                    string imgPath = Path.Combine(imageOutputDirPath, imageName) + "_" + i.ToString("000") + "." + imageFormat.ToString();
                    doc.Save(stream, imageSaveOptions);
                    Image img = Image.FromStream(stream);
                    Bitmap bm = new Bitmap(img); ;//ESBasic.Helpers.ImageHelper.Zoom(img, 0.6f);
                    bm.Save(imgPath, imageFormat);
                    img.Dispose();
                    stream.Dispose();
                    bm.Dispose();

                    System.Threading.Thread.Sleep(200);
                    if (this.ProgressChanged != null)
                    {
                        this.ProgressChanged(i - 1, endPageNum);
                    }
                }

                if (this.cancelled)
                {
                    return;
                }

                if (this.ConvertSucceed != null)
                {
                    this.ConvertSucceed();
                }
            }
            catch (Exception ex)
            {
                if (this.ConvertFailed != null)
                {
                    this.ConvertFailed(ex.Message);
                }
            }
        }
    }
    #endregion


    #region 将pdf文档转换为图片  
    public class Pdf2ImageConverter : IImageConverter
    {
        private bool cancelled = false;
        public event CbGeneric<int, int> ProgressChanged;
        public event CbGeneric ConvertSucceed;
        public event CbGeneric<string> ConvertFailed;

        public void Cancel()
        {
            if (this.cancelled)
            {
                return;
            }

            this.cancelled = true;
        }

        public void ConvertToImage(string originFilePath, string imageOutputDirPath)
        {
            this.cancelled = false;
            ConvertToImage(originFilePath, imageOutputDirPath, 0, 0, 200);
        }

        /**//// <summary>  
            /// 将pdf文档转换为图片的方法        
            /// </summary>  
            /// <param name="originFilePath">pdf文件路径</param>  
            /// <param name="imageOutputDirPath">图片输出路径，如果为空，默认值为pdf所在路径</param>         
            /// <param name="startPageNum">从PDF文档的第几页开始转换，如果为0，默认值为1</param>  
            /// <param name="endPageNum">从PDF文档的第几页开始停止转换，如果为0，默认值为pdf总页数</param>         
            /// <param name="resolution">设置图片的像素，数字越大越清晰，如果为0，默认值为128，建议最大值不要超过1024</param>  
        private void ConvertToImage(string originFilePath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            try
            {
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document(originFilePath);

                if (doc == null)
                {
                    throw new Exception("pdf文件无效或者pdf文件被加密！");
                }

                if (imageOutputDirPath.Trim().Length == 0)
                {
                    imageOutputDirPath = Path.GetDirectoryName(originFilePath);
                }

                if (!Directory.Exists(imageOutputDirPath))
                {
                    Directory.CreateDirectory(imageOutputDirPath);
                }

                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > doc.Pages.Count || endPageNum <= 0)
                {
                    endPageNum = doc.Pages.Count;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }

                if (resolution <= 0)
                {
                    resolution = 128;
                }

                string imageNamePrefix = Path.GetFileNameWithoutExtension(originFilePath);
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    if (this.cancelled)
                    {
                        break;
                    }

                    MemoryStream stream = new MemoryStream();
                    string imgPath = Path.Combine(imageOutputDirPath, imageNamePrefix) + "_" + i.ToString("000") + ".jpg";
                    Aspose.Pdf.Devices.Resolution reso = new Aspose.Pdf.Devices.Resolution(resolution);
                    Aspose.Pdf.Devices.JpegDevice jpegDevice = new Aspose.Pdf.Devices.JpegDevice(reso, 100);
                    jpegDevice.Process(doc.Pages[i], stream);

                    Image img = Image.FromStream(stream);
                    Bitmap bm = new Bitmap(img);//ESBasic.Helpers.ImageHelper.Zoom(img, 0.6f);
                    bm.Save(imgPath, ImageFormat.Jpeg);
                    img.Dispose();
                    stream.Dispose();
                    bm.Dispose();

                    System.Threading.Thread.Sleep(200);
                    if (this.ProgressChanged != null)
                    {
                        this.ProgressChanged(i - 1, endPageNum);
                    }
                }

                if (this.cancelled)
                {
                    return;
                }

                if (this.ConvertSucceed != null)
                {
                    this.ConvertSucceed();
                }
            }
            catch (Exception ex)
            {
                if (this.ConvertFailed != null)
                {
                    this.ConvertFailed(ex.Message);
                }
            }
        }
    }
    #endregion


    #region 将ppt文档转换为图片  
    public class Ppt2ImageConverter : IImageConverter
    {
        private Pdf2ImageConverter pdf2ImageConverter;
        public event CbGeneric<int, int> ProgressChanged;
        public event CbGeneric ConvertSucceed;
        public event CbGeneric<string> ConvertFailed;

        public void Cancel()
        {
            if (this.pdf2ImageConverter != null)
            {
                this.pdf2ImageConverter.Cancel();
            }
        }

        public void ConvertToImage(string originFilePath, string imageOutputDirPath)
        {
            ConvertToImage(originFilePath, imageOutputDirPath, 0, 0, 200);
        }

        /**//// <summary>  
            /// 将pdf文档转换为图片的方法        
            /// </summary>  
            /// <param name="originFilePath">ppt文件路径</param>  
            /// <param name="imageOutputDirPath">图片输出路径，如果为空，默认值为pdf所在路径</param>         
            /// <param name="startPageNum">从PDF文档的第几页开始转换，如果为0，默认值为1</param>  
            /// <param name="endPageNum">从PDF文档的第几页开始停止转换，如果为0，默认值为pdf总页数</param>         
            /// <param name="resolution">设置图片的像素，数字越大越清晰，如果为0，默认值为128，建议最大值不要超过1024</param>  
        private void ConvertToImage(string originFilePath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            try
            {
                Aspose.Slides.Presentation doc = new Aspose.Slides.Presentation(originFilePath);

                if (doc == null)
                {
                    throw new Exception("ppt文件无效或者ppt文件被加密！");
                }

                if (imageOutputDirPath.Trim().Length == 0)
                {
                    imageOutputDirPath = Path.GetDirectoryName(originFilePath);
                }

                if (!Directory.Exists(imageOutputDirPath))
                {
                    Directory.CreateDirectory(imageOutputDirPath);
                }

                if (startPageNum <= 0)
                {
                    startPageNum = 1;
                }

                if (endPageNum > doc.Slides.Count || endPageNum <= 0)
                {
                    endPageNum = doc.Slides.Count;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }

                if (resolution <= 0)
                {
                    resolution = 128;
                }





                //switch (documentType)
                //{
                //    case OfficeDocumentType.Word:
                //        new Aspose.Words.License().SetLicense(@"../Lic/aspose.lic");
                //        Aspose.Words.Document doc = new Aspose.Words.Document(sourcePath);
                //        doc.Save(targetPath, Aspose.Words.SaveFormat.Pdf);
                //        result = true;
                //        break;
                //    case OfficeDocumentType.Excel:
                //        new Aspose.Cells.License().SetLicense(@"../Lic/aspose.lic");
                //        Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(sourcePath);
                //        xls.Save(targetPath, Aspose.Cells.SaveFormat.Pdf);
                //        result = true;
                //        break;
                //    case OfficeDocumentType.PowerPoint:
                //        new Aspose.Slides.License().SetLicense(@"../Lic/aspose.lic");
                //        Aspose.Slides.Presentation ppt = new Aspose.Slides.Presentation(sourcePath);
                //        ppt.Save(targetPath, Aspose.Slides.Export.SaveFormat.Pdf);
                //        result = true;
                //        break;
                //    default: break;
                //}





                ISaveOptions o=new PdfOptions();
             
                //先将ppt转换为pdf临时文件  
                string tmpPdfPath = originFilePath + ".pdf";
                doc.Save(tmpPdfPath, Aspose.Slides.Export.SaveFormat.Pdf,o);

                //再将pdf转换为图片  
                Pdf2ImageConverter converter = new Pdf2ImageConverter();
                converter.ConvertFailed += new CbGeneric<string>(converter_ConvertFailed);
                converter.ConvertSucceed += new CbGeneric(converter_ConvertSucceed);
                converter.ProgressChanged += new CbGeneric<int, int>(converter_ProgressChanged);
                converter.ConvertToImage(tmpPdfPath, imageOutputDirPath);

                //删除pdf临时文件  
                File.Delete(tmpPdfPath);

                if (this.ConvertSucceed != null)
                {
                    this.ConvertSucceed();
                }
            }
            catch (Exception ex)
            {
                if (this.ConvertFailed != null)
                {
                    this.ConvertFailed(ex.Message);
                }
            }

            this.pdf2ImageConverter = null;
        }

        void converter_ProgressChanged(int done, int total)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(done, total);
            }
        }

        void converter_ConvertSucceed()
        {
            if (this.ConvertSucceed != null)
            {
                this.ConvertSucceed();
            }
        }

        void converter_ConvertFailed(string msg)
        {
            if (this.ConvertFailed != null)
            {
                this.ConvertFailed(msg);
            }
        }
    }
    #endregion


    #region 将图片压缩包解压。（如果课件本身就是多张图片，那么可以将它们压缩成一个rar，作为一个课件）  
    //// <summary>  
    /// 将图片压缩包解压。（如果课件本身就是多张图片，那么可以将它们压缩成一个rar，作为一个课件）  
    /// </summary>  
    public class Rar2ImageConverter : IImageConverter
    {
        private bool cancelled = false;
        public event CbGeneric<string> ConvertFailed;
        public event CbGeneric<int, int> ProgressChanged;
        public event CbGeneric ConvertSucceed;

        public void Cancel()
        {
            this.cancelled = true;
        }


        //public void ConvertToImage(string rarPath, string imageOutputDirPath)
        //{
        //    try
        //    {
        //        Unrar tmp = new Unrar(rarPath);
        //        tmp.Open(Unrar.OpenMode.List);
        //        string[] files = tmp.ListFiles();
        //        tmp.Close();

        //        int total = files.Length;
        //        int done = 0;

        //        Unrar unrar = new Unrar(rarPath);
        //        unrar.Open(Unrar.OpenMode.Extract);
        //        unrar.DestinationPath = imageOutputDirPath;

        //        while (unrar.ReadHeader() && !cancelled)
        //        {
        //            if (unrar.CurrentFile.IsDirectory)
        //            {
        //                unrar.Skip();
        //            }
        //            else
        //            {
        //                unrar.Extract();
        //                ++done;

        //                if (this.ProgressChanged != null)
        //                {
        //                    this.ProgressChanged(done, total);
        //                }
        //            }
        //        }
        //        unrar.Close();

        //        if (this.ConvertSucceed != null)
        //        {
        //            this.ConvertSucceed();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (this.ConvertFailed != null)
        //        {
        //            this.ConvertFailed(ex.Message);
        //        }
        //    }
        //}


        public void ConvertToImage(string originFilePath, string imageOutputDirPath)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

}