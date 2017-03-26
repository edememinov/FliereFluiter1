using System.Web.Mvc;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Concrete;
using Flierefluiter.Reception.Models;

namespace Flierefluiter.Reception.Controllers
{
    public class BoekingController : Controller
    {
        // GET: Veld
        private DefaultConnection db = new DefaultConnection();
        private IFlierefluiterRepository repository;

        public BoekingController(IFlierefluiterRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        // GET: Home
        [HttpGet]
        public ActionResult ReserveringList(BoekingViewModel boekingen)
        {
            boekingen.Resveringens = repository.Reserverings;

            return View(boekingen);
        }

        [Authorize]
        // GET: Home
        [HttpGet]
        public ActionResult Boeking()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Boeking(Boeking boeking)
        {
            if (ModelState.IsValid)
            {
                db.Boekings.Add(boeking);
                db.SaveChanges();
                return RedirectToAction("Veld");
            }

            return View(boeking);
        }
    }
}