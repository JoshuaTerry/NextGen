using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.Common.DataSources
{
    public static class ZipDataSource
    {
        public static IList<Zip> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Zip> existing = uow.GetRepositoryOrNull<Zip>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var cities = CityDataSource.GetDataSource(uow);
            var zips = new List<Zip>();

            var city = cities.FirstOrDefault(p => p.PlaceCode == "1854180"); // Noblesville
            if (city != null)
            {
                var list = new List<Zip>();

                list.Add(AddZip(city, "46061", 40.0499, -86.0215));          
                list.Add(AddZip(city, "46060", 40.0646, -85.9159));
                list.Add(AddZip(city, "46062", 40.0613, -86.0560));

                city.Zips = list;
                zips.AddRange(list);
            }

            city = cities.FirstOrDefault(p => p.PlaceCode == "1810342"); // Carmel
            if (city != null)
            {
                var list = new List<Zip>();

                list.Add(AddZip(city, "46033", 39.9793, -86.0856));
                list.Add(AddZip(city, "46032", 39.9626, -86.1748));
                list.Add(AddZip(city, "46082", 39.9729, -86.1079));

                city.Zips = list;
                zips.AddRange(list);
            }

            city = cities.FirstOrDefault(p => p.PlaceCode == "1823278"); // Fishers
            if (city != null)
            {
                var list = new List<Zip>();

                list.Add(AddZip(city, "46037", 39.9606, -85.9472));
                list.Add(AddZip(city, "46038", 39.9667, -86.0172));
                list.Add(AddZip(city, "46085", 0, 0));

                city.Zips = list;
                zips.AddRange(list);
            }

            city = cities.FirstOrDefault(p => p.PlaceCode == "1836003"); // Indianapolis
            if (city != null)
            {
                var list = new List<Zip>();

                list.Add(AddZip(city, "46201", 39.7741, -86.1092));
                list.Add(AddZip(city, "46202", 39.7841, -86.1635));
                list.Add(AddZip(city, "46203", 39.7376, -86.0969));
                list.Add(AddZip(city, "46204", 39.7720, -86.1570));
                list.Add(AddZip(city, "46205", 39.8294, -86.1344));
                list.Add(AddZip(city, "46206", 39.7909, -86.1477));
                list.Add(AddZip(city, "46207", 39.7909, -86.1477));
                list.Add(AddZip(city, "46208", 39.8193, -86.1711));
                list.Add(AddZip(city, "46209", 39.7909, -86.1477));
                list.Add(AddZip(city, "46210", 0.0000, 0.0000));
                list.Add(AddZip(city, "46211", 39.7909, -86.1477));
                list.Add(AddZip(city, "46213", 0.0000, 0.0000));
                list.Add(AddZip(city, "46214", 39.7928, -86.2915));
                list.Add(AddZip(city, "46216", 39.8680, -86.0097));
                list.Add(AddZip(city, "46217", 39.6746, -86.1915));
                list.Add(AddZip(city, "46218", 39.8073, -86.0997));
                list.Add(AddZip(city, "46219", 39.7824, -86.0429));
                list.Add(AddZip(city, "46220", 39.8671, -86.1088));
                list.Add(AddZip(city, "46221", 39.6919, -86.2368));
                list.Add(AddZip(city, "46222", 39.7910, -86.2153));
                list.Add(AddZip(city, "46223", 39.7909, -86.1477));
                list.Add(AddZip(city, "46224", 39.7954, -86.2569));
                list.Add(AddZip(city, "46225", 39.7403, -86.1633));
                list.Add(AddZip(city, "46226", 39.8389, -86.0525));
                list.Add(AddZip(city, "46227", 39.6748, -86.1327));
                list.Add(AddZip(city, "46228", 39.8482, -86.2007));
                list.Add(AddZip(city, "46229", 39.7885, -85.9771));
                list.Add(AddZip(city, "46230", 39.7909, -86.1477));
                list.Add(AddZip(city, "46231", 39.7155, -86.3206));
                list.Add(AddZip(city, "46234", 39.8133, -86.3264));
                list.Add(AddZip(city, "46235", 39.8370, -85.9745));
                list.Add(AddZip(city, "46236", 39.8957, -85.9675));
                list.Add(AddZip(city, "46237", 39.6711, -86.0721));
                list.Add(AddZip(city, "46239", 39.7215, -85.9990));
                list.Add(AddZip(city, "46240", 39.9064, -86.1249));
                list.Add(AddZip(city, "46241", 39.7299, -86.2854));
                list.Add(AddZip(city, "46242", 39.7909, -86.1477));
                list.Add(AddZip(city, "46244", 39.7909, -86.1477));
                list.Add(AddZip(city, "46247", 39.7909, -86.1477));
                list.Add(AddZip(city, "46249", 39.7909, -86.1477));
                list.Add(AddZip(city, "46250", 39.9021, -86.0648));
                list.Add(AddZip(city, "46251", 39.7909, -86.1477));
                list.Add(AddZip(city, "46253", 39.7909, -86.1477));
                list.Add(AddZip(city, "46254", 39.8497, -86.2716));
                list.Add(AddZip(city, "46255", 39.7909, -86.1477));
                list.Add(AddZip(city, "46256", 39.9084, -86.0131));
                list.Add(AddZip(city, "46259", 39.6509, -85.9813));
                list.Add(AddZip(city, "46260", 39.8981, -86.1776));
                list.Add(AddZip(city, "46262", 0.0000, 0.0000));
                list.Add(AddZip(city, "46266", 39.7909, -86.1477));
                list.Add(AddZip(city, "46268", 39.8986, -86.2333));
                list.Add(AddZip(city, "46274", 39.7909, -86.1477));
                list.Add(AddZip(city, "46275", 39.7909, -86.1477));
                list.Add(AddZip(city, "46277", 39.7909, -86.1477));
                list.Add(AddZip(city, "46278", 39.8929, -86.2981));
                list.Add(AddZip(city, "46280", 39.9324, -86.1313));
                list.Add(AddZip(city, "46282", 39.7909, -86.1477));
                list.Add(AddZip(city, "46283", 39.7909, -86.1477));
                list.Add(AddZip(city, "46285", 39.7909, -86.1477));
                list.Add(AddZip(city, "46290", 39.9383, -86.1632));
                list.Add(AddZip(city, "46291", 39.7909, -86.1477));
                list.Add(AddZip(city, "46295", 39.7909, -86.1477));
                list.Add(AddZip(city, "46296", 39.7909, -86.1477));
                list.Add(AddZip(city, "46298", 39.7909, -86.1477));

                city.Zips = list;
                zips.AddRange(list);
            }

            city = cities.FirstOrDefault(p => p.PlaceCode == "1804204"); // Beech Grove
            if (city != null)
            {
                var list = new List<Zip>();

                list.Add(AddZip(city, "46107", 39.7164, -86.0915));

                city.Zips = list;
                zips.AddRange(list);
            }

            uow.CreateRepositoryForDataSource(zips);            
            return zips;
        }    

        private static Zip AddZip(City city, string zip, double coordinateNS, double coordinateEW)
        {
            return new Zip()
            {
                City = city,
                ZipCode = zip,
                CoordinateEW = (decimal)coordinateEW,
                CoordinateNS = (decimal)coordinateNS,
                Id = GuidHelper.NewSequentialGuid()
            };
        }


    }

    
}
