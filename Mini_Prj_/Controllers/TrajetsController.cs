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
    public class TrajetsController : Controller
    {
        private NavetteDB_Entities db = new NavetteDB_Entities();

        // GET: Trajets
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                return View(await db.Trajets.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Trajets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Trajet trajet = await db.Trajets.FindAsync(id);
                if (trajet == null)
                {
                    return HttpNotFound();
                }
                return View(trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Trajets/Create
        public ActionResult Create()
        {
            if (Session["UsrSession"] != null)
            {
                ViewBag.UsrSession = Session["UsrSession"];
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

        // POST: Trajets/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,depart,arriver,date_depart,date_arriver")] Trajet trajet)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            List<SelectListItem> villes = new List<SelectListItem>();
            foreach (var v in db.Villes)
            {
                villes.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
            }
            ViewBag.depart = villes;
            ViewBag.arriver = villes;
            if (ModelState.IsValid)
            {
                db.Trajets.Add(trajet);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trajet);
        }

        // GET: Trajets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                List<SelectListItem> villes = new List<SelectListItem>();
                List<SelectListItem> villes2 = new List<SelectListItem>();
                foreach (var v in db.Villes)
                {
                    villes.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
                    villes2.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
                }
                var query = (from t in db.Trajets where t.id == id select t).FirstOrDefault() ;
                villes.Find(v => v.Text == query.depart).Selected = true;
                villes2.Find(v => v.Text == query.arriver).Selected = true;
                ViewBag.depart = villes;
                ViewBag.arriver = villes2;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Trajet trajet = await db.Trajets.FindAsync(id);
                if (trajet == null)
                {
                    return HttpNotFound();
                }
                return View(trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Trajets/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,depart,arriver,date_depart,date_arriver")] Trajet trajet)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            List<SelectListItem> villes = new List<SelectListItem>();
            foreach (var v in db.Villes)
            {
                villes.Add(new SelectListItem { Text = v.ville1, Value = v.ville1 });
            }
            ViewBag.depart = villes;
            ViewBag.arriver = villes;
            if (ModelState.IsValid)
            {
                db.Entry(trajet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trajet);
        }

        // GET: Trajets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Trajet trajet = await db.Trajets.FindAsync(id);
                if (trajet == null)
                {
                    return HttpNotFound();
                }
                return View(trajet);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Trajets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Trajet trajet = await db.Trajets.FindAsync(id);
            db.Trajets.Remove(trajet);
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
