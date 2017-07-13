using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class BudgetImportService
    {
        //private IUnitOfWork _uow = new UnitOfWorkEF();
        //public void ImportBudgets(Guid fileId, MappableEntityField[] fields)
        //{
        //    var importFile = GetImportFile(fileId);
        //    if (!fields.Any(f => f.PropertyName == "Account"))
        //        throw new Exception("No Account Mapping Specified.");

        //    char[] delimiters = { ',' };

        //    string[] columns;
        //    using (var stream = new MemoryStream(importFile.File.Data))
        //    {
        //        using (var streamReader = new StreamReader(stream))
        //        {
        //            string line = streamReader.ReadLine();


        //            if (importFile.ContainsHeaders)
        //            {
        //                string[] columns;
        //                using (var stream = new MemoryStream(importFile.File.Data))
        //                {
        //                    using (var streamReader = new StreamReader(stream))
        //                    {
        //                        columns = streamReader.ReadLine().Split(',');
        //                        string line = string.Empty;
        //                        while ((line = streamReader.ReadLine()) != string.Empty)
        //                        {
        //                            string[] values = line.Split(delimiters, StringSplitOptions.None);

        //                            if (fields.Any(f => f.PropertyName == "Account"))
        //                            {
        //                                var accountColumnName = fields.First(f => f.PropertyName == "Account").ColumnName;
        //                                var accountNumberIndex = Array.IndexOf(columns, accountColumnName);

        //                                var accountNumber = values[accountNumberIndex];

        //                                var accountResponse = accountService.GetWhereExpression(a => a.AccountNumber == accountNumber);
        //                                if (accountResponse.IsSuccessful)
        //                                {
        //                                    var Account = accountResponse.Data;

        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //    }

        //    private ImportFile GetImportFile(Guid id)
        //    {
        //        var includes = new Expression<Func<ImportFile, object>>[] { f => f.File, f => f.File.Data };
        //        var importFile = _uow.GetById<ImportFile>(id, includes);

        //        return importFile;
        //    }

        //}
    }
}
