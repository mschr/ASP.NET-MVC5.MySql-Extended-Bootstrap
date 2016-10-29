using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace my.ns.client.Areas.Admin.Controllers
{
    using entities.dto.identity;
    using entities.decorators;

    using entities.IdentityConfig;


    [Authorize(Roles = "Administrator")]
    public class UsersController : BaseController
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region public ApplicationRoleManager SignInManager
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager/* ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>()*/;
            }
            private set
            {
                _signInManager = value;
            }
        }
        #endregion

        #region public ApplicationUserManager UserManager

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager /*??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()*/;
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        #region public ApplicationRoleManager RoleManager
        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager/* ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>()*/;
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        [InjectionConstructor]
        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
            SignInManager = signInManager;
            System.Diagnostics.Trace.WriteLine("constructed");
        }

        // Controllers

        // GET: /Admin/Users/
        public ActionResult Index(string searchStringUserNameOrEmail, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringUserNameOrEmail;

                List<User> col_UserDTO = new List<User>();
                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result)
                {
                    col_UserDTO.Add(new User
                    {
                        UserName = item.UserName,
                        Email = item.Email,
                        LockoutEndDateUtc = item.LockoutEndDateUtc
                    });
                }

                // Set the number of pages
                var _UserDTOAsIPagedList =
                    new StaticPagedList<User>(col_UserDTO, intPage, intPageSize, intTotalPageCount);

                return View(_UserDTOAsIPagedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<User> col_UserDTO = new List<User>();
                return View(col_UserDTO.ToPagedList(1, 25));
            }
        }

        // GET: /Admin/Users/List
        public ActionResult List()
        {
            return RedirectToAction("Index");
        }


        #region Users *****************************

        // GET: /Admin/Users/Create 
        public ActionResult Create()
        {
            User objExpandedUserDTO = new User();

            ViewBag.Roles = GetAllRolesAsSelectList();

            return View(objExpandedUserDTO);
        }

        // PUT: /Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var Email = paramExpandedUserDTO.Email.Trim();
                var UserName = paramExpandedUserDTO.Email.Trim();
                var Password = paramExpandedUserDTO.Password.Trim();

                if (Email == "")
                {
                    throw new Exception("No Email");
                }

                if (Password == "")
                {
                    throw new Exception("No Password");
                }

                // UserName is LowerCase of the Email
                UserName = Email.ToLower();

                // Create user

                var objNewAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);

                if (AdminUserCreateResult.Succeeded == true)
                {
                    log.Info("Created user [" + UserName + "]");
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);

                    if (strNewRole != "0")
                    {
                        // Put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    ModelState.AddModelError(string.Empty,
                        "Error: Failed to create the user. Check password requirements.");
                    return View(paramExpandedUserDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                log.Debug("Error while creating user " + paramExpandedUserDTO.UserName, ex);
                return View("Create");
            }
        }

        // GET: /Admin/Users/EditUser?UserName=?
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User objExpandedUserDTO = GetUser(UserName);
            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }
            return View(objExpandedUserDTO);
        }

        // PUT: /Admin/Users/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User paramExpandedUserDTO)
        {
            try
            {
                if (paramExpandedUserDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User objExpandedUserDTO = UpdateDTOUser(paramExpandedUserDTO);

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                log.Debug("Error while editing uer " + paramExpandedUserDTO.UserName, ex);
                ModelState.AddModelError(string.Empty, "Error: " + ex);
            }
            return View("EditUser", paramExpandedUserDTO);

        }

        // DELETE: /Admin/DeleteUser
        public ActionResult DeleteUser(string UserName)
        {
            try
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                User objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(objExpandedUserDTO);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        #region User Roles *************************
        // GET: /Admin/EditRoles/TestUser 
        public ActionResult EditRoles(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserName = UserName.ToLower();

            // Check that we have an actual user
            User objExpandedUserDTO = GetUser(UserName);

            if (objExpandedUserDTO == null)
            {
                return HttpNotFound();
            }

            UserAndRoles objUserAndRolesDTO =
                GetUserAndRoles(UserName);

            return View(objUserAndRolesDTO);
        }

        // PUT: /Admin/EditRoles/TestUser 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(UserAndRoles paramUserAndRolesDTO)
        {
            try
            {
                if (paramUserAndRolesDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string UserName = paramUserAndRolesDTO.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    ApplicationUser user = UserManager.FindByName(UserName);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                    log.Info(string.Format("Added user {0} to role {1}", user.UserName, strNewRole));
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRoles objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View(objUserAndRolesDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                log.Debug("Error while editing role", ex);
                return View("EditRoles");
            }
        }

        // DELETE: /Admin/DeleteRole?UserName="TestUser&RoleName=Administrator
        public ActionResult DeleteRole(string UserName, string RoleName)
        {
            try
            {
                if ((UserName == null) || (RoleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserName = UserName.ToLower();

                // Check that we have an actual user
                User objExpandedUserDTO = GetUser(UserName);

                if (objExpandedUserDTO == null)
                {
                    return HttpNotFound();
                }

                if (UserName.ToLower() ==
                    this.User.Identity.Name.ToLower() && RoleName == "Administrator")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete Administrator Role for the current user");
                }

                // Go get the User
                ApplicationUser user = UserManager.FindByName(UserName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);
                log.Info(string.Format("Deleted role {0} for user {1}", RoleName, user.UserName));

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                log.Debug("Error while deleting role", ex);
                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRoles objUserAndRolesDTO =
                    GetUserAndRoles(UserName);

                return View("EditRoles", objUserAndRolesDTO);
            }
        }
        #endregion

        #region Claims ****************************

        // GET: /Admin/Users/ViewAllClaims
        /*
    public ActionResult ViewAllClaims()
    {
        List<UserClaims> list = new List<UserClaims>();
        foreach (var claims
            in UserManager.Users
                .Select(a => new
                {
                    type = a.Claims
                }).Select(b => b.type.Distinct()))
        {
            foreach (var claim in claims)
            {
                UserClaims insert = new UserClaims { ClaimType = claim.ClaimType };
                if (!list.Contains(insert)) list.Add(insert);
            }
        }
        return View(list);
    }
    */


        // GET: /Admin/EditClaims/TestUser 
        public ActionResult EditClaims(string UserName)
        {
            if (UserName == null)
            {
                return HttpNotFound();
            }
            DecoratedUserAndClaims unrDecorator = (DecoratedUserAndClaims)UnityConfig.DI.Resolve(typeof(IUserAndClaims));
            unrDecorator.UserNameOrId = UserName;
            var filter = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
            IEnumerable<System.Reflection.FieldInfo> fields = typeof(System.Security.Claims.ClaimTypes).GetFields(filter).Where(f => f.IsLiteral);
            ViewBag.fields = fields;

            UserAndClaims unr = unrDecorator.Decorate();
            return View(unr);
        }

        // PUT: /Admin/EditClaims/TestUser 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditClaims(UserClaim userClaim)
        {
            var _user = UserManager.FindByName(userClaim.UserName);
            if (_user == null)
            {
                return HttpNotFound("Invalid user");
            }
            if (ModelState.IsValid)
            {
                // trigger generating identity - usermanager will persist generic types to database
                _user.Claims.Remove(_user.Claims.Where(
                    c => c.ClaimType == userClaim.ClaimType).First());
                _user.Claims.Add(new UserClaimIntPk
                {
                    UserId = _user.Id,
                    ClaimType = userClaim.ClaimType,
                    ClaimValue = userClaim.ClaimValue
                });
                System.Security.Claims.ClaimsIdentity identity = await _user.GenerateUserIdentityAsync(UserManager);
                log.Info("Edited claim for user (" + userClaim.ClaimType + ":" + userClaim.ClaimValue + " for " + userClaim.UserName);
            }
            return RedirectToAction("EditClaims", new { UserName = userClaim.UserName });
        }

        // GET: /Admin/Users/AddClaim
        public ActionResult AddClaim(string UserName)
        {
            var filter = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
            var collection = typeof(System.Security.Claims.ClaimTypes).GetFields(filter)
                .Where(f => f.IsLiteral);
            var AppUser = (UserName != null) ? UserManager.FindByName(UserName) : null;
            ICollection<UserClaim> selList = new List<UserClaim>();
            var i18n = Resources.Views.Admin.Users.Resources.ResourceManager;
            foreach (var field in collection)
            {
                var value = (string)field.GetValue(null);
                if (ApplicationUser.acceptedClaims.Contains(value))
                {
                    if (AppUser != null && AppUser.Claims.Any(c => c.ClaimType == field.GetValue(null).ToString()))
                    { continue; }
                    selList.Add(new UserClaim { ClaimType = i18n.GetString("ClaimType_" + field.Name), ClaimValue = value });
                }
            }
            ViewBag.ClaimTypeList = selList.Select(c => new SelectListItem { Text = c.ClaimType, Value = c.ClaimValue }).ToList();
            var res = new UserClaim { UserName = UserName };
            return View(res);
        }

        // PUT: /Admin/Users/AddClaim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddClaim(UserClaim userClaim)
        {
            var _user = UserManager.FindByName(userClaim.UserName);
            if (_user == null)
            {
                ModelState.AddModelError("UserName", "Username invalid");
            }
            if (ModelState.IsValid)
            {
                // trigger generating identity - usermanager will persist generic types to database
                _user.Claims.Add(new UserClaimIntPk
                {
                    UserId = _user.Id,
                    ClaimType = userClaim.ClaimType,
                    ClaimValue = userClaim.ClaimValue
                });
                System.Security.Claims.ClaimsIdentity identity = await _user.GenerateUserIdentityAsync(UserManager);
                log.Info("Added claim to user (" + userClaim.ClaimType + ":" + userClaim.ClaimValue + " for " + userClaim.UserName);

            }
            return RedirectToAction("EditClaims", new { UserName = userClaim.UserName });
        }
        #endregion

        #region General Roles *****************************

        // GET: /Admin/Users/ViewAllRoles
        public ActionResult ViewAllRoles()
        {
            //var roleManager =
            //    new RoleManager<IdentityRole>
            //    (
            //        new RoleStore<IdentityRole>(new IdentityDb())
            //        );

            List<Role> colRoleDTO = (from objRole in RoleManager.Roles
                                     select new Role
                                     {
                                         Id = objRole.Id,
                                         RoleName = objRole.Name
                                     }).ToList();

            return View(colRoleDTO);
        }

        // GET: /Admin/Users/AddRole
        public ActionResult AddRole()
        {
            Role objRoleDTO = new Role();

            return View(objRoleDTO);
        }

        // PUT: /Admin/Users/AddRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(Role paramRoleDTO)
        {
            try
            {
                if (paramRoleDTO == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var RoleName = paramRoleDTO.RoleName.Trim();

                if (RoleName == "")
                {
                    throw new Exception("No RoleName");
                }

                // Create Role

                if (!RoleManager.RoleExists(RoleName))
                {
                    RoleManager.Create(new IdentityRoleIntPk(RoleName));
                    log.Info("Created role " + RoleName);

                }

                return RedirectToAction("ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }

        // DELETE: /Admin/Users/DeleteUserRole?RoleName=TestRole
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUserRole(string RoleName)
        {
            try
            {
                if (RoleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "administrator")
                {
                    throw new Exception(String.Format("Cannot delete {0} Role.", RoleName));
                }

                var UsersInRole = RoleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0)
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role because it still has users.",
                            RoleName)
                            );
                }

                var objRoleToDelete = (from objRole in RoleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                {
                    RoleManager.Delete(objRoleToDelete);
                    log.Info("Deleted role " + RoleName);
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            "Canot delete {0} Role does not exist.",
                            RoleName)
                            );
                }

                List<Role> colRoleDTO = (from objRole in RoleManager.Roles
                                         select new Role
                                         {
                                             Id = objRole.Id,
                                             RoleName = objRole.Name
                                         }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                log.Debug("Error occurred, deleting role " + RoleName, ex);
                List<Role> colRoleDTO = (from objRole in RoleManager.Roles
                                         select new Role
                                         {
                                             Id = objRole.Id,
                                             RoleName = objRole.Name
                                         }).ToList();

                return View("ViewAllRoles", colRoleDTO);
            }
        }
        #endregion

        // Utility

        #region private List<SelectListItem> GetAllRolesAsSelectList()
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems = RoleManager.Roles
                .OrderBy(x => x.Name)
                .Select(r => new SelectListItem
                {
                    Text = r.Name.ToString(),
                    Value = r.Name.ToString()
                })
                .ToList();

            SelectRoleListItems.Insert(0,
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });
            return SelectRoleListItems;
        }
        #endregion

        #region private User GetUser(string paramUserName)
        private User GetUser(string paramUserName)
        {
            DecoratedUser unr = (DecoratedUser)UnityConfig.DI.Resolve(typeof(IUser));
            unr.UserNameOrId = paramUserName;
            return unr.Decorate();
            /*User objExpandedUserDTO = new User();

            var result = UserManager.FindByName(paramUserName);

            // If we could not find the user, throw an exception
            if (result == null) throw new Exception("Could not find the User");

            objExpandedUserDTO.UserName = result.UserName;
            objExpandedUserDTO.Email = result.Email;
            objExpandedUserDTO.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExpandedUserDTO.AccessFailedCount = result.AccessFailedCount;
            objExpandedUserDTO.PhoneNumber = result.PhoneNumber;

            return objExpandedUserDTO;
            */
        }
        #endregion

        #region private User UpdateDTOUser(User objExpandedUserDTO)
        private User UpdateDTOUser(User paramExpandedUserDTO)
        {
            ApplicationUser result =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (result == null)
            {
                ModelState.AddModelError("", "Could not find the User");
                return paramExpandedUserDTO;
            }

            result.Email = paramExpandedUserDTO.Email;

            // Lets check if the account needs to be unlocked
            if (UserManager.IsLockedOut(result.Id))
            {
                // Unlock user attempts and set lockoutdate back into the past
                UserManager.ResetAccessFailedCount(result.Id);
                UserManager.SetLockoutEndDate(result.Id, DateTime.Now);
            }
            log.Info("Updating user " + paramExpandedUserDTO.UserName);
            UserManager.Update(result);
            // Was a password sent across?
            if (!string.IsNullOrEmpty(paramExpandedUserDTO.Password))
            {
                // Remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    // Add new password
                    var AddPassword =
                        UserManager.AddPassword(
                            result.Id,
                            paramExpandedUserDTO.Password
                            );
                    log.Info(string.Format("Updated password {0}, errors: {1}", paramExpandedUserDTO.UserName, string.Join(", ", AddPassword.Errors)));

                    foreach (var error in AddPassword.Errors)
                    {
                        ModelState.AddModelError("Password", PasswordValidatorCodes.GetLocalizedMessageForCode(error));
                    }
                }
            }

            return paramExpandedUserDTO;
        }
        #endregion

        #region private void DeleteUser(User paramExpandedUserDTO)
        private void DeleteUser(User paramExpandedUserDTO)
        {
            ApplicationUser user =
                UserManager.FindByName(paramExpandedUserDTO.UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
            log.Info(string.Format("Deleted user {0}", paramExpandedUserDTO.UserName));
        }
        #endregion

        #region private UserAndRolesDTO GetUserAndRoles(string UserName)
        private UserAndRoles GetUserAndRoles(string UserName)
        {
            // Go get the User
            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
            DecoratedUserAndRoles unr = (DecoratedUserAndRoles) UnityConfig.DI.Resolve(typeof(IUserAndRoles));
            unr.UserNameOrId = UserName;
            return unr.Decorate();
        }
        #endregion

        #region private List<string> RolesUserIsNotIn(string UserName)
        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);

            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }

            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }

            return colRolesUserInNotIn;
        }
        #endregion
    }
}