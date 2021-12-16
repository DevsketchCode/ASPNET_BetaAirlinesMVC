using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BetaAirlinesMVC.Models;
using BetaAirlinesMVC.ViewModel;
using BetaAirlinesMVC.Utilities;

namespace BetaAirlinesMVC.Controllers
{
    // Uses BetaAirlinesMVC.Utilities to run a SessionCheck
    // Having it here runs the session check in all actions on this controller
    // Else place it only on the actions that you want it on
    [SessionCheck]
    public class FlightsController : Controller
    {
        private BetaAirlinesDbContext db = new BetaAirlinesDbContext();
        private DataValidation dv = new DataValidation();

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
        public ActionResult BookAFlight(int? dpt, int? arr)
        {

            BookAFlightViewModel model = new BookAFlightViewModel();

            try
            {
                // Get all the list of airports
                ViewBag.dpt = new SelectList(db.Airports, "Id", "Name"); // Departure
                ViewBag.arr = new SelectList(db.Airports, "Id", "Name"); // Arrival
                ViewBag.UserID = Session["id"];

                // Get the list of predetermined flights for the user to choose from
                if (dpt != null && arr != null)
                {
                    model.FlightList = new List<Flight>(db.Flights.Where(x => x.DepartureAirportId == dpt && x.ArrivalAirportId == arr));
                }
                else if ((dpt != null && arr == null) || (dpt == null && arr != null))
                {
                    model.FlightList = new List<Flight>(db.Flights.Where(x => x.DepartureAirportId == dpt || x.ArrivalAirportId == arr));
                }
                else
                {
                    model.FlightList = db.Flights.ToList();
                }
            } catch(Exception ex)
            {
                model = null;
            }

            return View(model);
        }
    }
}
