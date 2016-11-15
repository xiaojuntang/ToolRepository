using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Cells;

namespace MvcPatterns
{
    public class Comm
    {
       
    }

    public static class AsposeLicenseHelper
    {
        public const string Key =
            "PExpY2Vuc2U+DQogIDxEYXRhPg0KICAgIDxMaWNlbnNlZFRvPkFzcG9zZSBTY290bGFuZCB" +
            "UZWFtPC9MaWNlbnNlZFRvPg0KICAgIDxFbWFpbFRvPmJpbGx5Lmx1bmRpZUBhc3Bvc2UuY2" +
            "9tPC9FbWFpbFRvPg0KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlV" +
            "HlwZT4NCiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1p" +
            "dGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+DQogICAgPE9yZGVySUQ+MTQ" +
            "wNDA4MDUyMzI0PC9PcmRlcklEPg0KICAgIDxVc2VySUQ+OTQyMzY8L1VzZXJJRD4NCiAgIC" +
            "A8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KICAgIDxQc" +
            "m9kdWN0cz4NCiAgICAgIDxQcm9kdWN0PkFzcG9zZS5Ub3RhbCBmb3IgLk5FVDwvUHJvZHVj" +
            "dD4NCiAgICA8L1Byb2R1Y3RzPg0KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl" +
            "0aW9uVHlwZT4NCiAgICA8U2VyaWFsTnVtYmVyPjlhNTk1NDdjLTQxZjAtNDI4Yi1iYTcyLT" +
            "djNDM2OGYxNTFkNzwvU2VyaWFsTnVtYmVyPg0KICAgIDxTdWJzY3JpcHRpb25FeHBpcnk+M" +
            "jAxNTEyMzE8L1N1YnNjcmlwdGlvbkV4cGlyeT4NCiAgICA8TGljZW5zZVZlcnNpb24+My4w" +
            "PC9MaWNlbnNlVmVyc2lvbj4NCiAgICA8TGljZW5zZUluc3RydWN0aW9ucz5odHRwOi8vd3d" +
            "3LmFzcG9zZS5jb20vY29ycG9yYXRlL3B1cmNoYXNlL2xpY2Vuc2UtaW5zdHJ1Y3Rpb25zLm" +
            "FzcHg8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+DQogIDwvRGF0YT4NCiAgPFNpZ25hdHVyZT5GT" +
            "zNQSHNibGdEdDhGNTlzTVQxbDFhbXlpOXFrMlY2RThkUWtJUDdMZFRKU3hEaWJORUZ1MXpP" +
            "aW5RYnFGZkt2L3J1dHR2Y3hvUk9rYzF0VWUwRHRPNmNQMVpmNkowVmVtZ1NZOGkvTFpFQ1R" +
            "Hc3pScUpWUVJaME1vVm5CaHVQQUprNWVsaTdmaFZjRjhoV2QzRTRYUTNMemZtSkN1YWoyTk" +
            "V0ZVJpNUhyZmc9PC9TaWduYXR1cmU+DQo8L0xpY2Vuc2U+";
        public static Stream LStream = (Stream)new MemoryStream(Convert.FromBase64String(Key));

        static readonly string LicensePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\DoucmentManager\\Aspose.Total.lic";
        public static void SetWordsLicense()
        {
            var l = new Aspose.Words.License();
            l.SetLicense(LicensePath);
        }
        public static void SetPdfLicense()
        {
            var l = new Aspose.Pdf.License();
            l.SetLicense(LicensePath);
        }
        public static void SetSlidesLicense()
        {
            var l = new Aspose.Slides.License();
            l.SetLicense(LicensePath);
        }
        /// <summary>
        /// Excel
        /// </summary>
        public static void SetCellsLicense()
        {
            var l = new Aspose.Cells.License();
            l.SetLicense(LicensePath);
        }
    }


    public delegate void CbGeneric<T, T1>(T item, T1 item2);

    public delegate void CbGeneric();

    public delegate void CbGeneric<T>(T item);

    public interface IImageConverter
    {
        void ConvertToImage(string originFilePath, string imageOutputDirPath);
    }

    #region 图片转换器工厂
    /// <summary>
    /// 图片转换器工厂。
    /// </summary>
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
            if (extendName == ".xls" || extendName == ".xlsx")
            {
                return new Xls2ImageConverter();
            }
            return null;
        }

        public bool Support(string extendName)
        {
            return extendName == ".doc" || extendName == ".docx" || extendName == ".pdf" || extendName == ".ppt" || extendName == ".pptx" || extendName == ".rar";
        }
    }
    #endregion

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

        /// <summary>
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

                AsposeLicenseHelper.SetWordsLicense();
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
                string imageName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(wordInputPath));
                //string imageName = Path.GetFileNameWithoutExtension(wordInputPath);
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
                    //string imgPath = Path.Combine(imageOutputDirPath, imageName) + "_" + i.ToString("000") + "." + imageFormat.ToString();
                    string imgPath = Path.Combine(imageOutputDirPath, "A") + "_" + i + ".Png";
                    doc.Save(stream, imageSaveOptions);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    Bitmap bm = new Bitmap(img);//ESBasic.Helpers.ImageHelper.Zoom(img, 0.6f);
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

        /// <summary>
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
                AsposeLicenseHelper.SetPdfLicense();
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

                string imageNamePrefix = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(originFilePath));
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    if (this.cancelled)
                    {
                        break;
                    }

                    MemoryStream stream = new MemoryStream();
                    //string imgPath = Path.Combine(imageOutputDirPath, imageNamePrefix) + "_" + i.ToString("000") + ".Png";
                    string imgPath = Path.Combine(imageOutputDirPath, "A") + "_" + i + ".Png";
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

        /// <summary>
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
                AsposeLicenseHelper.SetSlidesLicense();
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

                //先将ppt转换为pdf临时文件
                string tmpPdfPath = originFilePath + ".pdf";
                doc.Save(tmpPdfPath, Aspose.Slides.Export.SaveFormat.Pdf);

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

    #region Excel
    public class Xls2ImageConverter : IImageConverter
    {
        private Xls2ImageConverter xls2ImageConverter;
        public event CbGeneric<int, int> ProgressChanged;
        public event CbGeneric ConvertSucceed;
        public event CbGeneric<string> ConvertFailed;

        public void Cancel()
        {
            if (this.xls2ImageConverter != null)
            {
                this.xls2ImageConverter.Cancel();
            }
        }

        public void ConvertToImage(string originFilePath, string imageOutputDirPath)
        {
            ConvertToImage(originFilePath, imageOutputDirPath, 0, 0, 200);
        }

        private void ConvertToImage(string originFilePath, string imageOutputDirPath, int startPageNum, int endPageNum, int resolution)
        {
            try
            {
                AsposeLicenseHelper.SetCellsLicense();
                Aspose.Cells.Workbook excel = new Workbook(originFilePath);

                if (excel == null)
                {
                    throw new Exception("excel文件无效或者excel文件被加密！");
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

                if (endPageNum > excel.Worksheets.Count || endPageNum <= 0)
                {
                    endPageNum = excel.Worksheets.Count;
                }

                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum; startPageNum = endPageNum; endPageNum = startPageNum;
                }

                if (resolution <= 0)
                {
                    resolution = 128;
                }

                //先将excel转换为pdf临时文件
                string tmpPdfPath = originFilePath + ".pdf";
                excel.Save(tmpPdfPath, Aspose.Cells.SaveFormat.Pdf);

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

            this.xls2ImageConverter = null;
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
}
