using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class CreateAnswerRequestContract
    {
        public long QuestionId { get; set; }
        public string EnglishContent { get; set; }
        public string PersianContent { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
