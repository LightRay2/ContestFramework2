using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer
{
    class LoggingPipelineModule :HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            MethodDescriptor method = invokerContext.MethodDescriptor;

            Debug.WriteLine("{0}.{1}({2}) threw the following uncaught exception: {3}",
                method.Hub.Name,
                method.Name,
                String.Join(", ", invokerContext.Args),
                exceptionContext.Error);
        }
    }
}
