using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace DDI.WebApi.IoC
{
    /// <summary>
    /// A custom message handler that captures the HttpRequestMessage
    /// </summary>
    public class MessageHandler : DelegatingHandler
    {
        public const string HttpResponseMessageName = "RequestMessage";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallContext.LogicalSetData(HttpResponseMessageName, request);

            return base.SendAsync(request, cancellationToken);
        }
    }

}