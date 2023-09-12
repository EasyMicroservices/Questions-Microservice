using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class CreateAnswerRequestContract
    {
        public long QuestionId { get; set; }
        public List<LanguageDataContract> Contents { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
