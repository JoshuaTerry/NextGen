﻿using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using DDI.WebApi.Controllers;
using DDI.WebApi.Tests.Fakes;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class AuthorizationsControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";

        //[TestMethod,TestCategory(TESTDESCR)]
        //public async Task AuthorizationsController_GetManageInfo()
        //{
        //    var userId = "jtrick@myhouse.com";
        //    var claim = new Claim("test", userId);
        //    var principal = new Mock<IPrincipal>();
        //    var mockIdentity = new Mock<ClaimsIdentity>();
        //    mockIdentity.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(claim);
        //    principal.SetupGet(p => p.Identity).Returns(mockIdentity.Object);
        //    var accountManager = new FakeUserManager();
        //    var target = new AuthorizationsController(accountManager, null);
        //    target.User = principal.Object;
        //    var result = await target.GetManageInfo("", false);
        //    Assert.AreEqual(result.Email, userId);
        //}
    }
}
