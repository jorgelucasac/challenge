using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Middleware;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class UnhandledExceptionMiddlewareTests
{
    [Fact(DisplayName = "Middleware should return generic 500 in non-development environments")]
    public async Task Given_ProductionEnvironment_When_UnhandledExceptionOccurs_Then_ShouldHideExceptionDetails()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new UnhandledExceptionMiddleware(
            _ => throw new InvalidOperationException("Database connection failed"),
            NullLogger<UnhandledExceptionMiddleware>.Instance,
            new FakeHostEnvironment { EnvironmentName = "Production" });

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var payload = await JsonSerializer.DeserializeAsync<ApiResponse>(context.Response.Body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        payload.Should().NotBeNull();
        payload!.Success.Should().BeFalse();
        payload.Message.Should().Be("An unexpected error occurred.");
    }

    [Fact(DisplayName = "Middleware should return exception message in development")]
    public async Task Given_DevelopmentEnvironment_When_UnhandledExceptionOccurs_Then_ShouldExposeExceptionMessage()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new UnhandledExceptionMiddleware(
            _ => throw new InvalidOperationException("Database connection failed"),
            NullLogger<UnhandledExceptionMiddleware>.Instance,
            new FakeHostEnvironment { EnvironmentName = Environments.Development });

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var payload = await JsonSerializer.DeserializeAsync<ApiResponse>(context.Response.Body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        payload.Should().NotBeNull();
        payload!.Success.Should().BeFalse();
        payload.Message.Should().Be("Database connection failed");
    }

    private sealed class FakeHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = "Ambev.DeveloperEvaluation.WebApi";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
