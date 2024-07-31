using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ASecretCouncil.Tests.Fixtures;

public static class TestLoggerProvider
{
    public static ILogger<T> Logger<T>(ITestOutputHelper testOutputHelper)
    {
        return XUnitLogger.CreateLogger<T>(testOutputHelper);
    }
}
