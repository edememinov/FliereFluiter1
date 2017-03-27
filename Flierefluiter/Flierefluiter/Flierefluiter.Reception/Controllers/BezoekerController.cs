using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Concrete;
using Flierefluiter.Reception.Models;
using System.Data.Entity;

namespace Flierefluiter.Reception.Controllers
{
    public class BezoekerController : Controller
    {

        private DefaultConnection db = new DefaultConnection();
        private IFlierefluiterRepository repository;
        List<int?> idList = new List<int?>();
        List<Plaats> LeegVeldList = new List<Plaats>();


        public BezoekerController(IFlierefluiterRepository repository)
        {
            this.repository = repository;

        }

        [HttpGet]
        public ActionResult ReserveringToevoegen(ReserveringModel reserveringsModel)
        {
            reserveringsModel.Plaatsen = repository.Plaatss;

            return View(reserveringsModel);
        }

        [HttpPost]
        [ActionName("ReserveringToevoegen")]
        public ActionResult ReserveringToevoegenPost(ReserveringModel reserveringsModel)
        {

                reserveringsModel.Velden = repository.Velds.ToList();
                reserveringsModel.Plaatsen = repository.Plaatss.ToList();
                reserveringsModel.Reserveringen = repository.Reserverings.Where(p => p.BeginDatum >= reserveringsModel.Reservering.BeginDatum && p.BeginDatum <= reserveringsModel.Reservering.EindDatum || p.EindDatum <= reserveringsModel.Reservering.BeginDatum && p.EindDatum >= reserveringsModel.Reservering.EindDatum).ToList();
                reserveringsModel.Boekingen = repository.Boekings.Where(d => d.BeginDatum >= reserveringsModel.Reservering.BeginDatum && d.BeginDatum <= reserveringsModel.Reservering.EindDatum || d.EindDatum <= reserveringsModel.Reservering.BeginDatum && d.EindDatum >= reserveringsModel.Reservering.EindDatum).ToList();
   
                Reservering resv = new Reservering
                {
                    BeginDatum = reserveringsModel.Reservering.BeginDatum,
                    EindDatum = reserveringsModel.Reservering.EindDatum,
                    Email = reserveringsModel.Reservering.Email,
                    Naam = reserveringsModel.Reservering.Naam,
                    Telnr = reserveringsModel.Reservering.Telnr,
                    PlaatsId = reserveringsModel.Reservering.PlaatsId,
                    AantalPersonen = reserveringsModel.Reservering.AantalPersonen

                };

            resv.Plaats = reserveringsModel.Plaatsen.Where(p => p.PlaatsID.Equals(reserveringsModel.Reservering.PlaatsId)).First();


                if (reserveringsModel.Reserveringen != null)
                {

                    int i = 0;


                    foreach (var id in reserveringsModel.Reserveringen)
                    {
                        idList.Add(id.PlaatsId);
                    }
                    foreach (var idB in reserveringsModel.Boekingen)
                    {
                        if (!idList.Contains(idB.PlaatsId))
                        {
                            idList.Add(idB.PlaatsId);
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

                                return RedirectToAction("GeenPlek");
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            resv.PlaatsId = PlaatsArray[i].PlaatsID;
                        }

                    resv.Plaats = reserveringsModel.Plaatsen.Where(p => p.PlaatsID.Equals(reserveringsModel.Reservering.PlaatsId)).First();
                    resv.VeldID = reserveringsModel.Velden.Where(p => p.VeldID.Equals(resv.PlaatsId)).First().VeldID;
                    resv.Plaats = null;

                    if (ModelState.IsValid)
                        {


                            if (reserveringsModel.Reservering.EindDatum > reserveringsModel.Reservering.BeginDatum)
                            {
                                
                                db.Reserverings.Add(resv);
                                db.SaveChanges();
                                return RedirectToAction("ReserveringSucces");
                            }
                        }
                        return View(reserveringsModel);

                    }
                }

                else
                {
                    if (ModelState.IsValid)
                    {


                        if (reserveringsModel.Reservering.EindDatum > reserveringsModel.Reservering.BeginDatum)
                        {
                            resv.Plaats = reserveringsModel.Plaatsen.Where(p => p.PlaatsID.Equals(reserveringsModel.Reservering.PlaatsId)).First();
                            resv.VeldID = reserveringsModel.Velden.Where(p => p.VeldID.Equals(resv.PlaatsId)).First().VeldID;
                            db.Reserverings.Add(resv);
                            db.SaveChanges();
                            return RedirectToAction("ReserveringSucces");
                        }
                    }

                    return View(reserveringsModel);
                }

                return View(reserveringsModel);

            
        }

