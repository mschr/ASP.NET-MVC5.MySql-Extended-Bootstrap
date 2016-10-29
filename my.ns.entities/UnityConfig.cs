using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace my.ns.entities
{
    using DbContexts;
    using IdentityConfig;
    using decorators;

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
        private static IUnityContainer __container = null;
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = __container ?? new UnityContainer();
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

        /// <summary>Registers the type mappings for IdentityConfig, DbContexts and (dto)decorators with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        public static void RegisterTypes(IUnityContainer container)
        {
            // This library doesnt really have an App_Start. But if called on App_Start 
            // as opposed to via GetConfiguredContainer, we go ahead and  
            // override the lazy initialized new-up via this variable
            __container = container;

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            //new PerRequestLifetimeManager())
            container
                    .RegisterType<DbContext, IdentityDb>("identityDb")
                    .RegisterType<DbContext, ApplicationDb>("appDb")
                    .RegisterType<RoleStore<IdentityRoleIntPk, int, UserRoleIntPk>, ApplicationRoleStore>("roleStore")
                    .RegisterType<UserStore<ApplicationUser, IdentityRoleIntPk, int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>, ApplicationUserStore>("userStore")
                    .RegisterType<UserManager<ApplicationUser, int>, ApplicationUserManager>("userManager")
                    .RegisterType<RoleManager<IdentityRoleIntPk, int>, ApplicationRoleManager>("roleManager");

                    // be aware that authentication manager must be resolvable within 
                    // container for signinmanager to resolve. We have losely uncoupled 
                    // this library from other interfaces such as Microsoft.Owin.Security etc, thus 
                    // this is not guarenteed to work HttpContext.Current.GetOwinContext().Authentication


            // Register decorated DTO's
            container
                    .RegisterType<decorators.IUser, DecoratedUser>()
                    .RegisterType<decorators.IUserClaim, DecoratedUserClaim>()
                    .RegisterType<decorators.IUserAndRoles, DecoratedUserAndRoles>()
                    .RegisterType<decorators.IUserAndClaims, DecoratedUserAndClaims>();
            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
