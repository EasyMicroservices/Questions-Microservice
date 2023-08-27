using CodeReviewer.Engine;
using Contents.GeneratedServices;
using EasyMicroservices.QuestionsMicroservice.Contracts.Common;
using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.QuestionsMicroservice.Helpers;
using EasyMicroservices.QuestionsMicroservice.WebApi.Controllers;
using EasyMicroservices.QuestionsMicroservice.WebApi;
using EasyMicroservices.Tests;
using System;
using System.IO;
using System.Linq;

namespace EasyMicroservices.QuestionsMicroservice.Tests
{
    public class CodeReviewerCheckTests : CodeReviewerTests
    {
        static CodeReviewerCheckTests()
        {
            AssemblyManager.AddAssemblyToReview(
                typeof(ApplicationManager),
                typeof(StartUp),
                typeof(AnswerEntity),
                typeof(AnswerContract),
                typeof(AnswerController)
            );
        }
    }
}