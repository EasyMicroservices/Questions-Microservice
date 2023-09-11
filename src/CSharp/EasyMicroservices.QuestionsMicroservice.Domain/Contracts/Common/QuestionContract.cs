using EasyMicroservices.ContentsMicroservice.Clients.Attributes;
using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Common
{
    public class QuestionContract : IUniqueIdentitySchema, ISoftDeleteSchema, IDateTimeSchema
    {
        public long Id { get; set; }
        [ContentLanguage]
        public string Title { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string UniqueIdentity { get; set; }
        public bool IsDeleted { get; set; }

        public List<AnswerContract> Answers { get; set; }
    }
}
