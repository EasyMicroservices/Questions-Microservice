using EasyMicroservices.Cores.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Database.Schemas;
using System.Collections.Generic;

namespace EasyMicroservices.QuestionsMicroservice.Database.Entities
{
    public class QuestionEntity : QuestionSchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public ICollection<AnswerEntity> Answers { get; set; }
    }
}