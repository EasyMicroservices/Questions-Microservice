using Contents.GeneratedServices;
using EasyMicroservices.ContentsMicroservice.Clients.Helpers;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Contracts.Requests;
using EasyMicroservices.QuestionsMicroservice.Contracts.Responses;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class QuestionController : SimpleQueryServiceController<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long>
    {
        private readonly IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> _contractlogic;
        private readonly IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> _answerLogic;
        public string _contentRoot;
        private readonly ContentClient _contentClient;
        private readonly IConfiguration _config;
        public QuestionController(IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> contractLogic, IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> answerLogic, IConfiguration config) : base(contractLogic)
        {
            _contractlogic = contractLogic;
            _answerLogic = answerLogic;

            _config = config;
            _contentRoot = _config.GetValue<string>("RootAddresses:Content");
            _contentClient = new(_contentRoot, new HttpClient());
        }

        [HttpPost]
        public async Task<ListMessageContract<GetAllQuestionsWithAnswersResponse>> GetAllQuestionsWithAnswers(GetAllQuestionsWithAnswersRequestContract request)
        {
            var questions = await _contractlogic.GetAllByUniqueIdentity(request, query => query.Include(x => x.Answers));
            if (!questions)
                return questions.ToListContract<GetAllQuestionsWithAnswersResponse>();


            var questionsWithAnswers = questions.Result.Select(o => new GetAllQuestionsWithAnswersResponse
            {
                Id = o.Id,
                Title = o.Title,
                UniqueIdentity = o.UniqueIdentity,
                Answers = Task.WhenAll(o.Answers.Select(async x => new AnswerContract
                {
                    Id = o.Id,
                    Content = await ResolveAnswerContent(x, request.LanguageName),
                    CreationDateTime = x.CreationDateTime,
                    DeletedDateTime = x.DeletedDateTime,
                    IsDeleted = x.IsDeleted,
                    ModificationDateTime = x.ModificationDateTime,
                    QuestionId = x.QuestionId,
                    UniqueIdentity = x.UniqueIdentity
                })).Result.ToList()
            }).ToList();

            return questionsWithAnswers;
        }

        private async Task<string> ResolveAnswerContent(AnswerContract request, string LanguageName)
        {
            ContentLanguageHelper contentHelper = new(_contentClient);
            await contentHelper.ResolveContentLanguage(request, LanguageName);
            var content = request.Content;
            return content;
        }
    }
}
