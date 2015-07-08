using IoCMvcApplication.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IoCMvcApplication
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                // for the sake of simplicity and proving the concept, this is hard coded to IController
                return IoCHandler.Container.Resolve<IController>();
            }
            catch
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}