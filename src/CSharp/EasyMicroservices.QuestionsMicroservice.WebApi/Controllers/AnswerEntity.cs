using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Contracts.Requests;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Contracts.Requests;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class AnswerController : SimpleQueryServiceController<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long>
    {
        private readonly IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> _questionlogic;
        private readonly IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> _contractlogic;

        public AnswerController(IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> questionlogic, IContractLogic<AnswerEntity, CreateAnswerRequestContract, UpdateAnswerRequestContract, AnswerContract, long> contractLogic) : base(contractLogic)
        {
            _contractlogic = contractLogic;
            _questionlogic = questionlogic;
        }
        public override async Task<MessageContract<long>> Add(CreateAnswerRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkQuestionId = await _questionlogic.GetById(new GetIdRequestContract<long>() { Id = request.QuestionId });
            if (checkQuestionId.IsSuccess)
                return await base.Add(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "QuestionId is incorrect");

        }
        public override async Task<MessageContract<AnswerContract>> Update(UpdateAnswerRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkQuestionId = await _questionlogic.GetById(new GetIdRequestContract<long>() { Id = request.QuestionId });
            if (checkQuestionId.IsSuccess)
            return await base.Update(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "QuestionId is incorrect");
        }
    }
}
