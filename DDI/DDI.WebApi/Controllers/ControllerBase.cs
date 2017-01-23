using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    public class ControllerBase : ApiController
    {
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;

        public ControllerBase()
            :this(new DynamicTransmogrifier(), new Pagination())
        {
        }

        internal ControllerBase(DynamicTransmogrifier dynamicTransmogrifier, IPagination pagination)
        {
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
        }

        public IPagination Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }

        public DynamicTransmogrifier DynamicTransmogrifier
        {
            get { return _dynamicTransmogrifier; }
            set { _dynamicTransmogrifier = value; }
        }

        protected UrlHelper GetUrlHelper()
        {
            var urlHelper = new UrlHelper(Request);
            return urlHelper;
        }
    }
}