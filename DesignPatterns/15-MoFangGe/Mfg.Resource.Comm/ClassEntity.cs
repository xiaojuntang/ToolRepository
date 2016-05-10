using System;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 年级科目实体类
    /// </summary>
    [Serializable]
    public class ClassEntity
    {
        /// <summary>
        /// 小年级
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 年级的科目
        /// </summary>
        public string Name { get; set; }
    }
}
