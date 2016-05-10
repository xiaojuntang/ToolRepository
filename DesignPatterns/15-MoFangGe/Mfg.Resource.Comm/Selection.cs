using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 选项
    /// </summary>
    [Serializable]
    [DataContract]
    public class Selection
    {
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(IsRequired = false)]
        public string desc { get; set; }

        /// <summary>
        /// 内容数组
        /// </summary>
        [DataMember(IsRequired = false)]
        public string[] f_content { get; set; }
    }

    /// <summary>
    /// 获取选项
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetSelection
    {
        /// <summary>
        /// 获取选项List
        /// </summary>
        /// <param name="selection">选项json串</param>
        /// <returns> List<Selection></returns>
        public List<Selection> GetList(string selection)
        {
            if (string.IsNullOrEmpty(selection))
            {
                return new List<Selection>();
            }
            List<Selection> list = new List<Selection>();
            list = Getlists(selection, list);
            return list;
        }

        /// <summary>
        /// json转化
        /// </summary>
        /// <param name="selection"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Selection> Getlists(string selection, List<Selection> list)
        {
            list = JSON.Parse<List<Selection>>(selection);
            return list;
        }
    }
}
