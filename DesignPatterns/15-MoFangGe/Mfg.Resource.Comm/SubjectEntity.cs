using System;

namespace Mfg.Resource.Comm
{
    /// <summary>
    /// 科目的实体
    /// </summary>
    [Serializable]
    public class SubjectEntity
    {
        /// <summary>
        /// 科目id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string Name { get; set; }
    }

}
