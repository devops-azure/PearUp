using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Api.Diagnostics
{
    public class ErrorLoggingMiddleware
    {
        readonly RequestDelegate _next;
        static readonly ILogger Log = Serilog.Log.ForContext<ErrorLoggingMiddleware>();

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;
                PreserveStackTrace(ex);
                Log.ForContext("Exception", ex.GetType())
                    .ForContext("StackTrace", ex.StackTrace, true)
                    .Error(ex, ex.Message, ex.StackTrace);
                await ErrorLoggingResponse.HttpExceptionMessage(httpContext, ex);
            }
        }

        static void PreserveStackTrace(Exception e)
        {
            var ctx = new StreamingContext(StreamingContextStates.CrossAppDomain);
            var si = new SerializationInfo(typeof(Exception), new FormatterConverter());
            var ctor = typeof(Exception).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);

            e.GetObjectData(si, ctx);
            ctor.Invoke(e, new object[] { si, ctx });
        }

    }
}
