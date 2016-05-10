using System;

namespace Mfg.Resource.Comm
{

    /// <summary>
    /// 大年级实体类
    /// </summary>
    [Serializable]
    public class BClassEntity
    {
        /// <summary>
        /// 大年级id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 大年级名称
        /// </summary>
        public string Name { get; set; }
    }
}
