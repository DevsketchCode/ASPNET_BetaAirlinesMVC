using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BetaAirlinesMVC.Models;
using BetaAirlinesMVC.ViewModel;
using BetaAirlinesMVC.Utilities;

namespace BetaAirlinesMVC.Controllers
{
    public class UsersController : Controller
    {
        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserRole);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);

            // Create the list of User Roles
            List<UserRole> roles = new List<UserRole>();

            foreach (var userRole in db.UserRoles.Where(e => e.Id == id && e.Active == 1))
            {
                // get the single user's role
                UserRole usersRole = db.UserRoles.Where(me => me.Id == userRole.Id).SingleOrDefault();

                UserRole urm = new UserRole();
                // Populate  the object with the information from the database
                urm.Role = usersRole.Role;
                urm.Description = usersRole.Description;

                // Add to the roles list
                roles.Add(urm);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {

            UserCreateViewModel model = new UserCreateViewModel();

            // set default values for the form
            model.RegisteredDate = DateTime.Today;
            model.Active = 1;
            
            // Name the item in the viewbag whatever you'd like. ViewBag.[CustomName]
            var userroleslist = db.UserRoles.ToList();
            SelectList list = new SelectList(userroleslist, "Id", "Role");
            ViewBag.UserRoleList = list;
            return View(model);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Username,Password,RegisteredDate,Active,UserRoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                ViewBag.UserRoleID = new SelectList(db.UserRoles, "Id", "Role", user.UserRoleID);

                DataValidation dataValidation = new DataValidation();
                if(dataValidation.UsernameAlreadyExists(user.Username))
                {
                    ViewBag.Message = "Username already exists.";
                    return View();
                } 
                else
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRoleID = new SelectList(db.UserRoles, "Id", "Role", user.UserRoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Username,Password,RegisteredDate,Active,UserRoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                //TODO: hash new password after edit
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserRoleID = new SelectList(db.UserRoles, "Id", "Role", user.UserRoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Users/Login
        [HttpGet]
        public ActionResult Login()
        {
            // If already logged in, then log out first. 
            if(Session["id"] != null || Session["pw"] != null) {
                Logout();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            UserLoginViewModel model = new UserLoginViewModel();

            if (userName == null || password == null)
            {
                ViewBag.Message = "Please enter the username and password";
                return View();
            }

            // Get user loging information from the database
            User user = db.Users.Where(x => x.Username == userName).SingleOrDefault();
            if (user == null)
            {
                ViewBag.Message = "Invalid Username or Password";
                return View();
            }
            UserRole userRole = db.UserRoles.Where(x => x.Id == user.UserRoleID).SingleOrDefault();
            
            // DELETE THIS!
            // FIX THIS LOGIN AND SET AS SESSION AS LOGGED IN

            DataValidation authenticate = new DataValidation();
            bool isVerified = authenticate.IsAuthenticated(user.Id, password);
            if (isVerified)
            {
                this.Session["id"] = user.Id;
                this.Session["pw"] = user.Password;
                this.Session["role"] = userRole.Role;
                if(userRole.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("BookAFlight", "Flights");
                }
                
            } else
            {
                ViewBag.Message = "Invalid Username or Password";
                this.Session.Clear();
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["pw"] = null;
            Session["role"] = null;
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.AddHeader("Cache-control", "no-store, must-revalidate, private, no-cache");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.AppendToLog("window.location.reload();");

            return RedirectToAction("Login", "Users");
        }
    }
}
