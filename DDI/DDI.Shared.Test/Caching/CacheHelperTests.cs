using System;
using System.Threading;
using DDI.Shared.Caching;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.ModuleInfo;
using DDI.Shared.ModuleInfo.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Caching
{    
    // Note:  This test class also tests the MemoryCacheProvider.
    
    [TestClass]
    public class CacheHelperTests
    {
        private const string TESTDESCR = "Shared | Caching";
        private string _key;
        private string _item;
        private string _callbackItem;

        [TestMethod, TestCategory(TESTDESCR)]
        public void CacheHelper_GetEntry()
        {
            _key = "A";
            _item = "1";
            Assert.AreEqual("1", CacheHelper.GetEntry(_key, GetItemToCache), "GetEntry retrieved the provided value.");
            _item = "2";
            Assert.AreEqual("1", CacheHelper.GetEntry(_key, GetItemToCache), "GetEntry retrieved the cached value.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void CacheHelper_SetEntry()
        {
            _key = "B";
            _item = "1";
            CacheHelper.SetEntry(_key, "3");
            Assert.AreEqual("3", CacheHelper.GetEntry(_key, GetItemToCache), "GetEntry retrieved the cached value that was set.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void CacheHelper_ClearEntry()
        {
            _key = "C";
            _item = "1";
            CacheHelper.SetEntry(_key, "3");
            Assert.AreEqual("3", CacheHelper.GetEntry(_key, GetItemToCache), "GetEntry retrieved the cached value that was set.");
            CacheHelper.RemoveEntry(_key);
            Assert.AreEqual("1", CacheHelper.GetEntry(_key, GetItemToCache), "GetEntry retrieved the provided value after cache value cleared.");
        }

        [TestMethod, TestCategory("Long Running")]
        public void CacheHelper_AbsoluteTimeout()
        {
            _key = "D";
            _item = "1";
            _callbackItem = string.Empty; 

            Assert.AreEqual("1", CacheHelper.GetEntry(_key, 3, false, GetItemToCache, CallbackMethod), 
                "GetEntry retrieved the provided value.");

            _item = "4";
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Assert.AreEqual("4", CacheHelper.GetEntry(_key, GetItemToCache), "Cached item expired.");

            Assert.AreEqual("1", _callbackItem, "Callback method executed.");
        }

        [TestMethod, TestCategory("Long Running")]
        public void CacheHelper_SlidingTimeout()
        {
            _key = "E";
            _item = "1";
            _callbackItem = string.Empty;
            Assert.AreEqual("1", CacheHelper.GetEntry(_key, 3, true, GetItemToCache, CallbackMethod),
                "GetEntry retrieved the provided value.");

            _item = "4";
            for (int count = 0; count < 10; count++) // Retrieve item 10 times during the next 5 seconds
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Assert.AreEqual("1", CacheHelper.GetEntry(_key, GetItemToCache), $"Retrieval #{count + 1}.");
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(5));  // Waiting 5 seconds should expire the cached item.
            Assert.AreEqual("4", CacheHelper.GetEntry(_key, GetItemToCache), "Cached item expired.");

            Assert.AreEqual("1", _callbackItem, "Callback method executed.");
        }

        private string GetItemToCache()
        {
            return _item;
        }

        private void CallbackMethod(string entry)
        {
             _callbackItem = entry;
        }
    }
}
