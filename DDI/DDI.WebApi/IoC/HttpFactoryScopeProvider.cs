using DDI.Shared.Interfaces;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;

namespace DDI.WebApi.IoC
{
    /// <summary>
    /// A Factory provider that returns the DisposableFactory associated with the current Http dependency scope.
    /// </summary>
    public class HttpFactoryScopeProvider : IFactoryProvider
    {
        public IFactory GetFactory()
        {
            HttpRequestMessage request = (HttpRequestMessage)CallContext.LogicalGetData(MessageHandler.HttpResponseMessageName);
            if (request == null)
            {
                return null;
            }
            return ((DependencyScope)request.GetDependencyScope()).GetFactory();
        }
    }
}