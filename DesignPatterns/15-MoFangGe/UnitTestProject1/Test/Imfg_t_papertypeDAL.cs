using Climb.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Test
{
    public interface Imfg_t_papertypeDAL
    {
        mfg_t_papertype FindSingle(Query query);

        List<mfg_t_papertype> FindList(Query query);

        List<mfg_t_papertype> FindList(Query query, int index, int count);

        int Update(Update update);
    }
}
