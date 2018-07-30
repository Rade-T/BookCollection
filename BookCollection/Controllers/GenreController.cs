using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookCollection.DAO;
using BookCollection.Models;
using System.Net;
using System.Data;

namespace BookCollection.Controllers
{
    public class GenreController : Controller
    {
        private GenreDAO dao = new GenreDAO();
        
        public ActionResult Index()
        {
            var genres = dao.Read();
            return View(genres);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")]Genre genre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Create(genre);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(genre);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = dao.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = dao.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Update(genre);
                    return RedirectToAction("Index");
                }
                return View(genre);
            }
            catch
            {
                return View(genre);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = dao.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Genre genre = dao.Find(id);
            dao.Delete(genre);
            return RedirectToAction("Index");
        }
    }
}