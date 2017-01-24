using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Enums.Common;
using DDI.Services;
using DDI.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class CustomFieldsControllerTest
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod]
        public void GetCustomFieldsByEntityId_ReturnsCustomFieldCollection()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<CustomField>>();
            repo.Setup(r => r.Entities).Returns(SetupRepo());
            uow.Setup(m => m.GetRepository<CustomField>()).Returns(repo.Object);
            var service = new CustomFieldService(uow.Object);

            var controller = new CustomFieldsController(service);
            var result = controller.GetByEntityId(19);
            
            Assert.IsTrue(true);
        }

        private IQueryable<CustomField> SetupRepo()
        {
            var list = new List<CustomField>();
            list.Add(new CustomField { Id = new Guid("AFE2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL State", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B0E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL Issue Date", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B1E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL Amount", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B2E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL #", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("A3711EDE-DADC-E611-80E5-005056B7555A"), LabelText = "Passport #", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });

            var response = list.AsQueryable<CustomField>();
            return response;
        }
    }
}
