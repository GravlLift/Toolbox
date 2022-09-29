using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Toolbox
{
    public class DependencyBodyInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is DependencyTelemetry dependencyTelemetry)
            {
                // Try to get request
                if (dependencyTelemetry.TryGetOperationDetail("HttpRequest", out var requestObject))
                {
                    if (requestObject is HttpRequestMessage request)
                    {
                        foreach (var header in request.Headers)
                        {
                            dependencyTelemetry.Properties.AddOrReplace(
                                $"RequestHeaders_{header.Key}",
                                string.Join(", ", header.Value)
                            );
                        }

                        if (request.Content != null)
                        {
                            foreach (var header in request.Content.Headers)
                            {
                                dependencyTelemetry.Properties.AddOrReplace(
                                    $"RequestContentHeaders_{header.Key}",
                                    string.Join(", ", header.Value)
                                );
                            }
                            //if (request.Content is StringContent
                            //    || IsTextBasedContentType(request.Headers)
                            //    || IsTextBasedContentType(request.Content.Headers))
                            //{
                            //    using MemoryStream ms = new();
                            //    request.Content.CopyTo(ms, null, default);
                            //    var byteArray = new byte[ms.Length];
                            //    ms.Read(byteArray, 0, (int)ms.Length);
                            //    dependencyTelemetry.Properties.AddOrReplace("RequestContentBody", Encoding.Default.GetString(byteArray));
                            //}
                        }
                    }
                }
                // Try to get response
                if (
                    dependencyTelemetry.TryGetOperationDetail(
                        "HttpResponse",
                        out var responseObject
                    )
                )
                {
                    if (responseObject is HttpResponseMessage response)
                    {
                        foreach (var header in response.Headers)
                        {
                            dependencyTelemetry.Properties.AddOrReplace(
                                $"ResponseHeaders_{header.Key}",
                                string.Join(", ", header.Value)
                            );
                        }

                        if (response.Content != null)
                        {
                            foreach (var header in response.Content.Headers)
                            {
                                dependencyTelemetry.Properties.AddOrReplace(
                                    $"ResponseContentHeaders_{header.Key}",
                                    string.Join(", ", header.Value)
                                );
                            }
                            //if (response.Content is StringContent
                            //    || IsTextBasedContentType(response.Headers)
                            //    || IsTextBasedContentType(response.Content.Headers))
                            //{
                            //    using MemoryStream ms = new();
                            //    response.Content.CopyTo(ms, null, default);
                            //    var byteArray = new byte[ms.Length];
                            //    ms.Read(byteArray, 0, (int)ms.Length);
                            //    dependencyTelemetry.Properties.AddOrReplace("ResponseContentBody", Encoding.Default.GetString(byteArray));
                            //}
                        }
                    }
                }
            }
        }

        private static bool IsTextBasedContentType(HttpHeaders headers)
        {
            if (!headers.TryGetValues("Content-Type", out var values))
                return false;

            var header = string.Join(" ", values).ToLowerInvariant();
            var textBasedTypes = new[]
            {
                "html",
                "text",
                "xml",
                "json",
                "txt",
                "x-www-form-urlencoded"
            };
            return textBasedTypes.Any(t => header.Contains(t));
        }
    }
}
