using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace WCF.Entity
{
    [DataContract]
    public class FaultDetails
    {
        [DataMember]
        public string ErrorCode;

        [DataMember]
        public string Message;
    }
}
