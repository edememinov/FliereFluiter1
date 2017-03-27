using System.Web.Mvc;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Concrete;
using Flierefluiter.Reception.Models;
using System.Linq;
using System.Collections.Generic;

namespace Flierefluiter.Reception.Controllers
{
    public class BoekingController : Controller
    {
        // GET: Veld
        private DefaultConnection db = new DefaultConnection();
        private IFlierefluiterRepository repository;
        List<int?> idList = new List<int?>();

        public BoekingController(IFlierefluiterRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        // GET: Home
        [HttpGet]
        public ActionResult BoekingList(BoekingViewModel boekingen)
        {
            boekingen.Boekings = repository.Boekings;

            return View(boekingen);
        }

        [Authorize]
        // GET: Home
        [HttpGet]
        public ActionResult ReserveringList(BoekingViewModel boekingen)
        {
            boekingen.Resveringens = repository.Reserverings;

            return View(boekingen);
        }

        [HttpGet]
        public ActionResult ViewBoeking(BoekingViewModel boeking, int id)
        {
            boeking.Boeking = repository.Boekings.Where(x => x.BoekingID.Equals(id)).FirstOrDefault();

            return View(boeking);
        }

        [HttpGet]
        public ActionResult EditBoeking(BoekingViewModel boeking, int id)
        {
            boeking.Boeking = repository.Boekings.Where(x => x.BoekingID.Equals(id)).FirstOrDefault();
            boeking.Velden = repository.Velds;

            return View(boeking);
        }

        [HttpPost]
        [ActionName("EditBoeking")]
        public ActionResult EditBoekingPost(BoekingViewModel reserveringsModel)
        {

            reserveringsModel.Velden = repository.Velds.Where(p => p.VeldID.Equals(reserveringsModel.Reserv.VeldID)).ToList();
            reserveringsModel.Plaatsen = repository.Plaatss.Where(p => p.VeldID.Equals(reserveringsModel.Reserv.VeldID)).ToList();
            Plaats GekozenPlaats = reserveringsModel.Plaatsen.First();
            Veld GekozenVeld = reserveringsModel.Velden.FirstOrDefault();
            reserveringsModel.Resveringens = repository.Reserverings.Where(p => p.BeginDatum >= reserveringsModel.Reserv.BeginDatum && p.BeginDatum <= reserveringsModel.Reserv.EindDatum || p.EindDatum <= reserveringsModel.Reserv.BeginDatum && p.EindDatum >= reserveringsModel.Reserv.EindDatum).ToList();
            reserveringsModel.Boekings = repository.Boekings.Where(p => p.BeginDatum >= reserveringsModel.Boeking.BeginDatum && p.BeginDatum <= reserveringsModel.Boeking.EindDatum || p.EindDatum <= reserveringsModel.Boeking.BeginDatum && p.EindDatum >= reserveringsModel.Boeking.EindDatum).ToList();

            if (GekozenPlaats == null)
            {
                db.SaveChanges();
                return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
            }

            if (reserveringsModel.Resveringens != null)
            {

                int i = 0;


                foreach (var id in reserveringsModel.Resveringens)
                {
                    idList.Add(id.PlaatsId);
                }
                foreach (var id in reserveringsModel.Boekings)
                {
                    if (!idList.Contains(id.PlaatsId))
                    {
                        idList.Add(id.PlaatsId);
                    }

                }

                var resvIdArray = idList.ToArray();
                var PlaatsArray = reserveringsModel.Plaatsen.ToArray();

                while (i <= PlaatsArray.Length)
                {

                    if (resvIdArray.Contains(PlaatsArray[i].PlaatsID))
                    {
                        i++;

                        if (i == PlaatsArray.Length)
                        {

                            //throw new Exception("Geen plek");

                            return RedirectToAction("GeenPlek", "Bezoeker", new { area = "" });
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        reserveringsModel.Reserv.PlaatsId = PlaatsArray[i].PlaatsID;
                    }


                    if (ModelState.IsValid)
                    {


                        if (reserveringsModel.Reserv.EindDatum > reserveringsModel.Reserv.BeginDatum)
                        {
                            db.Entry(reserveringsModel.Boeking).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
                        }
                    }

                    return View(reserveringsModel);
                }
            }

            else
            {
                if (ModelState.IsValid)
                {
                    if (reserveringsModel.Reserv.EindDatum > reserveringsModel.Reserv.BeginDatum)
                    {
                        db.Entry(reserveringsModel.Reserv).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
                    }
                }

                return View(reserveringsModel);
            }

            return View(reserveringsModel);

        }

        [HttpGet]
        public ActionResult AddBoeking(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Reserv = repository.Reserverings.Where(x => x.ReserveringID.Equals(id)).FirstOrDefault();
            reserveringen.Boeking = new Boeking
            {
                AantalPersonen = reserveringen.Reserv.AantalPersonen,
                BeginDatum = reserveringen.Reserv.BeginDatum,
                EindDatum = reserveringen.Reserv.EindDatum,
                Email = reserveringen.Reserv.Email,
                Naam = reserveringen.Reserv.Naam,
                VeldID = reserveringen.Reserv.VeldID,
                Telnr = reserveringen.Reserv.Telnr,
                PlaatsId = reserveringen.Reserv.PlaatsId
            };

            return View(reserveringen);
        }

        [HttpGet]
        public ActionResult DeleteBoeking(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Boeking = repository.Boekings.Where(x => x.BoekingID.Equals(id)).FirstOrDefault();

            return View(reserveringen);
        }

        [HttpPost]
        [ActionName("DeleteBoeking")]
        public ActionResult DeleteBoekingPost(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Boeking = repository.Boekings.Where(x => x.BoekingID.Equals(id)).FirstOrDefault();

            Boeking boek = new Boeking
            {
                AantalPersonen = reserveringen.Boeking.AantalPersonen,
                BeginDatum = reserveringen.Boeking.BeginDatum,
                EindDatum = reserveringen.Boeking.EindDatum,
                Email = reserveringen.Boeking.Email,
                Naam = reserveringen.Boeking.Naam,
                PlaatsId = reserveringen.Boeking.PlaatsId,
                BoekingID = reserveringen.Boeking.BoekingID,
                Telnr = reserveringen.Boeking.Telnr,
                VeldID = reserveringen.Boeking.VeldID
            };
            db.Boekings.Attach(boek);
            db.Boekings.Remove(boek);
            db.SaveChanges();

            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
        }

        [HttpGet]
        public ActionResult DeleteResView(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Reserv = repository.Reserverings.Where(x => x.ReserveringID.Equals(id)).FirstOrDefault();

            return View(reserveringen);
        }

        [HttpPost]
        [ActionName("DeleteResView")]
        public ActionResult DeleteResViewPost(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Reserv = repository.Reserverings.Where(x => x.ReserveringID.Equals(id)).FirstOrDefault();

            Reservering res = new Reservering
            {
                AantalPersonen = reserveringen.Reserv.AantalPersonen,
                BeginDatum = reserveringen.Reserv.BeginDatum,
                EindDatum = reserveringen.Reserv.EindDatum,
                Email = reserveringen.Reserv.Email,
                Naam = reserveringen.Reserv.Naam,
                PlaatsId = reserveringen.Reserv.PlaatsId,
                ReserveringID = reserveringen.Reserv.ReserveringID,
                Telnr = reserveringen.Reserv.Telnr,
                VeldID = reserveringen.Reserv.VeldID
            };
            db.Reserverings.Attach(res);
            db.Reserverings.Remove(res);
            db.SaveChanges();

            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
        }

        [HttpPost]
        [ActionName("AddBoeking")]
        public ActionResult AddBoekingPost(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Reserv = repository.Reserverings.Where(x => x.ReserveringID.Equals(id)).FirstOrDefault();
            reserveringen.Boeking = new Boeking
            {
                AantalPersonen = reserveringen.Boeking.AantalPersonen,
                BeginDatum = reserveringen.Boeking.BeginDatum,
                EindDatum = reserveringen.Boeking.EindDatum,
                Email = reserveringen.Boeking.Email,
                Naam = reserveringen.Boeking.Naam,
                VeldID = reserveringen.Boeking.VeldID,
                Adres = reserveringen.Boeking.Adres,
                Geboortedatum = reserveringen.Boeking.Geboortedatum,
                PaspoortID = reserveringen.Boeking.PaspoortID,
                Postcode = reserveringen.Boeking.Postcode,
                Plaats = reserveringen.Boeking.Plaats,
                Telnr = reserveringen.Boeking.Telnr,
                Woonplaats = reserveringen.Boeking.Woonplaats,
                PlaatsId = reserveringen.Boeking.PlaatsId
                
                
            };

            Reservering res = new Reservering
            {
                AantalPersonen = reserveringen.Reserv.AantalPersonen,
                BeginDatum = reserveringen.Reserv.BeginDatum,
                EindDatum = reserveringen.Reserv.EindDatum,
                Email = reserveringen.Reserv.Email,
                Naam = reserveringen.Reserv.Naam,
                PlaatsId = reserveringen.Reserv.PlaatsId,
                ReserveringID = reserveringen.Reserv.ReserveringID,
                Telnr = reserveringen.Reserv.Telnr,
                VeldID = reserveringen.Reserv.VeldID
            };
            db.Reserverings.Attach(res);
            db.Reserverings.Remove(res);
            db.Boekings.Add(reserveringen.Boeking);
            db.SaveChanges();

            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
        }

        [HttpGet]
        public ActionResult EditReservation(BoekingViewModel reserveringen, int id)
        {
            reserveringen.Velden = repository.Velds;
            reserveringen.Reserv = repository.Reserverings.Where(x => x.ReserveringID.Equals(id)).FirstOrDefault();

            return View(reserveringen);
        }

        [HttpPost]
        [ActionName("EditReservation")]
        public ActionResult EditReservationPost(BoekingViewModel reserveringsModel)
        {

            reserveringsModel.Velden = repository.Velds.Where(p => p.VeldID.Equals(reserveringsModel.Reserv.VeldID)).ToList();
            reserveringsModel.Plaatsen = repository.Plaatss.Where(p => p.VeldID.Equals(reserveringsModel.Reserv.VeldID)).ToList();
            Plaats GekozenPlaats = reserveringsModel.Plaatsen.First();
            Veld GekozenVeld = reserveringsModel.Velden.FirstOrDefault();
            reserveringsModel.Resveringens = repository.Reserverings.Where(p => p.BeginDatum >= reserveringsModel.Reserv.BeginDatum && p.BeginDatum <= reserveringsModel.Reserv.EindDatum || p.EindDatum <= reserveringsModel.Reserv.BeginDatum && p.EindDatum >= reserveringsModel.Reserv.EindDatum).ToList();
            reserveringsModel.Boekings = repository.Boekings.Where(p => p.BeginDatum >= reserveringsModel.Boeking.BeginDatum && p.BeginDatum <= reserveringsModel.Boeking.EindDatum || p.EindDatum <= reserveringsModel.Boeking.BeginDatum && p.EindDatum >= reserveringsModel.Boeking.EindDatum).ToList();

            if (GekozenPlaats == null)
            {
                db.SaveChanges();
                return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
            }

            if (reserveringsModel.Resveringens != null)
            {

                int i = 0;


                foreach (var id in reserveringsModel.Resveringens)
                {
                    idList.Add(id.PlaatsId);
                }
                foreach (var id in reserveringsModel.Boekings)
                {
                    if (!idList.Contains(id.PlaatsId))
                    {
                        idList.Add(id.PlaatsId);
                    }
                    
                }

                var resvIdArray = idList.ToArray();
                var PlaatsArray = reserveringsModel.Plaatsen.ToArray();

                while (i <= PlaatsArray.Length)
                {

                    if (resvIdArray.Contains(PlaatsArray[i].PlaatsID))
                    {
                        i++;

                        if (i == PlaatsArray.Length)
                        {

                            //throw new Exception("Geen plek");

                            return RedirectToAction("GeenPlek", "Bezoeker", new { area = "" });
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        reserveringsModel.Reserv.PlaatsId = PlaatsArray[i].PlaatsID;
                    }


                    if (ModelState.IsValid)
                    {


                        if (reserveringsModel.Reserv.EindDatum > reserveringsModel.Reserv.BeginDatum)
                        {
                            db.Entry(reserveringsModel.Reserv).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
                        }
                    }

                    return View(reserveringsModel);
                }
            }

            else
            {
                if (ModelState.IsValid)
                {
                    if (reserveringsModel.Reserv.EindDatum > reserveringsModel.Reserv.BeginDatum)
                    {
                        db.Entry(reserveringsModel.Reserv).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
                    }
                }

                return View(reserveringsModel);
            }

            return View(reserveringsModel);

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