using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace Template.Web.FunctionalTests;

public static class TestUtils
{
    public static readonly string LocalSettingsPath = "appsettings.Development.json";

    internal static Mock<ILogger> CreateMockLogger()
    {
        var mockLogger = new Mock<ILogger>();
        mockLogger
            .Setup(logger => logger.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()))
            .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((level, eventId, state, exception, formatter) =>
            {
                var message = formatter(state, exception);
                Console.WriteLine($"LogError: {message}");
            });

        mockLogger
            .Setup(logger => logger.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()))
            .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((level, eventId, state, exception, formatter) =>
            {
                var message = formatter(state, exception);
                Console.WriteLine($"LogInformation: {message}");
            });

        return mockLogger;
    }

    internal static void LoadEnvironmentVariablesFromLocalSettings(string path)
    {
        var json = JObject.Parse(File.ReadAllText(path));
        var values = json["Values"] as JObject;
        if (values != null)
        {
            foreach (var setting in values)
            {
                Environment.SetEnvironmentVariable(setting.Key, setting.Value.ToString());
            }
        }
    }

    internal static Mock<HttpRequest> CreateMockRequest(object body)
    {
        var json = JsonConvert.SerializeObject(body);
        var byteArray = Encoding.ASCII.GetBytes(json);
        var _memoryStream = new MemoryStream(byteArray);
        _memoryStream.Flush();
        _memoryStream.Position = 0;
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(_memoryStream);
        return mockRequest;
    }

    internal static Mock<HttpRequest> CreateMockRequest(string body)
    {
        var byteArray = Encoding.ASCII.GetBytes(body);
        var _memoryStream = new MemoryStream(byteArray);
        _memoryStream.Flush();
        _memoryStream.Position = 0;
        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.Body).Returns(_memoryStream);
        return mockRequest;
    }
}
