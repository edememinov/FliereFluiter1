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
using Flierefluiter.WebUI.Models;

namespace Flierefluiter.WebUI.Controllers
{
    public class BezoekerController : Controller
    {

        private DefaultConnection db = new DefaultConnection();
        private IFlierefluiterRepository repository;
        List<int?> idList = new List<int?>();


        public BezoekerController(IFlierefluiterRepository repository)
        {
            this.repository = repository;

        }
        [HttpGet]
        public ActionResult PlaatsToevoegen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PlaatsToevoegen(Plaats plaats)
        {
           Plaats place = new Plaats
           {
               Bezet = plaats.Bezet,
               NaamPlaats = plaats.NaamPlaats,
               PlaatsID = plaats.PlaatsID,
               PrijsStandaard = plaats.PrijsStandaard,
               VeldID = plaats.VeldID,

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
            if(GekozenPlaats == null)
            {
                db.Reserverings.Add(resv);
                db.SaveChanges();
                return RedirectToAction("ReserveringSucces");
            }

            if (reserveringsModel.Reserveringen != null)
            {
              
                int i = 0;


                foreach (var id in reserveringsModel.Reserveringen)
                {
                    idList.Add(id.PlaatsId);
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