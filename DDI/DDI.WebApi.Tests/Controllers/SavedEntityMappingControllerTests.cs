using DDI.Services;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Models.Client.Core;
using DDI.WebApi.Controllers.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]

    public class SavedEntityMappingControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAllSavedEntityMappings_ReturnsSavedEntityMappingCollection()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<SavedEntityMapping>(null)).Returns(SetupRepo());

            var service = new ServiceBase<SavedEntityMapping>(uow.Object);
            var controller = new SavedEntityMappingController(service);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(((contentResult.Content as DataResponse<object>).Data as List<DDI.Shared.Models.ICanTransmogrify>).Count, 1);

        }
        private IQueryable<SavedEntityMapping> SetupRepo()
        {
            var list = new List<SavedEntityMapping>();
            list.Add(new SavedEntityMapping
            {
                Id = Guid.Parse("64C5C696-A036-4365-A379-F5CA179B55FC"),
                Description = "Test Mapping",
                MappingType = EntityMappingType.Budget,
                MappingFields = new SavedEntityMappingField[]
                {
                    new SavedEntityMappingField()
                    {
                        Id = Guid.Parse("BA177023-9259-4ED2-833C-71F348FA4111"),
                        ColumnName = "Amount",
                        SavedEntityMappingId = Guid.Parse("64C5C696-A036-4365-A379-F5CA179B55FC"),
                        EntityMapping = new EntityMapping()
                        {
                            Id = Guid.Parse("27DE022D-AFEC-4427-A504-A5807D88BAD1"),
                            MappingType = EntityMappingType.Budget,
                            PropertyName = "YearAmount"
                        }
                    }
                }
            });
            
            var response = list.AsQueryable<SavedEntityMapping>();
            return response;
        }

    }
}
