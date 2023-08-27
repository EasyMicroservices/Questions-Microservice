using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Responses
{
    public class GetAllQuestionsWithAnswersResponseContract
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string UniqueIdentity { get; set; }
        public List<AnswerContract> Answers { get; set; }
    }
}
