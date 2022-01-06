using BetaAirlinesMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BetaAirlinesMVC.Utilities
{
    public class DataValidation : Controller
    {

        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();

        public bool IsAuthenticated(int id, string password)
        {
            bool isAuthenticated = false;

            // Retrieve PW Hash using -- bool verified = BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);
            // from website: https://jasonwatmore.com/post/2021/05/27/net-5-hash-and-verify-passwords-with-bcrypt

            // Verifiy user is in the database
            User user = db.Users.Find(id);
            if(user != null)
            {
                // Verify the hashed password in the database
                if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    isAuthenticated = true;
                }
            }
            return isAuthenticated;
        }

        public bool PWMatch(int userId, string formPW)
        {
            bool match = false;
            User user = db.Users.Find(userId);
            if(user.Password == formPW)
            {
                match = true;
            }

            return match;
        }

        public bool UsernameAlreadyExists(string username)
        {
            bool alreadyExists = false;

            User user = db.Users.Where(x => x.Username == username).SingleOrDefault();
            if(user != null)
            {
                alreadyExists = true;
            }

            return alreadyExists;
        }

        public bool RoleApproved(string userRole, string Controller, string Action)
        {
            bool actionApproved = false;

            // General User ("ALL" if is able to access all pages under the controller)
            string[,] generalUserActionsApproved = new string[,]
            {
                { "Contact", "Index" },
                { "Contact", "Confirmation" },
                { "BookedFlights", "Index" },
                { "Flights", "BookAFlight" },
                { "Users", "Login" }
            };

            // Manager User ("ALL" if is able to access all pages under the controller)
            string[,] mgrActionsApproved = new string[,]
            {
                { "Manage", "Index" },
                { "BookedFlights", "ALL" },
                { "Contact", "ALL" },
                { "Flights", "ALL" },
                { "Users", "ALL" }
            };

            // Admin User Approved Pages ("ALL" if is able to access all pages under the controller)
            string[,] adminActions = new string[,] {
                { "Admin", "Index" },
                { "Airports", "ALL" },
                { "BookedFlights", "ALL" },
                { "Contact", "ALL" },
                { "Flights", "ALL" },
                { "UserRoles", "ALL" },
                { "Users", "ALL" }
            };

            // Check if the User has access to the page that is being requested
            if (userRole == "General")
            {
                for(int i = 0; i < generalUserActionsApproved.GetLength(0); i++)
                {
                    if(generalUserActionsApproved[i, 0] == Controller && (generalUserActionsApproved[i, 1] == Action || generalUserActionsApproved[i, 1] == "ALL"))
                    {
                        actionApproved = true;
                    }
                }
            } else if (userRole == "Manager")
            {
                for (int i = 0; i < mgrActionsApproved.GetLength(0); i++)
                {
                    if (mgrActionsApproved[i, 0] == Controller && (mgrActionsApproved[i, 1] == Action || mgrActionsApproved[i, 1] == "ALL"))
                    {
                        actionApproved = true;
                    }
                }
            } else if (userRole == "Admin")
            {
                for (int i = 0; i < adminActions.GetLength(0); i++)
                {
                    if (adminActions[i, 0] == Controller && (adminActions[i, 1] == Action || adminActions[i, 1] == "ALL"))
                    {
                        actionApproved = true;
                    }
                }
            }          

            return actionApproved;

        }
    }

    public class SessionCheck: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpSessionStateBase session = filterContext.HttpContext.Session;
            bool nullSessionParamFound = session == null || (session != null && (session["id"] == null || session["pw"] == null || session["role"] == null));
            bool allSessionParamsAvail = session["id"] != null && session["pw"] != null && session["role"] != null;

            bool idMatch = false;
            bool roleMatch = false;

            // User to get the userRole to make sure they have access to the controller page
            BetaAirlinesDbContext db = new BetaAirlinesDbContext();
            User user = db.Users.Find(session["id"]);

            DataValidation dv = new DataValidation();
            if (allSessionParamsAvail) {
                idMatch = dv.PWMatch((int)session["id"], session["pw"].ToString());

                // check to see if the db role matches with who's logged in
                if (user.UserRole.Role == session["role"].ToString())
                {
                    roleMatch = true;
                }

            }

            // determine requesting Controller and Action name to determine if they have the correct rights for the page
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            // if session for un or pw isn't set, or does not match what's in the database
            // this also validates the password in case the password has changed mid session
            // VALIDATE ROLE: also redirect to login if role does not match (if manually put in page, but doesn't have that role)
            if ((nullSessionParamFound || !allSessionParamsAvail || !idMatch || !roleMatch)
                || (idMatch &&
                    (!dv.RoleApproved(user.UserRole.Role, controllerName, actionName))))
            {
                // Redirect to the login page
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { 
                    {"Controller", "Users"},
                    { "Action", "Login"}
                    });
            }
        }
    }
}