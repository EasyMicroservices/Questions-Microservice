using Contents.GeneratedServices;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Contracts.Requests;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Contracts.Requests;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class AnswerController : SimpleQueryServiceController<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long>
    {
        private readonly IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> _questionlogic;
        private readonly IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> _contractlogic;
        private readonly IConfiguration _config;
        private readonly ContentClient _contentClient;
        private string _contentRoot;

        public AnswerController(IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> questionlogic, IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> contractLogic, IConfiguration config) : base(contractLogic)
        {
            _contractlogic = contractLogic;
            _questionlogic = questionlogic;

            _config = config;

            _contentRoot = _config.GetValue<string>("RootAddresses:Content");
            _contentClient = new(_contentRoot, new HttpClient());
        }
        public override async Task<MessageContract<long>> Add(CreateAnswerRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkQuestionId = await _questionlogic.GetById(new GetIdRequestContract<long>() { Id = request.QuestionId });
            if (checkQuestionId.IsSuccess)
            {
                var addAnswerResult = await base.Add(request, cancellationToken);
                if (addAnswerResult)
                {
                    var getAnswerResult = await base.GetById(new GetIdRequestContract<long>
                    {
                        Id = addAnswerResult.Result
                    }, cancellationToken);

                    var addContentResult = await _contentClient.AddContentWithKeyAsync(new AddContentWithKeyRequestContract
                    {
                        Key = $"{getAnswerResult.Result.UniqueIdentity}-Content",
                        LanguageData = request.Contents.Select(x => new Contents.GeneratedServices.LanguageDataContract
                        {
                            Language = x.LanguageName,
                            Data = x.Data
                        }).ToList(),
                    });

                    if (addContentResult.IsSuccess)
                        return addAnswerResult.Result;

                    await _contentClient.HardDeleteByIdAsync(new Int64DeleteRequestContract
                    {
                        Id = addAnswerResult.Result
                    });
                    return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");

                }

                await _contentClient.HardDeleteByIdAsync(new Int64DeleteRequestContract
                {
                    Id = addAnswerResult.Result
                });
                return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "An error has occured.");

            }

            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "QuestionId is incorrect");
        }

        public override async Task<MessageContract<AnswerContract>> Update(UpdateAnswerRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkQuestionId = await _questionlogic.GetById(new GetIdRequestContract<long>() { Id = request.QuestionId });
            if (checkQuestionId.IsSuccess)
            {
                var answerResult = await _contractlogic.GetById(new GetIdRequestContract<long> { Id = request.Id });

                if (answerResult)
                {
                    var UpdateResponse = await _contentClient.UpdateContentWithKeyAsync(new AddContentWithKeyRequestContract
                    {
                        Key = $"{answerResult.Result.UniqueIdentity}-Content",
                        LanguageData = request.Content.Select(x => new Contents.GeneratedServices.LanguageDataContract
                        {
                            Language = x.LanguageName,
                            Data = x.Data
                        }).ToList(),
                    });

                    if (UpdateResponse.IsSuccess)
                        return answerResult;

                    return ((EasyMicroservices.ServiceContracts.FailedReasonType)UpdateResponse.Error.FailedReasonType, UpdateResponse.Error.Message);

                }

                return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "Answer Id is incorrect");

            }

            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "QuestionId is incorrect");
        }

    }
}
