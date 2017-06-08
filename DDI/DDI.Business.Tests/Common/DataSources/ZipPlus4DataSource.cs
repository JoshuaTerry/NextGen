using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Common;


namespace DDI.Business.Tests.Common.DataSources
{
    public static class ZipPlus4DataSource
    {
        public static IList<ZipPlus4> GetDataSource(IUnitOfWork uow)
        {
            IList<ZipPlus4> existing = uow.GetRepositoryDataSource<ZipPlus4>();
            if (existing != null)
            {
                return existing;
            }

            var zips = ZipDataSource.GetDataSource(uow);
            var zipStreets = ZipStreetDataSource.GetDataSource(uow);
            var zipPlus4s = new List<ZipPlus4>();

            // There is no way to include all Zip+4's for Noblesville & 46204, so only selected streets are included:
            // Noblesville 46061: PO Box
            // Noblesville 46060: S 8th St.
            // Noblesville 46060: Ten Point Dr.
            // Indianapolis 46024: W Ohio St.

            var zip = zips.FirstOrDefault(p => p.ZipCode == "46061");  // Noblesville 46061 PO BOX
            var street = zip.ZipStreets.FirstOrDefault(p => p.Street == "PO BOX");

            if (street != null)
            {
                var list = new List<ZipPlus4>();

                list.Add(AddZipPlus4(street, "X115967831", "1981", "2084", "", "", "", "1981", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967833", "2161", "2180", "", "", "", "2161", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967840", "3001", "3052", "", "", "", "3001", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967841", "3061", "3172", "", "", "", "3061", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530029", "361", "366", "", "", "", "0361", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530042", "591", "596", "", "", "", "0591", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967825", "1261", "1372", "", "", "", "1261", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967823", "1021", "1140", "", "", "", "1021", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530026", "321", "328", "", "", "", "0321", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530030", "371", "388", "", "", "", "0371", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530033", "421", "436", "", "", "", "0421", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530028", "351", "358", "", "", "", "0351", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530012", "111", "118", "", "", "", "0111", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X121806291", "2401", "2512", "", "", "", "2401", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X121655272", "2281", "2398", "", "", "", "2281", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967830", "1861", "1974", "", "", "", "1861", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967824", "1141", "1252", "", "", "", "1141", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530017", "181", "196", "", "", "", "0181", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123017530", "4000", "4000", "", "", "", "4000", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122205337", "2761", "2892", "", "", "", "2761", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530019", "211", "226", "", "", "", "0211", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530032", "411", "418", "", "", "", "0411", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530013", "121", "136", "", "", "", "0121", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967827", "1501", "1612", "", "", "", "1501", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530027", "331", "346", "", "", "", "0331", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X121633990", "2641", "2758", "", "", "", "2641", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967829", "1741", "1854", "", "", "", "1741", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530016", "171", "178", "", "", "", "0171", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967826", "1381", "1492", "", "", "", "1381", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967819", "601", "658", "", "", "", "0601", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530014", "141", "148", "", "", "", "0141", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530005", "1", "16", "", "", "", "0001", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530031", "391", "406", "", "", "", "0391", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530034", "441", "448", "", "", "", "0441", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530021", "241", "256", "", "", "", "0241", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967828", "1621", "1732", "", "", "", "1621", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530010", "81", "88", "", "", "", "0081", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530006", "21", "28", "", "", "", "0021", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530020", "231", "238", "", "", "", "0231", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123633338", "451", "478", "", "", "", "0451", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530023", "271", "286", "", "", "", "0271", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X121655275", "3181", "3282", "", "", "", "3181", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530025", "301", "316", "", "", "", "0301", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530018", "201", "208", "", "", "", "0201", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123633339", "481", "580", "", "", "", "0481", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530007", "31", "46", "", "", "", "0031", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530008", "51", "58", "", "", "", "0051", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967822", "901", "1020", "", "", "", "0901", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X121633989", "2521", "2638", "", "", "", "2521", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530015", "151", "166", "", "", "", "0151", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530022", "261", "268", "", "", "", "0261", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967832", "2101", "2152", "", "", "", "2101", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530011", "91", "106", "", "", "", "0091", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967820", "661", "760", "", "", "", "0661", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X115967821", "781", "900", "", "", "", "0781", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530024", "291", "298", "", "", "", "0291", EvenOddType.Any, EvenOddType.Any, true));
                list.Add(AddZipPlus4(street, "X123530009", "61", "76", "", "", "", "0061", EvenOddType.Any, EvenOddType.Any, true));
                
                street.ZipPlus4s = list;
                zipPlus4s.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46060"); // Noblesville S 8th St
            street = zip.ZipStreets.FirstOrDefault(p => p.Street == "8TH" && p.Prefix == "S");

            if (street != null)
            {
                var list = new List<ZipPlus4>();

                list.Add(AddZipPlus4(street, "X123874679", "347", "347", "STE", "A", "B", "2700", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914981", "501", "599", "", "", "", "27ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122866711", "175", "175", "TRLR", "1", "6", "2609", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867397", "1000", "1098", "", "", "", "3419", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123874678", "347", "347", "APT", "1", "4", "2700", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867393", "300", "398", "", "", "", "2713", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122939040", "1101", "1199", "", "", "", "37ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123502842", "304", "304", "APT", "E", "E", "2710", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914985", "901", "999", "", "", "", "34ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914980", "401", "499", "", "", "", "27ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867406", "201", "299", "", "", "", "2712", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914977", "600", "698", "", "", "", "34ND", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867407", "301", "399", "", "", "", "2714", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914974", "200", "298", "", "", "", "2711", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867396", "900", "998", "", "", "", "3417", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914982", "601", "699", "", "", "", "34ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867404", "998 1/2", "998 1/2", "", "", "", "3417", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914986", "1001", "1099", "", "", "", "34ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867409", "1701", "1799", "", "", "", "3742", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867402", "1500", "1598", "", "", "", "3737", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122866710", "908", "908", "APT", "1", "3", "3406", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123502562", "26", "26", "", "", "", "2680", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123503483", "2", "98", "", "", "", "2604", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867394", "700", "798", "", "", "", "3413", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914976", "500", "598", "", "", "", "27ND", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867405", "101", "199", "", "", "", "2608", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122866709", "908", "908", "", "", "", "3404", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867398", "1100", "1198", "", "", "", "3715", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867403", "1600", "1698", "", "", "", "3739", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123503484", "1", "99", "", "", "", "2605", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914973", "100", "198", "", "", "", "26ND", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123502841", "304", "304", "APT", "1", "4", "2710", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122939041", "1201", "1299", "", "", "", "37ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867401", "1400", "1498", "", "", "", "3735", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123529879", "304", "304", "", "", "", "2623", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867408", "1601", "1699", "", "", "", "3705", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914975", "400", "498", "", "", "", "27ND", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123502843", "304", "304", "APT", "F", "F", "2710", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914990", "1401", "1499", "", "", "", "37ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914991", "1501", "1599", "", "", "", "37ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867395", "800", "898", "", "", "", "3415", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867410", "1801", "1899", "", "", "", "3708", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122939042", "1301", "1399", "", "", "", "3702", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914979", "1800", "1898", "", "", "", "3706", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122866712", "175", "175", "", "", "", "2626", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914984", "801", "899", "", "", "", "34ND", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867400", "1300", "1398", "", "", "", "3733", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122867399", "1200", "1298", "", "", "", "3717", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914978", "1700", "1798", "", "", "", "3741", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122914983", "701", "799", "", "", "", "34ND", EvenOddType.Odd, EvenOddType.Any, false));

                street.ZipPlus4s = list;
                zipPlus4s.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46060"); // Noblesville Ten Point Drive
            street = zip.ZipStreets.FirstOrDefault(p => p.Street == "TEN PT");

            if (street != null)
            {
                var list = new List<ZipPlus4>();

                list.Add(AddZipPlus4(street, "X115364732", "15401", "15499", "", "", "", "8027", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115364729", "15368", "15398", "", "", "", "7994", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115364727", "15200", "15299", "", "", "", "7992", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115364730", "15400", "15498", "", "", "", "8028", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115364731", "15301", "15399", "", "", "", "7991", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115364728", "15300", "15366", "", "", "", "7993", EvenOddType.Even, EvenOddType.Any, false));

                street.ZipPlus4s = list;
                zipPlus4s.AddRange(list);
            }

            zip = zips.FirstOrDefault(p => p.ZipCode == "46204"); // Indianapolis W Ohio St
            street = zip.ZipStreets.FirstOrDefault(p => p.Street == "OHIO" && p.Prefix == "W");

            if (street != null)
            {
                var list = new List<ZipPlus4>();

                list.Add(AddZipPlus4(street, "X127087856", "101", "101", "STE", "2150", "2150", "4205", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X123467181", "151", "151", "", "", "", "1960", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X121623369", "101", "101", "STE", "101", "101", "2853", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X123358665", "101", "101", "STE", "1", "1", "1906", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X118479110", "101", "101", "STE", "100", "100", "1918", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121582426", "101", "101", "STE", "400", "400", "1970", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120400064", "101", "101", "STE", "1900", "1900", "4239", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120923532", "101", "101", "STE", "1590", "1590", "4201", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X122483057", "101", "101", "STE", "1575", "1580", "2051", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X127087855", "101", "101", "STE", "250", "250", "2853", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453349", "101", "101", "STE", "820", "820", "2829", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454862", "101", "101", "STE", "300", "300", "4206", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X123007440", "101", "101", "STE", "500", "500", "2830", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118724856", "2", "98", "", "", "", "19ND", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454827", "101", "101", "STE", "820", "820", "1955", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118405079", "101", "101", "STE", "750", "750", "1995", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453345", "101", "101", "STE", "1000", "1000", "1990", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454837", "101", "101", "STE", "1000", "1000", "1982", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120886087", "101", "101", "STE", "770", "780", "1995", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118405078", "101", "101", "STE", "760", "760", "1973", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454840", "101", "101", "STE", "660", "660", "1985", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X122471102", "101", "101", "STE", "1792", "1792", "4200", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X126866655", "101", "101", "STE", "1650", "1707", "4201", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454849", "101", "101", "STE", "1401", "1401", "1996", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X124018360", "101", "101", "STE", "1310", "1310", "1995", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454841", "101", "101", "STE", "1400", "1400", "1988", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453346", "101", "101", "STE", "550", "550", "2825", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118962269", "101", "101", "STE", "800", "800", "4203", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453347", "101", "101", "STE", "660", "660", "2827", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118790845", "101", "101", "STE", "1450", "1450", "1998", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X126758905", "101", "101", "STE", "1180", "1180", "4201", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121217139", "101", "101", "STE", "1111", "1111", "1995", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X115454838", "101", "101", "STE", "550", "550", "1984", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120763029", "101", "151", "", "", "", "1905", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X122593051", "101", "101", "STE", "9", "9", "1980", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X115454831", "101", "101", "STE", "610", "610", "1972", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121582427", "101", "101", "STE", "444", "444", "1970", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X119217064", "101", "101", "FL", "21ST", "21ST", "4205", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123467182", "151", "151", "STE", "X", "X", "1964", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X120690985", "101", "101", "STE", "2100", "2121", "4203", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123467180", "151", "151", "STE", "X", "X", "2564", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X120690984", "101", "101", "STE", "1776", "1776", "4201", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X122471101", "101", "101", "STE", "1175", "1175", "1982", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X115454848", "101", "101", "STE", "1350", "1350", "1996", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118768107", "101", "101", "FL", "NINTH", "NINTH", "1980", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X118768104", "101", "101", "FL", "NINTH", "NINTH", "4213", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454842", "101", "101", "STE", "1100", "1100", "1989", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121121573", "101", "101", "STE", "1150", "1150", "1918", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X124018358", "101", "101", "STE", "1300", "1300", "1994", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454858", "101", "101", "STE", "1800", "1800", "4202", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454846", "101", "101", "STE", "1600", "1600", "1994", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X119117263", "101", "101", "STE", "700", "700", "1973", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121582428", "101", "101", "FL", "2ND", "2ND", "4223", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X123467183", "151", "151", "STE", "120", "120", "1964", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454860", "101", "101", "STE", "2000", "2000", "4204", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118014420", "101", "101", "STE", "200", "205", "1951", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454825", "101", "101", "STE", "1750", "1750", "1933", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X121011473", "101", "101", "STE", "650", "650", "1954", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453344", "101", "101", "STE", "1800", "1800", "1987", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454868", "101", "101", "STE", "500", "500", "4254", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X126838377", "31", "31", "", "", "", "1981", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X118962267", "101", "101", "STE", "850", "850", "1974", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118542910", "101", "101", "STE", "1200", "1200", "4239", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120520168", "101", "101", "STE", "450", "450", "0058", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X126649186", "101", "101", "STE", "675", "675", "2835", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X118898312", "101", "101", "STE", "1540", "1540", "1998", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X119117264", "101", "101", "STE", "720", "730", "1973", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X120520170", "101", "101", "STE", "470", "470", "1970", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118898310", "101", "101", "STE", "1500", "1515", "1908", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X118542909", "101", "101", "STE", "1250", "1250", "2051", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X122658649", "100", "198", "", "", "", "2153", EvenOddType.Even, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X121011474", "101", "101", "STE", "875", "875", "4207", EvenOddType.Odd, EvenOddType.Odd, false));
                list.Add(AddZipPlus4(street, "X118699714", "1", "99", "", "", "", "1916", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454822", "101", "101", "FL", "20TH", "20TH", "4204", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X120400062", "101", "101", "STE", "670", "670", "1954", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115453351", "101", "101", "STE", "1350", "1350", "2831", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X115454823", "101", "101", "", "", "", "1906", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X118790842", "101", "101", "FL", "9TH", "9TH", "1980", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X120400063", "101", "101", "STE", "600", "600", "1989", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X119217063", "101", "101", "FL", "21", "21", "4205", EvenOddType.Any, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X115454863", "101", "101", "STE", "2200", "2200", "4207", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X122624590", "31", "31", "", "", "", "1981", EvenOddType.Odd, EvenOddType.Any, false));
                list.Add(AddZipPlus4(street, "X123794274", "101", "101", "STE", "414", "414", "4223", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X119117262", "101", "101", "STE", "710", "710", "1923", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X124018359", "101", "101", "STE", "1320", "1330", "1994", EvenOddType.Odd, EvenOddType.Even, false));
                list.Add(AddZipPlus4(street, "X118898311", "101", "101", "STE", "1010", "1010", "1908", EvenOddType.Odd, EvenOddType.Even, false));

                street.ZipPlus4s = list;
                zipPlus4s.AddRange(list);
            }

            uow.CreateRepositoryForDataSource(zipPlus4s);            
            return zipPlus4s;
        }

        private static ZipPlus4 AddZipPlus4(ZipStreet street, string updateKey, string addressLow, string addressHigh, string secondaryAbbreviation, string secondaryLow, string secondaryHigh,
            string plus4, EvenOddType addressType, EvenOddType secondaryType, bool isRange)
        {
            return new ZipPlus4()
            {
                ZipStreet = street,
                UpdateKey = updateKey,
                AddressLow = addressLow,
                AddressHigh = addressHigh,
                SecondaryAbbreviation = secondaryAbbreviation,
                SecondaryLow = secondaryLow,
                SecondaryHigh = secondaryHigh,
                Plus4 = plus4,
                AddressType = addressType,
                SecondaryType = secondaryType,
                IsRange = isRange,
                Id = GuidHelper.NewSequentialGuid()
            };
        }


    }

    
}
