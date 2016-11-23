using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Net.Plugins
{
    /// <summary>
    /// 重写List ToString方法
    /// 作者：唐晓军
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GList<T> : List<T> where T : class
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);

            //using (MemoryStream menoryStream = new MemoryStream())
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ZTree));
            //    serializer.WriteObject(menoryStream, this);
            //    return Encoding.UTF8.GetString(menoryStream.ToArray());
            //}
        }
    }
}
