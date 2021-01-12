using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mini_Prj_.Models;

namespace Mini_Prj_.Controllers
{
    public class AbonnementsController : Controller
    {
        private NavetteDB_Entities db = new NavetteDB_Entities();

        //Post DeleteA
        public async Task<ActionResult> DeleteA(int id)
        {
            int idC = int.Parse(Session["Usrid"].ToString());
            var a = from aff in db.Affectations where aff.idAbonnement == id && aff.idClient == idC select aff;
            db.Affectations.Remove(a.FirstOrDefault());
            await db.SaveChangesAsync();
            return RedirectToAction("MesAbonnements");
        }
        // Get: MesAbonnements
        public async Task<ActionResult> MesAbonnements()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                int idC = int.Parse(Session["Usrid"].ToString());
                var query =await (from a in db.Affectations where a.idClient==idC  select a).ToListAsync(); 
                return View(query);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // Post: Reserver
        public async Task<ActionResult> Reserver(int idA)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null && Session["Usrid"]!=null)
            {
                int idC = int.Parse(Session["Usrid"].ToString());
                var check = (from a in db.Affectations where a.idClient == idC && a.idAbonnement == idA select a).FirstOrDefault();
                if (check == null)
                {
                    db.Affectations.Add(new Affectation { idAbonnement = idA, idClient = idC, date_affectation = DateTime.Now });
                    await db.SaveChangesAsync();
                    ViewBag.dejaExist = "";
                    return RedirectToAction("MesAbonnements", "Abonnements");
                }
                else
                {
                    var query = (from a in db.Abonnements 
                                 join t in db.Trajets on a.idTrajet equals t.id 
                                 where a.id == idA  select t).FirstOrDefault();
                    ViewBag.dejaExist = "Vous etes deja inscrit a cette Abonnement";
                    return RedirectToAction("Offres", "Abonnements",new {depart=query.depart,arriver=query.arriver });
                }
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }


        // GET: Offres
        public async Task<ActionResult> Offres(string depart,string arriver)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            //List<SelectListItem> villesD = new List<SelectListItem>();
            //List<SelectListItem> villesA = new List<SelectListItem>();
            //foreach (var v in db.Villes)
            //{
            //    villesD.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
            //    villesA.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
            //}
            //villesD.Find(v => v.Value == depart).Selected = true;
            //villesA.Find(v => v.Value == arriver).Selected = true;
            //ViewBag.villesD = villesD;
            //ViewBag.villesA = villesA;
            ViewBag.depart = new SelectList(db.Villes, "ville1", "ville1", depart);
            ViewBag.arriver = new SelectList(db.Villes, "ville1", "ville1", arriver);
            var abonmnt = (from a in db.Abonnements join s in db.Societes
                           on a.idSociete equals s.id 
                           join t in db.Trajets on a.idTrajet equals t.id
                           join b in db.Buses on t.id equals b.idTrajet
                           where t.depart == depart && t.arriver == arriver 
                           group a by new
                           {
                               a.id,
                               a.date_debut,
                               a.date_fin,
                               t.date_arriver,
                               t.date_depart,
                               t.arriver,
                               t.depart,
                               b.typeDeVehicule,
                               a.tarif
                           } into t_
                           select new offre()
                           {
                               id=t_.Key.id,
                               date_arriver = t_.Key.date_arriver,
                               date_depart = t_.Key.date_depart,
                               arriver = t_.Key.arriver,
                               depart = t_.Key.depart,
                               typeDeVehicule = t_.Key.typeDeVehicule,
                               tarif = t_.Key.tarif,
                               date_debut=t_.Key.date_debut,
                               date_fin=t_.Key.date_fin,
                               nbBus =t_.Count()
                           });
            ViewBag.offres = await abonmnt.ToListAsync();
            //var abonmnt = db.Abonnements.Include(a => a.Societe).Include(a => a.Trajet).Where(t=>t.Trajet.depart== depart&&t.Trajet.arriver== arriver);
            return View();
        }

        // GET: Abonnements
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                var abonnements = db.Abonnements.Include(a => a.Societe).Include(a => a.Trajet);
                return View(await abonnements.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }

        }

        // GET: Abonnements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Abonnement abonnement = await db.Abonnements.FindAsync(id);
                if (abonnement == null)
                {
                    return HttpNotFound();
                }
                return View(abonnement);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Abonnements/Create
        public ActionResult Create()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                ViewBag.idTrajet = trajets;
                return View();
            }                
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Abonnements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,idTrajet,idSociete,date_debut,date_fin,tarif")] Abonnement abonnement)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (ModelState.IsValid)
            {
                db.Abonnements.Add(abonnement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            string help = "";
            List<SelectListItem> trajets = new List<SelectListItem>();
            ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
            foreach (var t in db.Trajets)
            {
                help = t.depart + " => " + t.arriver;
                trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
            }
            ViewBag.idTrajet = trajets;
            return View(abonnement);
        }

        // GET: Abonnements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Abonnement abonnement = await db.Abonnements.FindAsync(id);
                if (abonnement == null)
                {
                    return HttpNotFound();
                }
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                var query = (from t in db.Trajets where t.id == abonnement.idTrajet select t).FirstOrDefault();
                trajets.Find(t => t.Text == query.depart + " => " + query.arriver).Selected = true;
                ViewBag.idTrajet = trajets;
                return View(abonnement);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Abonnements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,idTrajet,idSociete,date_debut,date_fin,tarif")] Abonnement abonnement)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(abonnement).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                //ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                ViewBag.idTrajet = trajets;
                return View(abonnement);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Abonnements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Abonnement abonnement = await db.Abonnements.FindAsync(id);
                if (abonnement == null)
                {
                    return HttpNotFound();
                }
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                var query = (from t in db.Trajets where t.id == abonnement.idTrajet select t).FirstOrDefault();
                trajets.Find(t => t.Text == query.depart + " => " + query.arriver).Selected = true;
                ViewBag.idTrajet = trajets;
                return View(abonnement);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Abonnement abonnement = await db.Abonnements.FindAsync(id);
            db.Abonnements.Remove(abonnement);
            await db.SaveChangesAsync();
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
