using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;

namespace my.ns.client
{
    using entities.DbContexts;
    using entities.IdentityConfig;
    using Microsoft.AspNet.Identity.Owin;
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        /// <summary>
        /// Aliased GetConfiguredContainer
        /// </summary>
        /// <returns>a configured IUnityContainer</returns>
        public static IUnityContainer DI { get { return GetConfiguredContainer(); } set { } }

        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // wire up the entities namespace
            entities.UnityConfig.RegisterTypes(container);
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c =>
                HttpContext.Current.GetOwinContext().Authentication))
                .RegisterType<SignInManager<ApplicationUser, int>, ApplicationSignInManager>("signInManager");

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
