using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BetaAirlinesMVC.Models;

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
            //ViewBag.UserRoleID = new SelectList(db.UserRoles, "Id", "Role");
            // Name the item in the viewbag whatever you'd like. ViewBag.[CustomName]
            ViewBag.UserRole = db.UserRoles.ToList();
            return View();
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
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserRoleID = new SelectList(db.UserRoles, "Id", "Role", user.UserRoleID);
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
    }
}
