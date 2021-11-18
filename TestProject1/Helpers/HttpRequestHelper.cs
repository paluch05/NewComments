using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;

namespace TestProject1.Helpers
{
    internal static class HttpRequestHelper
    {
        public static Mock<HttpRequest> CreateMockRequest(object body,
            Dictionary<string, StringValues> queryStringValues = null)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(n => n.Body).Returns(ms);

            if (queryStringValues != null)
            {
                mockRequest.Setup(n => n.Query).Returns(() => new QueryCollection(queryStringValues));
            }

            return mockRequest;
        }
    }
}
