using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class SegmentDataSource
    {

        public static IList<Segment> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Segment> existing = uow.GetRepositoryOrNull<Segment>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var ledgers = LedgerDataSource.GetDataSource(uow);
            var years = FiscalYearDataSource.GetDataSource(uow);
            var segments = new List<Segment>();

            foreach(var year in years.Where(p => p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                if (year.Segments == null)
                {
                    year.Segments = new List<Segment>();
                }

                if (year.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE)
                {
                    AddSegment(segments, year, 1, "1001", "Cash");
                    AddSegment(segments, year, 1, "1501", "Accounts Receivable");
                    AddSegment(segments, year, 1, "2002", "Accounts Payable");
                    AddSegment(segments, year, 1, "3001", "Capital");
                    AddSegment(segments, year, 1, "3002", "Retained Earnings");
                    AddSegment(segments, year, 1, "4001", "Sales");
                    AddSegment(segments, year, 1, "5001", "Office Supplies");

                    AddSegment(segments, year, 2, "CORP", "Corporate Office");
                    AddSegment(segments, year, 2, "MIDW", "Midwest Region");
                }

                else
                {
                    AddSegment(segments, year, 1, "01", "Unrestricted");
                    AddSegment(segments, year, 1, "02", "Temporarily Restricted");
                    AddSegment(segments, year, 1, "03", "Permanently Restricted");

                    var segment2 = AddSegment(segments, year, 2, "100", "Cash & Cash Equivalents");
                    var segment3 = AddSegment(segments, year, 3, "10", "Cash", segment2);
                    AddSegment(segments, year, 4, "01", "Cash on Hand", segment3);
                    AddSegment(segments, year, 4, "10", "Regular", segment3);

                    segment2 = AddSegment(segments, year, 2, "150", "Sundry Receivables");
                    segment3 = AddSegment(segments, year, 3, "50", "Receivables", segment2);
                    AddSegment(segments, year, 4, "40", "Due from Unrestricted", segment3);
                    AddSegment(segments, year, 4, "41", "Due to Unrestricted", segment3);
                    AddSegment(segments, year, 4, "42", "Due from Temp Restricted", segment3);
                    AddSegment(segments, year, 4, "43", "Due to Temp Restricted", segment3);
                    AddSegment(segments, year, 4, "44", "Due from Perm Restricted", segment3);
                    AddSegment(segments, year, 4, "45", "Due to Perm Restricted", segment3);

                    AddSegment(segments, year, 4, "50", "Due from Other Entities", segment3);
                    AddSegment(segments, year, 4, "51", "Due to Other Entities", segment3);
                    AddSegment(segments, year, 4, "52", "Due from ABC Entity", segment3);
                    AddSegment(segments, year, 4, "53", "Due to ABC Entity", segment3);
                    AddSegment(segments, year, 4, "54", "Due from DEF Entity", segment3);
                    AddSegment(segments, year, 4, "55", "Due to DEF Entity", segment3);

                    AddSegment(segments, year, 3, "53", "Other Assets", segment2);
                    AddSegment(segments, year, 4, "10", "Cash Suspense", segment3);

                    segment2 = AddSegment(segments, year, 2, "200", "Investment Notes");
                    segment3 = AddSegment(segments, year, 3, "00", "Demand Notes", segment2);
                    AddSegment(segments, year, 4, "20", "Variable Demand Notes", segment3);                

                    segment2 = AddSegment(segments, year, 2, "310", "Undesignated Fund");
                    segment3 = AddSegment(segments, year, 3, "50", "General Fund", segment2);
                    AddSegment(segments, year, 4, "02", "Accumulated Revenue", segment3);

                    segment2 = AddSegment(segments, year, 2, "380", "Temp Restricted Funds");
                    segment3 = AddSegment(segments, year, 3, "50", "General Fund", segment2);
                    AddSegment(segments, year, 4, "02", "Accumulated Revenue", segment3);

                    segment2 = AddSegment(segments, year, 2, "390", "Perm Restricted Funds");
                    segment3 = AddSegment(segments, year, 3, "50", "General Fund", segment2);
                    AddSegment(segments, year, 4, "02", "Accumulated Revenue", segment3);

                    segment2 = AddSegment(segments, year, 2, "470", "General Fund");
                    segment3 = AddSegment(segments, year, 3, "80", "General Fund", segment2);
                    AddSegment(segments, year, 4, "10", "Income from Regions", segment3);

                    segment2 = AddSegment(segments, year, 2, "480", "Temp Restricted Funds");
                    segment3 = AddSegment(segments, year, 3, "80", "Endowment", segment2);
                    AddSegment(segments, year, 4, "10", "ETF Gifts", segment3);

                    segment2 = AddSegment(segments, year, 2, "500", "Interest Expense");
                    segment3 = AddSegment(segments, year, 3, "10", "interest Expense", segment2);
                    AddSegment(segments, year, 4, "01", "Interest on Notes & Balances", segment3);

                    AddSegment(segments, year, 5, "01", "Central Office");
                    AddSegment(segments, year, 5, "02", "Extension Fund");
                    AddSegment(segments, year, 5, "03", "Services");
                    AddSegment(segments, year, 5, "05", "Development");
                    AddSegment(segments, year, 5, "07", "NCM");
                }
            }

            uow.CreateRepositoryForDataSource(segments);

            return segments;
        }    

        private static Segment AddSegment(IList<Segment> list, FiscalYear year, int level, string code, string name, Segment parent = null)
        {
            Segment segment = new Segment()
            {
                Level = level,
                Code = code,
                Name = name,
                ParentSegment = parent,
                ChildSegments = new List<Segment>(),
                FiscalYear = year,
                Id = GuidHelper.NewSequentialGuid(),
                SegmentLevel = year.Ledger.SegmentLevels.FirstOrDefault(p => p.Level == level)
            };

            list.Add(segment);
            year.Segments.Add(segment);

            if (parent != null)
            {
                parent?.ChildSegments.Add(segment);
            }

            if (segment.SegmentLevel.Segments == null)
            {
                segment.SegmentLevel.Segments = new List<Segment>();
            }
            segment.SegmentLevel.Segments.Add(segment);

            return segment;
        }

    }

    
}
