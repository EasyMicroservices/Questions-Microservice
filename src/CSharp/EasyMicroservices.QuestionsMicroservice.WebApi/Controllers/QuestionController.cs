using Contents.GeneratedServices;
using EasyMicroservices.ContentsMicroservice.Clients.Helpers;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Contracts.Requests;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Contracts.Requests;
using EasyMicroservices.QuestionsMicroservice.Contracts.Responses;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

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
        public override async Task<MessageContract<long>> Add(CreateQuestionRequestContract request, CancellationToken cancellationToken = default)
        {
            var addQuestionResult = await base.Add(request, cancellationToken);
            if (addQuestionResult.IsSuccess)
            {
                var getQuestionId = await base.GetById(new Cores.Contracts.Requests.GetIdRequestContract<long> { Id = addQuestionResult.Result });
                var addContent = await _contentClient.AddContentWithKeyAsync(new AddContentWithKeyRequestContract
                {
                    Key = $"{getQuestionId.Result.UniqueIdentity}-Title",
                    LanguageData = request.Titles.Select(x => new Contents.GeneratedServices.LanguageDataContract
                    {
                        Data = x.Data,
                        Language = x.LanguageName,
                    }).ToList(),
                });
                if (addContent.IsSuccess)
                    return addQuestionResult.Result;

                await _contentClient.HardDeleteByIdAsync(new Int64DeleteRequestContract
                {
                    Id = addQuestionResult.Result
                });
                return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");
            }
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");

        }
        public override async Task<MessageContract<QuestionContract>> Update(UpdateQuestionRequestContract request, CancellationToken cancellationToken = default)
        {
            var getQuestion = await base.GetById(new Cores.Contracts.Requests.GetIdRequestContract<long> { Id = request.Id });

            if (getQuestion.IsSuccess)
            {
                request.UniqueIdentity = getQuestion.Result.UniqueIdentity;
                var updateQuestion = await _contractlogic.Update(request, cancellationToken);
                if (!updateQuestion.IsSuccess)
                    return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");

                if (updateQuestion.IsSuccess)
                {
                    var getQuestionId = await base.GetById(new Cores.Contracts.Requests.GetIdRequestContract<long> { Id = updateQuestion.Result.Id });
                    var addContent = await _contentClient.UpdateContentWithKeyAsync(new AddContentWithKeyRequestContract
                    {
                        Key = $"{getQuestionId.Result.UniqueIdentity}-Title",
                        LanguageData = request.Titles.Select(x => new Contents.GeneratedServices.LanguageDataContract
                        {
                            Data = x.Data,
                            Language = x.LanguageName,
                        }).ToList(),
                    });
                    if (addContent.IsSuccess)
                        return updateQuestion.Result;
                    await _contentClient.HardDeleteByIdAsync(new Int64DeleteRequestContract
                    {
                        Id = updateQuestion.Result.Id
                    });
                    return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");
                }
            }

            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");

        }
        [HttpPost]
        public async Task<ListMessageContract<GetAllQuestionsWithAnswersResponseContract>> GetAllQuestionsWithAnswers(GetAllQuestionsWithAnswersRequestContract request)
        {
            var questions = await _contractlogic.GetAllByUniqueIdentity(request, query => query.Include(x => x.Answers));
            if (!questions)
                return questions.ToListContract<GetAllQuestionsWithAnswersResponseContract>();


            var task = questions.Result.Select(async o => new GetAllQuestionsWithAnswersResponseContract
            {
                Id = o.Id,
                Title = await ResolveQuestionContent(o, request.LanguageName),
                UniqueIdentity = o.UniqueIdentity,
                Answers = Task.WhenAll(o.Answers.Select(async x => new AnswerContract
                {
                    Id = x.Id,
                    Content = await ResolveAnswerContent(x, request.LanguageName),
                    CreationDateTime = x.CreationDateTime,
                    DeletedDateTime = x.DeletedDateTime,
                    IsDeleted = x.IsDeleted,
                    ModificationDateTime = x.ModificationDateTime,
                    QuestionId = x.QuestionId,
                    UniqueIdentity = x.UniqueIdentity
                })).Result.ToList()
            }).ToList();
            var questionsWithAnswers = (await Task.WhenAll(task)).ToList();
            return questionsWithAnswers;
        }

        [HttpPost]
        public async Task<MessageContract<QuestionResponseContract>> GetQuestionById(GetIdRequestContract<long> request)
        {
            var question = await _contractlogic.GetById(request, query => query.Include(x => x.Answers));
            if (!question)
                return question.ToContract<QuestionResponseContract>();

            var questionTitle = await _contentClient.GetAllByKeyAsync(new GetAllByKeyRequestContract
            {
                Key = $"{question.Result.UniqueIdentity}-Title"
            });
            if (!questionTitle.IsSuccess)
                return questionTitle.ToContract<QuestionResponseContract>();
            List<AnswerResponseContract> asnwers = new List<AnswerResponseContract>();
            foreach (var answer in question.Result.Answers)
            {
                var answerContent = await _contentClient.GetAllByKeyAsync(new GetAllByKeyRequestContract
                {
                    Key = $"{answer.UniqueIdentity}-Content"
                });
                if (!answerContent.IsSuccess)
                    return answerContent.ToContract<QuestionResponseContract>();
                asnwers.AddRange(question.Result.Answers.Select(x => new AnswerResponseContract
                {
                    Id = x.Id,
                    Contents = answerContent.Result.Select(x => new Contracts.Common.LanguageDataContract()
                    {
                        Data = x.Data,
                        LanguageName = x.Language?.Name
                    }).ToList(),
                    CreationDateTime = x.CreationDateTime,
                    DeletedDateTime = x.DeletedDateTime,
                    IsDeleted = x.IsDeleted,
                    ModificationDateTime = x.ModificationDateTime,
                    QuestionId = x.QuestionId,
                    UniqueIdentity = x.UniqueIdentity
                }));
            }

            var questionResult = question.Result;
            return new QuestionResponseContract()
            {
                Id = questionResult.Id,
                Titles = questionTitle.Result.Select(x=>new Contracts.Common.LanguageDataContract()
                {
                     Data = x.Data,
                     LanguageName = x.Language?.Name
                }).ToList(),
                UniqueIdentity = questionResult.UniqueIdentity,
                CreationDateTime = questionResult.CreationDateTime,
                DeletedDateTime = questionResult.DeletedDateTime,
                IsDeleted = questionResult.IsDeleted,
                ModificationDateTime = questionResult.ModificationDateTime,
               Answers = asnwers
            };
        }

        private async Task<string> ResolveAnswerContent(AnswerContract request, string LanguageName)
        {
            ContentLanguageHelper contentHelper = new(_contentClient);
            await contentHelper.ResolveContentLanguage(request, LanguageName);
            var content = request.Content;
            return content;
        }
        private async Task<string> ResolveQuestionContent(QuestionContract request, string LanguageName)
        {
            ContentLanguageHelper contentHelper = new(_contentClient);
            await contentHelper.ResolveContentLanguage(request, LanguageName);
            var title = request.Title;
            return title;
        }
    }
}
