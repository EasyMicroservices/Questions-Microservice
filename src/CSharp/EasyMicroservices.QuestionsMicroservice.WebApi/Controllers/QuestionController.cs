using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Contracts.Requests;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class QuestionController : SimpleQueryServiceController<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long>
    {
        private readonly IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> _contractlogic;

        public QuestionController( IContractLogic<QuestionEntity, CreateQuestionRequestContract, UpdateQuestionRequestContract, QuestionContract, long> contractLogic) : base(contractLogic)
        {
            _contractlogic = contractLogic;
        }
    }
}