        [HttpGet]
        public ActionResult ListEmptySpot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListEmptySpot(BoekingViewModel Velden, DateTime beginDate, DateTime endDate)
        {
            Velden.Resveringens = repository.Reserverings.Where(p => p.BeginDatum >= beginDate && p.BeginDatum <= endDate || p.EindDatum <= beginDate && p.EindDatum >= endDate).ToList();
            Velden.Boekings = repository.Boekings.Where(p => p.BeginDatum >= beginDate && p.BeginDatum <= endDate || p.EindDatum <= beginDate && p.EindDatum >= endDate).ToList();
            Velden.Plaatsen = repository.Plaatss;

            int i = 0;

            foreach (var id in Velden.Resveringens)
            {
                idList.Add(id.PlaatsId);
            }
            foreach (var idB in Velden.Boekings)
            {
                if (!idList.Contains(idB.PlaatsId))
                {
                    idList.Add(idB.PlaatsId);
                }

            }

            var resvIdArray = idList.ToArray();
            var PlaatsArray = Velden.Plaatsen.ToArray();

            while (i < PlaatsArray.Length)
            {

                if (resvIdArray.Contains(PlaatsArray[i].PlaatsID))
                {
                    i++;
                }
                else
                {
                    LeegVeldList.Add(PlaatsArray[i]);
                    i++;
                }

            }
            Velden.Plaatsen = LeegVeldList;
            return View(Velden);
        }

        [HttpGet]
        public ActionResult PlaatsList(BoekingViewModel Velden)
        {
            Velden.Plaatsen = repository.Plaatss;

            return View(Velden);
        }


        [HttpGet]
        public ActionResult EditVeld(PlaatsenViewModel Velden, int id)
        {
            Velden.Veld = repository.Velds.Where(x => x.VeldID.Equals(id)).FirstOrDefault();

            return View(Velden);
        }

        [HttpPost]
        [ActionName("EditVeld")]
        public ActionResult EditVeldPost(PlaatsenViewModel Velden, int id)
        {
            Veld veld = new Veld
            {
                Amp = Velden.Veld.Amp,
                CAI = Velden.Veld.CAI,
                Naam = Velden.Veld.Naam,
                Oppervlak = Velden.Veld.Oppervlak,
                PrijsPerDag = Velden.Veld.PrijsPerDag,
                Riool = Velden.Veld.Riool,
                TypePlaats = Velden.Veld.TypePlaats,
                VeldID = Velden.Veld.VeldID,
                Water = Velden.Veld.Water,
                Wifi = Velden.Veld.Wifi,


            };
            db.Entry(Velden.Veld).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ReserveringSucces", "Bezoeker", new { area = "" });
        }

        [HttpGet]
        public ActionResult PlaatsToevoegen(PlaatsenViewModel plaatsen)
        {
            plaatsen.PlaatsVeld = repository.Velds;
            return View(plaatsen);
        }

        [HttpGet]
        public ActionResult VeldList(PlaatsenViewModel plaatsen)
        {
            plaatsen.Velden = repository.Velds;
            return View(plaatsen);
        }

        [HttpPost]
        [ActionName("PlaatsToevoegen")]
        public ActionResult PlaatsToevoegenPost(PlaatsenViewModel plaatsen)
        {
            Plaats place = new Plaats
            {
                Bezet = plaatsen.Plek.Bezet,
                NaamPlaats = plaatsen.Plek.NaamPlaats,
                PlaatsID = plaatsen.Plek.PlaatsID,
                PrijsStandaard = plaatsen.Plek.PrijsStandaard,
                VeldID = plaatsen.Plek.VeldID,

            };

            if (ModelState.IsValid)
            {
                db.Plaatss.Add(place);
                db.SaveChanges();
                return RedirectToAction("ReserveringSucces");
            }
            return RedirectToAction("Geenplek");
        }


