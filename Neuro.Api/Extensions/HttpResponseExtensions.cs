using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Base.Result;

namespace Neuro.Api.Extensions;

public static class HttpResponseExtensions
{
    public static IActionResult ToActionResult(this Result response)
    {
        return response.IsSuccess ? new OkObjectResult(response) : new BadRequestObjectResult(response);
    }

}