using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class ZipBranchDataSource
    {
        public static IList<ZipBranch> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<ZipBranch> existing = uow.GetRepositoryOrNull<ZipBranch>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var zips = ZipDataSource.GetDataSource(uow);
            var zipBranches = new List<ZipBranch>();

            var zip = zips.FirstOrDefault(p => p.ZipCode == "46061"); // Noblesville
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Noblesville", "X13121", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46060"); // Noblesville
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Noblesville", "X13121", "P*", true));
                list.Add(AddZipBranch(zip, "Strawtown", "X13776", "NN", false));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46062"); // Noblesville
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Noblesville", "X13121", "P*", true));
                list.Add(AddZipBranch(zip, "Westfield", "X14031", "PY", false));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46033"); // Carmel
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Carmel", "X11782", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46032"); // Carmel
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Carmel", "X11782", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46082"); // Carmel
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Carmel", "X11782", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46037"); // Fishers
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Fishers", "X12200", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46038"); // Fishers
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Fishers", "X12200", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46085"); // Fishers
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Newgistics Merchandise Retrn", "010481", "NN", false));
                list.Add(AddZipBranch(zip, "Fishers", "X12200", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46204"); // Indianapolis
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Indianapolis", "X12558", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46107"); // Beech Grove
            if (zip != null)
            {
                var list = new List<ZipBranch>();

                list.Add(AddZipBranch(zip, "Beech Grove", "X11555", "P*", true));

                zip.ZipBranches = list;
                zipBranches.AddRange(list);
            }

            uow.CreateRepositoryForDataSource(zipBranches);            
            return zipBranches;
        }    

        private static ZipBranch AddZipBranch(Zip zip, string description, string uspsKey, string facilityCode, bool isPreferred)
        {
            return new ZipBranch()
            {
                Zip = zip,
                Description = description,
                USPSKey = uspsKey,
                FacilityCode = facilityCode,
                IsPreferred = isPreferred,
                Id = GuidHelper.NextGuid()
            };
        }


    }

    
}
