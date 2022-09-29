using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace Toolbox
{
    public class RequestBodyTelemetryMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            var response = context.Response;
            if (response.StatusCode == 500)
            {
                if (request?.Body?.CanRead == true)
                {
                    request.EnableBuffering();

                    var bodySize = (int)(request.ContentLength ?? request.Body.Length);
                    if (bodySize > 0)
                    {
                        request.Body.Position = 0;

                        byte[] body;

                        using (var ms = new MemoryStream(bodySize))
                        {
                            await request.Body.CopyToAsync(ms);

                            body = ms.ToArray();
                        }

                        request.Body.Position = 0;

                        var requestTelemetry = context.Features.Get<RequestTelemetry>();
                        if (requestTelemetry != null)
                        {
                            var requestBodyString = Encoding.UTF8.GetString(body);
                            requestTelemetry.Properties.AddOrReplace("requestBody", requestBodyString);
                        }
                    }
                }
                if (response?.Body?.CanRead == true)
                {
                    var bodySize = (int)(response.ContentLength ?? response.Body.Length);
                    if (bodySize > 0)
                    {
                        response.Body.Position = 0;

                        byte[] body;

                        using (var ms = new MemoryStream(bodySize))
                        {
                            await response.Body.CopyToAsync(ms);

                            body = ms.ToArray();
                        }

                        response.Body.Position = 0;

                        var requestTelemetry = context.Features.Get<RequestTelemetry>();
                        if (requestTelemetry != null)
                        {
                            var responseBodyString = Encoding.UTF8.GetString(body);
                            requestTelemetry.Properties.AddOrReplace("responseBody", responseBodyString);
                        }
                    }
                }
            }

            await next(context);
        }
    }
}
