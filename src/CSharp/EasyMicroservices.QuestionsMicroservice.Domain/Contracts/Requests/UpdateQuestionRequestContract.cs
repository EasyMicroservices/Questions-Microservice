using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class CreateQuestionRequestContract
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}
