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
    public class SocietesController : Controller
    {
        private NavetteDB_Entities db = new NavetteDB_Entities();
        //

        public ActionResult dashboard()
        {
            ViewBag.UsrSession = Session["UsrSession"];
            if (Session["UsrSession"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        //
        // GET: Societes
        public async Task<ActionResult> Index()
        {
            if (Session["UsrSession"] != null)
            {
                var societes = db.Societes.Include(s => s.Utilisateur);
                return View(await societes.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Societes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Societe societe = await db.Societes.FindAsync(id);
                if (societe == null)
                {
                    return HttpNotFound();
                }
                return View(societe);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // GET: Societes/Create
        public ActionResult Create()
        {
            if (Session["UsrSession"] != null)
            {
                ViewBag.id = new SelectList(db.Utilisateurs, "id", "nom");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Societes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,adresse,raisonSocial")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Societes.Add(societe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.Utilisateurs, "id", "nom", societe.id);
            return View(societe);
        }

        // GET: Societes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["UsrSession"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Societe societe = await db.Societes.FindAsync(id);
                if (societe == null)
                {
                    return HttpNotFound();
                }
                ViewBag.id = new SelectList(db.Utilisateurs, "id", "nom", societe.id);
                return View(societe);
            }
            else
            {
                return RedirectToAction("Login", "Utilisateurs");
            }
        }

        // POST: Societes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,adresse,raisonSocial")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(societe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.Utilisateurs, "id", "nom", societe.id);
            return View(societe);
        }

        // GET: Societes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = await db.Societes.FindAsync(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: Societes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Societe societe = await db.Societes.FindAsync(id);
            db.Societes.Remove(societe);
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
