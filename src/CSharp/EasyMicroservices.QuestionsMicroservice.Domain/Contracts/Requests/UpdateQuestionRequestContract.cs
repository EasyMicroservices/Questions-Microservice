using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class UpdateQuestionRequestContract
    {
        public long Id { get; set; }
        public List<LanguageDataContract> Titles { get; set; }
        public string UniqueIdentity { get; set; }

    }
}
