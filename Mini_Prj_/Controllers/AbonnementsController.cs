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

        public async Task<ActionResult> Offres()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            var abonmnt = (from s in db.Societes
                           join b in db.Buses on s.id equals b.idSociete
                           join t in db.Trajets on b.idTrajet equals t.id
                           join a in db.Abonnements on t.id equals a.idTrajet
                           select new offre(){ date_arriver=t.date_arriver, date_depart=t.date_depart, arriver=t.arriver, depart=t.depart,
                               typeDeVehicule=b.typeDeVehicule,tarif=a.tarif});
            ViewBag.offres = await abonmnt.ToListAsync();
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
