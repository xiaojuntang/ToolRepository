using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcPatterns.HttpModulDemo
{
    /// <summary>
    /// http://www.tuicool.com/articles/UBrqQ3
    /// </summary>
    public class SiteStatModule : IHttpModule
    {
        private const string Html_CONTENT_TYPE = "text/html";

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.ReleaseRequestState += OnReleaseRequestState;
        }

        #endregion

        public void OnReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpResponse response = app.Response;
            string contentType = response.ContentType.ToLowerInvariant();
            if (response.StatusCode == 200 && !string.IsNullOrEmpty(contentType) && contentType.Contains(Html_CONTENT_TYPE))
            {
                response.Filter = new SiteStatResponseFilter(response.Filter);
            }
        }
    }

    public abstract class AbstractHttpResponseFilter : Stream
    {
        protected readonly Stream _responseStream;

        protected long _position;

        protected AbstractHttpResponseFilter(Stream responseStream)
        {
            _responseStream = responseStream;
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override long Length { get { return 0; } }

        public override long Position { get { return _position; } set { _position = value; } }

        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteCore(buffer, offset, count);
        }

        protected abstract void WriteCore(byte[] buffer, int offset, int count);

        public override void Close()
        {
            _responseStream.Close();
        }

        public override void Flush()
        {
            _responseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            _responseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }
    }



    public class SiteStatResponseFilter : AbstractHttpResponseFilter
    {
        private static readonly string END_HTML_TAG_NAME = "</body>";

        private static readonly string SCRIPT_PATH = "DearBruce.ModifyResponseSteamInHttpModule.CoreLib.site-tongji.htm";

        private static readonly string SITE_STAT_SCRIPT_CONTENT = "";

        static SiteStatResponseFilter()
        {
            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(SCRIPT_PATH);
            if (stream == null)
            {
                throw new FileNotFoundException(string.Format("The file \"{0}\" not found in assembly", SCRIPT_PATH));
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                SITE_STAT_SCRIPT_CONTENT = reader.ReadToEnd();
                reader.Close();
            }
        }

        public SiteStatResponseFilter(Stream responseStream)
            : base(responseStream)
        {

        }

        protected override void WriteCore(byte[] buffer, int offset, int count)
        {
            string strBuffer = Encoding.UTF8.GetString(buffer, offset, count);
            strBuffer = AppendSiteStatScript(strBuffer);
            byte[] data = Encoding.UTF8.GetBytes(strBuffer);
            _responseStream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 附加站点统计脚本
        /// </summary>
        /// <param name="strBuffer"></param>
        /// <returns></returns>
        protected virtual string AppendSiteStatScript(string strBuffer)
        {
            if (string.IsNullOrEmpty(strBuffer))
            {
                return strBuffer;
            }
            int endHtmlTagIndex = strBuffer.IndexOf(END_HTML_TAG_NAME, StringComparison.InvariantCultureIgnoreCase);
            if (endHtmlTagIndex <= 0)
            {
                return strBuffer;
            }
            return strBuffer.Insert(endHtmlTagIndex, SITE_STAT_SCRIPT_CONTENT);
        }
    }
}