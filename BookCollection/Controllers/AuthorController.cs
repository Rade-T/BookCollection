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
    public class AuthorController : Controller
    {
        private AuthorDAO dao = new AuthorDAO();
        private NationalityDAO nationalityDao = new NationalityDAO();
        
        public ActionResult Index()
        {
            var authors = dao.Read();
            return View(authors);
        }

        public ActionResult Create()
        {
            PopulateNationalitiesDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (author.NationalityID == -1)
                    {
                        var EmptyNationality = new Nationality();
                        EmptyNationality.Name = "";
                        EmptyNationality.Id = -1;
                        author.Nationality = EmptyNationality;
                    }
                    else
                    {
                        Nationality n = nationalityDao.Find(author.NationalityID);
                        author.Nationality = n;
                    }
                    dao.Create(author);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            PopulateNationalitiesDropDownList();
            return View(author);
        }

        private void PopulateNationalitiesDropDownList(object selectedNationality = null)
        {
            var nationalities = nationalityDao.Read();
            var EmptyNationality = new Nationality();
            EmptyNationality.Name = "";
            EmptyNationality.Id = -1;
            nationalities.Insert(0, EmptyNationality);
            ViewBag.NationalityID = new SelectList(nationalities, "Id", "Name", selectedNationality);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = dao.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = dao.Find(id);
            author.Nationality = nationalityDao.Find(author.NationalityID);
            if (author == null)
            {
                return HttpNotFound();
            }
            PopulateNationalitiesDropDownList(author.NationalityID);
            return View(author);
        }

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            author.Nationality = nationalityDao.Find(author.NationalityID);
            PopulateNationalitiesDropDownList();
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Update(author);
                    return RedirectToAction("Index");
                }
                return View(author);
            }
            catch
            {
                return View(author);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = dao.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Author author = dao.Find(id);
            dao.Delete(author);
            return RedirectToAction("Index");
        }
    }
}