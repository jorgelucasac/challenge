using System.Text.Json;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Middleware;

namespace Ambev.DeveloperEvaluation.Unit.WebApi;

public class ValidationExceptionMiddlewareTests
{
    [Fact(DisplayName = "Middleware should return 404 when key not found exception is thrown")]
    public async Task Given_KeyNotFoundException_When_Invoked_Then_ShouldReturnNotFound()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ValidationExceptionMiddleware(_ => throw new KeyNotFoundException("Sale with ID 123 not found"));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var payload = await JsonSerializer.DeserializeAsync<ApiResponse>(context.Response.Body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        payload.Should().NotBeNull();
        payload!.Success.Should().BeFalse();
        payload.Message.Should().Be("Sale with ID 123 not found");
    }

    [Fact(DisplayName = "Middleware should return 400 when validation exception is thrown")]
    public async Task Given_ValidationException_When_Invoked_Then_ShouldReturnBadRequest()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var errors = new[]
        {
            new FluentValidation.Results.ValidationFailure("Id", "Id is required")
        };
        var middleware = new ValidationExceptionMiddleware(_ => throw new ValidationException(errors));

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var payload = await JsonSerializer.DeserializeAsync<ApiResponse>(context.Response.Body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        payload.Should().NotBeNull();
        payload!.Success.Should().BeFalse();
        payload.Message.Should().Be("Validation Failed");
    }
}
