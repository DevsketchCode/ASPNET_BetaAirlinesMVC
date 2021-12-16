using BetaAirlinesMVC.Models;
using System.Linq;
using System.Web;
using System.Data;
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
    }

    public class SessionCheck: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if(session != null && session["id"] == null || session["pw"] == null)
            {

                //TODO: Get role to determine if should be shown or not.
                //TODO: Validate password in case the password has changed mid session
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { 
                    {"Controller", "Users"},
                    { "Action", "Login"}
                    });
            }
        }
    }
}