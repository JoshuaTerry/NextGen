using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Enums.Common;
using DDI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared;

namespace DDI.Services.Tests.Controllers
{
    [TestClass]
    public class CustomFieldsServiceTest
    {
        private const string TESTDESCR = "WebApi | Services";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetCustomFieldsByEntityId_ReturnsCustomFieldCollection()
        {
            var repo = new Mock<IRepository<CustomField>>();
            var unitOfWork = new UnitOfWorkNoDb();
            unitOfWork.SetRepository(repo.Object);
            repo.Setup(r => r.Entities).Returns(SetupRepo());

            var service = new CustomFieldService(unitOfWork);
            var result = service.GetByEntityId(19);
            
            Assert.IsTrue(result.Data.Count == 5);
        }

        private IQueryable<CustomField> SetupRepo()
        {
            var list = new List<CustomField>();
            list.Add(new CustomField { Id = new Guid("AFE2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL State", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B0E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL Issue Date", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B1E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL Amount", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("B2E2AC6C-1FDC-E611-80E5-005056B7555A"), LabelText = "DL #", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("A3711EDE-DADC-E611-80E5-005056B7555A"), LabelText = "Passport #", DecimalPlaces = 0, IsActive = true, IsRequired = false, DisplayOrder = 1, Entity = CustomFieldEntity.CRM, FieldType = CustomFieldType.Number });
            list.Add(new CustomField { Id = new Guid("A3711EDE-DADC-E611-80E5-005056B7555A"), LabelText = "Account #", DecimalPlaces = 0, IsActive = true, IsRequired = true, DisplayOrder = 1, Entity = CustomFieldEntity.Accounting, FieldType = CustomFieldType.Number });

            return list.AsQueryable();
        }
    }
}
