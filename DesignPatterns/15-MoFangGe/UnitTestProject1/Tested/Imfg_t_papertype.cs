using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Tested
{
    public interface Imfg_t_papertype : IDALBase<mfg_t_papertypeInfo>, IAutoIdentity<mfg_t_papertypeInfo>
    {
        /// <summary>
        /// 从主键读取信息
        /// </summary>
        /// <param name="pkID"></param>
        /// <returns>查询到的数据</returns>
        mfg_t_papertypeInfo GetInfoByPK(string pkID);
    }
}
