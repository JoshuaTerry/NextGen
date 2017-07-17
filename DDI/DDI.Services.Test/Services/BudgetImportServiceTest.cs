using DDI.Services.GL;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq.Expressions;
using System.Text;

namespace DDI.Services.Test.Services
{
    [TestClass]
    public class BudgetImportServiceTest
    {
        private const string TESTDESCR = "Services | BudgetImport";

        [TestMethod, TestCategory(TESTDESCR)]
        public void ImportBudgets_OneBudgetImported()
        {
            Guid groupId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6922");

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetById<ImportFile>(It.IsAny<Guid>(), It.IsAny<Expression<Func<ImportFile, object>>[]>())).Returns(CreateImportFile());


            var service = new BudgetImportService(uow.Object);
        }

        private ImportFile CreateImportFile()
        {
            var importFile = new ImportFile();
            var file = new FileStorage();

            var contents = new StringBuilder();
            contents.AppendLine("Business Unit,Fiscal Year,Account,Budget,Period 1,Period 2,Period 3,Period 4,Period 5,Period 6,Period 7,Period 8,Period 9,Period 10,Period 11,Period 12,Period 13");
            contents.AppendLine("DCEF,2016,01-305-45-60,What If Budget,12.54,45.78,-65.78,0.78,0,0,0,0,0,0,0,0,0");
            file.Data = Encoding.ASCII.GetBytes(contents.ToString());
            file.Extension = "csv";
            file.FileType = "csv";
            file.Name = "TestFile";



            return null;
        }
    }
}
