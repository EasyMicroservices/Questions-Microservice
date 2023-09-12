using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice.Contracts.Requests
{
    public class UpdateQuestionRequestContract
    {
        [JsonIgnore]
        [BindNever]
        public string UniqueIdentity { get; set; }

        public long Id { get; set; }
        public List<LanguageDataContract> Titles { get; set; }

    }
}
