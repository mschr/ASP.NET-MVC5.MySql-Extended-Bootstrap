using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;

namespace my.ns.entities.decorators
{
    using IdentityConfig;
    public abstract class ADecorable
    {
        public ADecorable(ApplicationUserManager userManager) 
        {
            this._userManager = userManager;
        }

        protected Regex uuidRegEx = new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}", RegexOptions.IgnoreCase);

        private IDecorable _component;
        protected IDecorable component
        {
            get
            {
                if (this._component == null)
                    throw new ArgumentNullException("Decoratable has not been correctly initialized");
                return this._component;
            }
            set
            {
                this._component = value;
            }
        }

        private ApplicationUser _appUser;
        protected ApplicationUser AppUser
        {
            get
            {
                {
                    if (_appUser != null) return _appUser;
                    int uid = 0;
                    
                    if (UserNameOrId != null)
                    {
                        // for reqular uuid 
                        //if (uuidRegEx.Matches(_userNameOrId).Count > 0)
                        if(int.TryParse(_userNameOrId, out uid))
                        {
                            _appUser = UserManager.FindById(uid);
                        }
                        else
                        {
                            _appUser = UserManager.FindByName(UserNameOrId);
                        }
                    }
                    if (_appUser == null)
                    {
                        return null;
                    }
                    return _appUser;
                }
            }
            private set { _appUser = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager; }
            set { _userManager = value; }
        }
        private ApplicationUserManager _userManager;

        public string UserNameOrId
        {
            get { return _userNameOrId; }
            set { _userNameOrId = value; }
        }
        private string _userNameOrId;

        public ADecorable()
        {
        }
        public abstract dynamic Decorate();


    }
}
