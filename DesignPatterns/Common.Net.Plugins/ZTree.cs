using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net.Plugins
{
    /// <summary>
    /// Jquery ZTree对象
    /// 作者：唐晓军
    /// </summary>
    [DataContract]
    public class ZTree
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember(Name = "id", Order = 1)]
        public int id { get; set; }

        /// <summary>
        /// 父结点
        /// </summary>
        [DataMember(Name = "pId")]
        public int pId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember(Name = "checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        [DataMember(Name = "open")]
        public bool IsOpen { get; set; }
    }
}
