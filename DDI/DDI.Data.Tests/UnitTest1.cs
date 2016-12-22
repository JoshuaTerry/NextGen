using System;

using DDI.Data;
using DDI.Data.Models.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Data.Tests
{
    [TestClass]
    public class UnitTest1
    {
        #region Methods

        [TestMethod]
        public void TestMethod1()
        {
            using (var context = new DomainContext())
            {
                Guid id = Guid.Parse("29F025C3-0CC3-E611-80E3-005056B7555A");
                var repository = new Repository<Constituent>(context);
                var constituent =repository.GetById(
                    id, 
                    c => c.ConstituentAddresses, 
                    c => c.Gender);

                Console.WriteLine(constituent);
            }
        }

        #endregion Methods
    }
}