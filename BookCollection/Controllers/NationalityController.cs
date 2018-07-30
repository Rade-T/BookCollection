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
    public class NationalityController : Controller
    {
        private NationalityDAO dao = new NationalityDAO();
        
        public ActionResult Index()
        {
            var nationalitys = dao.Read();
            return View(nationalitys);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")]Nationality nationality)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Create(nationality);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(nationality);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = dao.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = dao.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        [HttpPost]
        public ActionResult Edit(Nationality nationality)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Update(nationality);
                    return RedirectToAction("Index");
                }
                return View(nationality);
            }
            catch
            {
                return View(nationality);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationality nationality = dao.Find(id);
            if (nationality == null)
            {
                return HttpNotFound();
            }
            return View(nationality);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Nationality nationality = dao.Find(id);
            dao.Delete(nationality);
            return RedirectToAction("Index");
        }
    }
}