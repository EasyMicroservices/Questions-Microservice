using EasyMicroservices.Cores.Interfaces;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class GetAllQuestionsWithAnswersRequestContract : IUniqueIdentitySchema
    {
        public string LanguageName { get; set; }
        public string UniqueIdentity { get; set; }
    }
}