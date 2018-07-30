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
    public class LanguageController : Controller
    {
        private LanguageDAO dao = new LanguageDAO();
        
        public ActionResult Index()
        {
            var languages = dao.Read();
            return View(languages);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")]Language language)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Create(language);
                    return RedirectToAction("Index");
                }
            }
            catch
            { 
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(language);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = dao.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = dao.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Update(language);
                    return RedirectToAction("Index");
                }
                return View(language);
            }
            catch
            {
                return View(language);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = dao.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Language language = dao.Find(id);
            dao.Delete(language);
            return RedirectToAction("Index");
        }
    }
}