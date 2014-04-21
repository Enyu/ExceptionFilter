using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using DPE.ExceptionFilter.Service.Controllers;
using FluentAssertions;
using NUnit.Framework;

namespace DEP.ExceptionFilter.Tests
{
    public class ExceptionFilterTests
    {
        private DPE.ExceptionFilter.ExceptionFilter _exceptionFilter;
        private ApplesController _applesController;
        private string _uri;
        private const string LogPath = @"C:\log\error.txt";
        private const string RequestBody = "request Body";
        private const string Exception = "Exception";

        [SetUp]
        public void SetUp()
        {
            _exceptionFilter = new DPE.ExceptionFilter.ExceptionFilter();
            _applesController = new ApplesController();
            _uri = "http://localhost/someUri/mogul";
        }

        [Test]
        public void should_log_error_successfully()
        {
            var httpControllerContext = GenerateControllerContext(RequestBody);
            var actionContext = new HttpActionContext {ControllerContext = httpControllerContext};

            var executedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Exception = new Exception(Exception)
            };

            _exceptionFilter.OnException(executedContext);

            var savedLog = TestHelper.TestHelper.ReadFile(LogPath);
            savedLog.Should().NotBeNull();
            savedLog.Contains("RequestUrl : " + _uri).Should().BeTrue();
            savedLog.Contains("Headers : {\"foo\":[\"bar\"]}").Should().BeTrue();
            savedLog.Contains("RequestBody : " + RequestBody).Should().BeTrue();
            savedLog.Contains("Message : " + Exception).Should().BeTrue();
        }

        private HttpControllerContext GenerateControllerContext(string content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, _uri) { Content = new StringContent(content) };
            request.Headers.Add("foo", "bar");
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _applesController.Request = request;
            var httpControllerContext = new HttpControllerContext
            {
                Request = request,
                Controller = _applesController,
                ControllerDescriptor = new HttpControllerDescriptor { ControllerName = "ApplesController" }
            };
            return httpControllerContext;
        }

        [TearDown]
        public void TearDown()
        {
            TestHelper.TestHelper.RemoveFile(LogPath);
        }
    }
}
