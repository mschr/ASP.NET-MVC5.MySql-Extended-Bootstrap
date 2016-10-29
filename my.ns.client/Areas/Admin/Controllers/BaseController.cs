using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace my.ns.client.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static IEnumerable<MethodInfo> renderedList = null;
        private void renderList()
        {
            // see http://stackoverflow.com/questions/21583278/getting-all-controllers-and-actions-names-in-c-sharp
            Assembly asm = Assembly.GetExecutingAssembly();

            Func<Type, bool> inheritedController = type => typeof(Controller).IsAssignableFrom(type) && type.FullName.Contains(asm.GetName().Name);
            Func<MethodInfo, bool> nonPostActionAttribute = method =>
                !method.IsDefined(typeof(NonActionAttribute), false) // Actions only (all are public are, unless [NonAction] applies
                && !method.IsDefined(typeof(HttpPostAttribute), false);
            Func<MethodInfo, bool> definedByProject = method => method.DeclaringType.FullName.Contains(asm.GetName().Name) && !method.IsSpecialName;

            IEnumerable<MethodInfo> methods = asm.GetTypes()
                .Where(inheritedController)
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public ^ BindingFlags.GetProperty ^ BindingFlags.SetProperty))
                    .Where(nonPostActionAttribute)
                    .Where(definedByProject);
            log.Debug("Rendered public methods list for sidebar usage: " + string.Join(";", methods.Select(m => m.Name)));
            BaseController.renderedList = methods;
        }
        public BaseController()
        {
            if (BaseController.renderedList == null) renderList();
            ViewBag.ActionMethods = BaseController.renderedList;
            log.Info("Info log");
            log.Trace("Trace log");
            System.Diagnostics.Trace.Write("Trace diagnostic");
        }

    }
}