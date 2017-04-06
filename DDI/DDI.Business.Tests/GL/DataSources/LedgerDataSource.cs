using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class LedgerDataSource
    {
        public static IList<Ledger> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Ledger> existing = uow.GetRepositoryOrNull<Ledger>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var businessUnits = BusinessUnitDataSource.GetDataSource(uow);
            var list = new List<Ledger>();

            foreach (var unit in businessUnits)
            {
                list.Add(new Ledger()
                {
                    AccountGroup1Title = "Category",
                    AccountGroup2Title = "Class",
                    AccountGroup3Title = "Type",
                    AccountGroup4Title = "Other",
                    AccountGroupLevels = 3,
                    ApproveJournals = true,
                    BusinessUnit = unit,
                    IsParent = (unit.BusinessUnitType == Shared.Enums.GL.BusinessUnitType.Organization),
                    Code = unit.Code,
                    Name = unit.Name,
                    CapitalizeHeaders = true,
                    CopyCOAChanges = true,
                    FundAccounting = true,
                    FixedBudgetName = "Approved Budget",
                    WorkingBudgetName = "Working Budget",
                    WhatIfBudgetName = "What If Budget",
                    Status = Shared.Enums.GL.LedgerStatus.Active,
                    NumberOfSegments = 5,
                    Id = GuidHelper.NewSequentialGuid()
                });
            }

            Ledger orgLedger = list.First(p => p.IsParent == true);
            foreach (var entry in list.Where(p => p.IsParent = false))
            {
                entry.OrgLedger = orgLedger;
            }

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
