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
    public class Demande_TrajetController : Controller
    {
        private NavetteDB_Entities db = new NavetteDB_Entities();

        //Post DeleteD
        public async Task<ActionResult> DeleteD(int id)
        {
            int idC = int.Parse(Session["Usrid"].ToString());
            var demande = from d in db.Demande_Trajet where d.id == id && d.idUtilisateur == idC select d;
            db.Demande_Trajet.Remove(demande.FirstOrDefault());
            await db.SaveChangesAsync();
            return RedirectToAction("MesDemandes");
        }

        //Get:MesDemandes
        public async Task<ActionResult> MesDemandes()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                int idC = int.Parse(Session["Usrid"].ToString());
                var check = (from u in db.Utilisateurs where u.id == idC select u).FirstOrDefault();
                if (check.role_=="client")
                {
                    var demande_Trajet = (from d in db.Demande_Trajet where d.idUtilisateur == idC select d);
                    return View(await demande_Trajet.ToListAsync());
                }
                else
                {
                    return RedirectToAction("Login", "Utilisateurs");
                }

            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }
        //
        [HttpPost]
        public async Task<ActionResult> AjouterDemande(int id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                var query = (from d in db.Demande_Trajet where d.id == id select d).FirstOrDefault();
                var check = (from t in db.Trajets where t.depart==query.depart && t.arriver==query.arriver select t).FirstOrDefault();
                if(check==null)
                {
                    db.Trajets.Add(new Trajet { depart = query.depart, arriver = query.arriver, date_depart = query.date_depart, date_arriver = query.date_arriver });
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Demande_Trajet
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                var demande_Trajet = db.Demande_Trajet.Include(d => d.Client);
                return View(await demande_Trajet.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Demande_Trajet/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Demande_Trajet demande_Trajet = await db.Demande_Trajet.FindAsync(id);
                if (demande_Trajet == null)
                {
                    return HttpNotFound();
                }
                return View(demande_Trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Demande_Trajet/Create
        public ActionResult Create()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                ViewBag.idUtilisateur = new SelectList(db.Clients, "id", "id");
                List<SelectListItem> villes = new List<SelectListItem>();
                foreach (var v in db.Villes)
                {
                    villes.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
                }
                ViewBag.depart = villes;
                ViewBag.arriver = villes;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Demande_Trajet/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,depart,arriver,date_depart,date_arriver,idUtilisateur")] Demande_Trajet demande_Trajet)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Demande_Trajet.Add(demande_Trajet);
                    await db.SaveChangesAsync();
                    return RedirectToAction("MesDemandes");
                }

                ViewBag.idUtilisateur = new SelectList(db.Clients, "id", "id", demande_Trajet.idUtilisateur);
                return View(demande_Trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Demande_Trajet/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Demande_Trajet demande_Trajet = await db.Demande_Trajet.FindAsync(id);
                if (demande_Trajet == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idUtilisateur = new SelectList(db.Clients, "id", "id", demande_Trajet.idUtilisateur);
                return View(demande_Trajet);
            }                
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Demande_Trajet/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,depart,arriver,date_depart,date_arriver,idUtilisateur")] Demande_Trajet demande_Trajet)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(demande_Trajet).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("MesDemandes");
                }
                ViewBag.idUtilisateur = new SelectList(db.Clients, "id", "id", demande_Trajet.idUtilisateur);
                return View(demande_Trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Demande_Trajet/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Demande_Trajet demande_Trajet = await db.Demande_Trajet.FindAsync(id);
                if (demande_Trajet == null)
                {
                    return HttpNotFound();
                }
                return View(demande_Trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Demande_Trajet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Demande_Trajet demande_Trajet = await db.Demande_Trajet.FindAsync(id);
            db.Demande_Trajet.Remove(demande_Trajet);
            await db.SaveChangesAsync();
            return RedirectToAction("MesDemandes");
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
