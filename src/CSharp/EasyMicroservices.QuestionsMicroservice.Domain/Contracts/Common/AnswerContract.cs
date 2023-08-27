using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using EasyMicroservices.ContentsMicroservice.Clients.Attributes;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Common
{
    public class AnswerContract : IUniqueIdentitySchema, IDateTimeSchema, ISoftDeleteSchema
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }

        [ContentLanguage]
        public string Content { get; set; }

        public string UniqueIdentity { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
