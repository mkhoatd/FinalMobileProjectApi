using FluentValidation.Results;

namespace Api.PreProcessor;

public class HeaderRequestLogger : IGlobalPreProcessor
{
    public async Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var logger = ctx.Resolve<ILogger<HeaderRequestLogger>>();
        foreach (var header in ctx.Request.Headers)
        {
            logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
        }
        logger.LogInformation("Request Method: {Method}", ctx.Request.Method);
        logger.LogInformation("Request Scheme: {Scheme}", ctx.Request.Scheme);
        logger.LogInformation("Request Path: {Path}", ctx.Request.Path);
    }
}


// public class HeaderRequestLogger<TRequest> : IPreProcessor<TRequest>
// {
//     public Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
//     {

//     }
//
// }