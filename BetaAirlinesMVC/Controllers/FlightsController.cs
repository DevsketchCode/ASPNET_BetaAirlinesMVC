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

namespace BetaAirlinesMVC.Controllers
{
    public class FlightsController : Controller
    {
        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();

        // GET: Flights
        public ActionResult Index()
        {
            var flights = db.Flights.Include(f => f.ArrivalAirport).Include(f => f.DepartureAirport);
            return View(flights.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // GET: Flights/Create
        public ActionResult Create()
        {
            ViewBag.ArrivalAirportId = new SelectList(db.Airports, "Id", "Name");
            ViewBag.DepartureAirportId = new SelectList(db.Airports, "Id", "Name");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DepartureDate,DepartureAirportId,ArrivalAirportId,FlightLengthInMinutes,Active")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Flights.Add(flight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArrivalAirportId = new SelectList(db.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewBag.DepartureAirportId = new SelectList(db.Airports, "Id", "Name", flight.DepartureAirportId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArrivalAirportId = new SelectList(db.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewBag.DepartureAirportId = new SelectList(db.Airports, "Id", "Name", flight.DepartureAirportId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DepartureDate,DepartureAirportId,ArrivalAirportId,FlightLengthInMinutes,Active")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArrivalAirportId = new SelectList(db.Airports, "Id", "Name", flight.ArrivalAirportId);
            ViewBag.DepartureAirportId = new SelectList(db.Airports, "Id", "Name", flight.DepartureAirportId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);
            db.Flights.Remove(flight);
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


        // Book A Flight
        [HttpGet]
        public ActionResult BookAFlight(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get all the list of airports
            ViewBag.AirportsList = db.Airports.ToList();

            // Get the list of predetermined flights for the user to choose from
            ViewBag.FlightsList = db.Flights.ToList();

            return View();
        }

        // Post request handler for when they submit the form from the view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAFlight([Bind(Include = "DateBooked, UserId, FlightId")] BookAFlightViewModel bfvm)
        {

            if (bfvm != null)
            {
                // Get the userId from the form
                int UserId = db.Users.Where(e => e.Id == bfvm.UserId).Select(e => e.Id).SingleOrDefault();

                // Get the flight where the flight ID matches the flight that was selected on the form
                int FlightId = db.Flights.Where(e => e.Id == bfvm.FlightId).Select(e => e.Id).SingleOrDefault();

                BookedFlight bookedFlight = new BookedFlight();
                bookedFlight.UserId = UserId;
                bookedFlight.FlightId = FlightId;
                bookedFlight.DateBooked = Convert.ToDateTime(bfvm.DateBooked);
                bookedFlight.Active = 1; // Set Booked Flight to Active as it just being created

                db.BookedFlights.Add(bookedFlight);
                db.SaveChanges();

                // Send it back to the index page if it has been successfully submitted
                return RedirectToAction("Index");
            }

            // The form submission was unsuccessful
            return View(bfvm);
        }
    }
}
