using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class UpdateAnswerRequestContract
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string Content { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