        [HttpPost]
        public ActionResult VeldToevoegen(Veld veld)
        {
            Veld field = new Veld
            {
                Amp = veld.Amp,
                CAI = veld.CAI,
                Naam = veld.Naam,
                Oppervlak = veld.Oppervlak,
                PrijsPerDag = veld.PrijsPerDag,
                Riool = veld.Riool,
                TypePlaats = veld.TypePlaats,
                VeldID = veld.VeldID,
                Water = veld.Water,
                Wifi = veld.Wifi,


            };

            if (ModelState.IsValid)
            {
                db.Velds.Add(field);
                db.SaveChanges();
                return RedirectToAction("ReserveringSucces");
            }
            return RedirectToAction("Geenplek");
        }

        [HttpGet]
        public ActionResult VeldToevoegen()
        {
            return View();
        }


        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            ReserveringModel reservering = new ReserveringModel
            {
                Plaatsen = repository.Plaatss.ToList(),
                Velden = repository.Velds.ToList()

            };
            return View(reservering);
        }

        [HttpGet]
        public ActionResult ReserveringSucces()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GeenPlek()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ReserveringModel reserveringsModel)
        {

            reserveringsModel.Velden = repository.Velds.Where(p => p.VeldID.Equals(reserveringsModel.Reservering.VeldID)).ToList();
            reserveringsModel.Plaatsen = repository.Plaatss.Where(p => p.VeldID.Equals(reserveringsModel.Reservering.VeldID)).ToList();
            Plaats GekozenPlaats = reserveringsModel.Plaatsen.First();
            Veld GekozenVeld = reserveringsModel.Velden.FirstOrDefault();
            reserveringsModel.Reserveringen = repository.Reserverings.Where(p => p.BeginDatum >= reserveringsModel.Reservering.BeginDatum && p.BeginDatum <= reserveringsModel.Reservering.EindDatum || p.EindDatum <= reserveringsModel.Reservering.BeginDatum && p.EindDatum >= reserveringsModel.Reservering.EindDatum).ToList();
            reserveringsModel.Boekingen = repository.Boekings.Where(d => d.BeginDatum >= reserveringsModel.Reservering.BeginDatum && d.BeginDatum <= reserveringsModel.Reservering.EindDatum || d.EindDatum <= reserveringsModel.Reservering.BeginDatum && d.EindDatum >= reserveringsModel.Reservering.EindDatum).ToList();

            Reservering resv = new Reservering
            {
                BeginDatum = reserveringsModel.Reservering.BeginDatum,
                EindDatum = reserveringsModel.Reservering.EindDatum,
                Email = reserveringsModel.Reservering.Email,
                Naam = reserveringsModel.Reservering.Naam,
                Telnr = reserveringsModel.Reservering.Telnr,
                PlaatsId = GekozenPlaats.PlaatsID,
                VeldID = GekozenVeld.VeldID,
                AantalPersonen = reserveringsModel.Reservering.AantalPersonen,

            };


            if (reserveringsModel.Reserveringen != null)
            {

                int i = 0;


                foreach (var id in reserveringsModel.Reserveringen)
                {
                    idList.Add(id.PlaatsId);
                }
                foreach (var idB in reserveringsModel.Boekingen)
                {
                    if (!idList.Contains(idB.PlaatsId))
                    {
                        idList.Add(idB.PlaatsId);
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

                            return RedirectToAction("GeenPlek");



                        }
                        else
                        {
                            continue;
                        }



                    }
                    else
                    {

                        resv.PlaatsId = PlaatsArray[i].PlaatsID;


                    }





                    if (ModelState.IsValid)
                    {


                        if (reserveringsModel.Reservering.EindDatum > reserveringsModel.Reservering.BeginDatum)
                        {



                            db.Reserverings.Add(resv);
                            db.SaveChanges();
                            return RedirectToAction("ReserveringSucces");
                        }
                    }

                    return View(reserveringsModel);







                }
            }

            else
            {
                if (ModelState.IsValid)
                {


                    if (reserveringsModel.Reservering.EindDatum > reserveringsModel.Reservering.BeginDatum)
                    {



                        db.Reserverings.Add(resv);
                        db.SaveChanges();
                        return RedirectToAction("ReserveringSucces");
                    }
                }

                return View(reserveringsModel);
            }

            return View(reserveringsModel);

        }
    








    }
}