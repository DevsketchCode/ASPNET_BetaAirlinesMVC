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
    public class BookedFlightsController : Controller
    {
        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();

        // GET: BookedFlights
        public ActionResult Index()
        {
            var bookedFlights = db.BookedFlights.Include(b => b.Flight).Include(b => b.User);
            return View(bookedFlights.ToList());
        }

        // GET: BookedFlights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookedFlight bookedFlight = db.BookedFlights.Find(id);
            if (bookedFlight == null)
            {
                return HttpNotFound();
            }
            return View(bookedFlight);
        }

        // GET: BookedFlights/Create
        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: BookedFlights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateBooked,Active,UserId,FlightId")] BookedFlight bookedFlight)
        {
            if (ModelState.IsValid)
            {
                db.BookedFlights.Add(bookedFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", bookedFlight.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", bookedFlight.UserId);
            return View(bookedFlight);
        }

        // GET: BookedFlights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookedFlight bookedFlight = db.BookedFlights.Find(id);
            if (bookedFlight == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", bookedFlight.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", bookedFlight.UserId);
            return View(bookedFlight);
        }

        // POST: BookedFlights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateBooked,Active,UserId,FlightId")] BookedFlight bookedFlight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookedFlight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlightId = new SelectList(db.Flights, "Id", "Id", bookedFlight.FlightId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", bookedFlight.UserId);
            return View(bookedFlight);
        }

        // GET: BookedFlights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookedFlight bookedFlight = db.BookedFlights.Find(id);
            if (bookedFlight == null)
            {
                return HttpNotFound();
            }
            return View(bookedFlight);
        }

        // POST: BookedFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookedFlight bookedFlight = db.BookedFlights.Find(id);
            db.BookedFlights.Remove(bookedFlight);
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
