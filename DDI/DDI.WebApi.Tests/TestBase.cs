using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using DDI.Shared;
using DDI.Shared.Data;
using DDI.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests
{
    public class TestBase
    {
        private static bool _initialized = false;

        public virtual void Initialize()
        {
            if (!_initialized)
            {
                Factory.ConfigureForTesting();
                Factory.RegisterRepositoryFactory<RepositoryFactoryNoDb>();
                Factory.RegisterServiceFactory<DDI.Services.ServiceFactory>();
                _initialized = true;
            }
        }

        /// <summary>
        /// Create a controller and initialize the Http properties.
        /// </summary>
        /// <typeparam name="T">Controller type</typeparam>
        /// <param name="unitOfWork">A UnitOfWork that will contain a repository with a pre-populated data source.</param>
        public T CreateController<T>(IUnitOfWork unitOfWork) where T : ApiController
        {
            T controller = Factory.CreateController<T>(unitOfWork);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            return controller;
        }

        /// <summary>
        /// Get the response object from an IHttpActionResult returned by the controller.
        /// </summary>
        /// <param name="httpActionResult">The IHttpActionResult object returned by the controller.</param>
        public IDataResponse<object> GetResponse(IHttpActionResult httpActionResult)
        {
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<IDataResponse>), "Result is Ok");

            var okResult = (OkNegotiatedContentResult<IDataResponse>)httpActionResult;

            Assert.IsInstanceOfType(okResult.Content, typeof(IDataResponse<object>), "Content is the correct type.");
            
            return (IDataResponse<object>)okResult.Content;
        }

        /// <summary>
        /// Get the response data (normally a list of dynamic objects) from an IDataReponse object.
        /// </summary>
        /// <param name="dataResponse">An IDataReponse object returned by a controller.</param>
        public IList<dynamic> GetReponseData(IDataResponse<object> dataResponse)
        {
            return (dataResponse.Data as IList<ExpandoObject>).Cast<dynamic>().ToList();
        }

        /// <summary>
        /// Get the response data (normally a list of dynamic objects) from an IHttpActionResult object.
        /// </summary>
        /// <param name="dataResponse">An IHttpActionResult object returned by a controller.</param>
        public IList<dynamic> GetResponseData(IHttpActionResult result)
        {
            return GetReponseData(GetResponse(result));
        }

        /// <summary>
        /// Get the single response entity from an IHttpActionResult object.
        /// </summary>
        /// <param name="dataResponse">An IHttpActionResult object returned by a controller.</param>
        public T GetResponseEntity<T>(IHttpActionResult httpActionResult)
        {
            IDataResponse<object> result = GetResponse(httpActionResult);

            Assert.IsInstanceOfType(result.Data, typeof(T), $"Content data is {typeof(T).Name}.");

            return (T)result.Data;
        }
    }
}
