using EasyMicroservices.Cores.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Database.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Database.Entities
{
    public class AnswerEntity : AnswerSchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public QuestionEntity Question { get; set; }
    }
}
