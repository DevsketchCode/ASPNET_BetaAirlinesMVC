using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BetaAirlinesMVC.Models;
using BetaAirlinesMVC.Utilities;
using BetaAirlinesMVC.ViewModel;

namespace BetaAirlinesMVC.Controllers
{

    // Uses BetaAirlinesMVC.Utilities to run a SessionCheck
    // Having it here runs the session check in all actions on this controller
    // Else place it only on the actions that you want it on
    [SessionCheck]
    public class BookedFlightsController : Controller
    {
        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            List<MyFlightsViewModel> yourFlights = new List<MyFlightsViewModel>();

            try { 
            int loggedInUser = (int)Session["id"];
            foreach (var flight in db.BookedFlights.Where(x => x.UserId == loggedInUser))
                {
                    // Get flight data
                    Flight flights = db.Flights.Where(x => x.Id == flight.FlightId).SingleOrDefault();
                    User theUser = db.Users.Where(u => u.Id == flight.UserId).SingleOrDefault();
                    Airport depAirport = db.Airports.Where(a => a.Id == flights.DepartureAirportId).SingleOrDefault();
                    Airport arrAirport = db.Airports.Where(a => a.Id == flights.ArrivalAirportId).SingleOrDefault();

                    // Create my flight object
                    MyFlightsViewModel mfvm = new MyFlightsViewModel();
                    mfvm.Id = flight.Id;
                    mfvm.UserId = flight.UserId;
                    mfvm.FirstName = theUser.FirstName;
                    mfvm.LastName = theUser.LastName;
                    mfvm.DateBooked = flight.DateBooked;
                    mfvm.DepartureAirport = depAirport.Name;
                    mfvm.ArrivalAirport = arrAirport.Name;
                    mfvm.ActiveBookedFlight = flight.Active;


                    yourFlights.Add(mfvm);
                }
            } catch (Exception ex)
            {
                yourFlights = null;
                ViewBag.AlertMessage = "You have no booked flights.";
            }

            return View(yourFlights);
        }

        // GET: BookedFlights
        public ActionResult Admin()
        {
            var bookedFlights = db.BookedFlights.Include(b => b.Flight).Include(b => b.BookedUserId);
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
        public ActionResult Create(int usr, int fid, [Bind(Include = "Id,DateBooked,Active,UserId,FlightId")] BookedFlight bookedFlight)
        {
            if (ModelState.IsValid)
            {
                if(fid != null)
                {
                    bookedFlight.DateBooked = DateTime.Now;
                    bookedFlight.FlightId = fid;
                    bookedFlight.UserId = usr;
                    bookedFlight.Active = 1; // Default is active
                }
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
