using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCF.Service
{
    [ServiceContract]
    public interface IHello
    {
        [OperationContract]
        string SayHello(string name);

        [OperationContract(Name = "SayHelloBySex")]
        string SayHello(string name, string sex);

    }
}
