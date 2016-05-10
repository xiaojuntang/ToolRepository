using Climb.Core;
using System;

namespace Mfg.Resouce.Models
{
    /// <summary>
    /// 问题类型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Serializable]
    public class QuestionStyle<TKey> : BaseEntity
    {
        public QuestionStyle()
        { }
        #region Model
        private string _f_name;
        private string _f_subject;
        private string _f_blcass;
        private int? _f_styleareid;
        private string _f_styleareaname;

        /// <summary>
        /// 
        /// </summary>
        public string f_name
        {
            set { _f_name = value; }
            get { return _f_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string f_subject
        {
            set { _f_subject = value; }
            get { return _f_subject; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string f_blcass
        {
            set { _f_blcass = value; }
            get { return _f_blcass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? f_styleareid
        {
            set { _f_styleareid = value; }
            get { return _f_styleareid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string f_styleareaname
        {
            set { _f_styleareaname = value; }
            get { return _f_styleareaname; }
        }
        #endregion Model

        public TKey f_dataext { get; set; }
    }
}
