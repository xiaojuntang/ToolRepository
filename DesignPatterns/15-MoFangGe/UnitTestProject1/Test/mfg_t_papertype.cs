using System;
namespace UnitTestProject1
{
	/// <summary>
	/// mfg_t_papertype:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class mfg_t_papertype
	{
		public mfg_t_papertype()
		{}
		#region Model
		private int _f_id;
		private string _f_name;
		private string _f_class;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int f_id
		{
			set{ _f_id=value;}
			get{return _f_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string f_name
		{
			set{ _f_name=value;}
			get{return _f_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string f_class
		{
			set{ _f_class=value;}
			get{return _f_class;}
		}
		#endregion Model

	}
}

