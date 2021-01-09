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
    public class BusesController : Controller
    {
        private NavetteDB_Entities db = new NavetteDB_Entities();

        // GET: Buses
        public async Task<ActionResult> Index()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                var buses = db.Buses.Include(b => b.Societe).Include(b => b.Trajet);
                return View(await buses.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Buses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bus bus = await db.Buses.FindAsync(id);
                if (bus == null)
                {
                    return HttpNotFound();
                }
                return View(bus);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Buses/Create
        public ActionResult Create()
        {
            if (Session["UsrSession"] != null)
            {
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                ViewBag.UsrSession = Session["UsrSession"];
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

        // POST: Buses/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,typeDeVehicule,description_,idSociete,idTrajet")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Buses.Add(bus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idSociete = new SelectList(db.Societes, "id", "adresse", bus.idSociete);
            ViewBag.idTrajet = new SelectList(db.Trajets, "id", "depart", bus.idTrajet);
            return View(bus);
        }

        // GET: Buses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bus bus = await db.Buses.FindAsync(id);
                if (bus == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial", bus.idSociete);
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                var query = (from t in db.Trajets where t.id == bus.idTrajet select t).FirstOrDefault();
                trajets.Find(t => t.Text == query.depart + " => " + query.arriver).Selected = true;
                ViewBag.idTrajet = trajets;
                return View(bus);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Buses/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,typeDeVehicule,description_,idSociete,idTrajet")] Bus bus)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bus).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                //ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial", bus.idSociete);
                string help = "";
                List<SelectListItem> trajets = new List<SelectListItem>();
                //ViewBag.idSociete = new SelectList(db.Societes, "id", "RaisonSocial");
                foreach (var t in db.Trajets)
                {
                    help = t.depart + " => " + t.arriver;
                    trajets.Add(new SelectListItem { Text = help, Value = t.id.ToString() });
                }
                ViewBag.idTrajet = trajets;
                return View(bus);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Buses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bus bus = await db.Buses.FindAsync(id);
                if (bus == null)
                {
                    return HttpNotFound();
                }
                return View(bus);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Bus bus = await db.Buses.FindAsync(id);
            db.Buses.Remove(bus);
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
