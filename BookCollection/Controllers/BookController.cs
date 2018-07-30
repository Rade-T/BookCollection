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
    public class BookController : Controller
    {
        private BookDAO dao = new BookDAO();
        private LanguageDAO languageDAO = new LanguageDAO();
        private GenreDAO genreDAO = new GenreDAO();
        private BookAuthorsDAO bookAuthorsDAO = new BookAuthorsDAO();
        private AuthorDAO authorDAO = new AuthorDAO();

        private void PopulateLanguagesDropDownList(object selectedLanguage = null)
        {
            var languages = languageDAO.Read();
            ViewBag.LanguageID = new SelectList(languages, "Id", "Name", selectedLanguage);
        }

        private void PopulateGenresDropDownList(object selectedGenre = null)
        {
            var genres = genreDAO.Read();
            var EmptyGenre = new Genre();
            EmptyGenre.Name = "";
            EmptyGenre.Id = -1;
            genres.Insert(0, EmptyGenre);
            ViewBag.GenreID = new SelectList(genres, "Id", "Name", selectedGenre);
        }

        public ActionResult Index(string searchString, string bookGenre, string bookLanguage)
        {
            var books = dao.Read();
            var genres = genreDAO.Read();
            var languages = languageDAO.Read();
            ViewBag.bookGenre = new SelectList(genres, "Name", "Name");
            ViewBag.bookLanguage= new SelectList(languages, "Name", "Name");

            if (!String.IsNullOrEmpty(searchString))
            {
                books = dao.Search(searchString);
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                var filteredBooks = new List<Book>();
                foreach (var book in books)
                {
                    if (book.Genre.Name == bookGenre)
                    {
                        filteredBooks.Add(book);
                    }
                }
                books = filteredBooks;
            }

            if (!string.IsNullOrEmpty(bookLanguage))
            {
                var filteredBooks = new List<Book>();
                foreach (var book in books)
                {
                    if (book.Language.Name == bookLanguage)
                    {
                        filteredBooks.Add(book);
                    }
                }
                books = filteredBooks;
            }

            return View(books);
        }

        public ActionResult Create()
        {
            PopulateLanguagesDropDownList();
            PopulateGenresDropDownList();
            PopulateBookAuthors(new Book());
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book, string[] selectedAuthors)
        {
            PopulateLanguagesDropDownList();
            PopulateGenresDropDownList();
            PopulateBookAuthors(book);
            try
            {
                if (ModelState.IsValid)
                {
                    Language l = languageDAO.Find(book.LanguageID);
                    Genre g;
                    if (book.GenreID == -1)
                    {
                        g = new Genre();
                        g.Id = -1;
                        g.Name = "";
                    }
                    else
                    {
                        g = genreDAO.Find(book.GenreID);
                    }
                    book.Id = dao.CurrentID() + 1;
                    book.Language = l;
                    book.Genre = g;
                    UpdateBookAuthors(selectedAuthors, book);
                    if (book.Authors.Count == 0)
                    {
                        ModelState.AddModelError("", "At least one author should be selected");
                        return View(book);
                    }
                    dao.Create(book);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }

            return View(book);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = dao.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = dao.Find(id);
            book.Language = languageDAO.Find(book.LanguageID);
            book.Genre = genreDAO.Find(book.GenreID);
            if (book == null)
            {
                return HttpNotFound();
            }
            PopulateLanguagesDropDownList(book.LanguageID);
            PopulateGenresDropDownList(book.GenreID);
            PopulateBookAuthors(book);
            return View(book);
        }

        private void PopulateBookAuthors(Book book)
        {
            var Authors = authorDAO.Read();
            List<BookAuthors> bookAuthorsList = bookAuthorsDAO.Find(book.Id);
            var bookAuthors = new HashSet<int>();
            foreach (var bookAuthor in bookAuthorsList)
            {
                bookAuthors.Add(bookAuthor.AuthorID);
            }
            var viewModel = new List<BookAuthors>();
            foreach (var author in Authors)
            {
                viewModel.Add(new BookAuthors
                {
                    AuthorID = author.Id,
                    Name = author.Name,
                    Assigned = bookAuthors.Contains(author.Id)
                });
            }
            ViewBag.BookAuthors = viewModel;
        }

        [HttpPost]
        public ActionResult Edit(Book book, string[] selectedAuthors)
        {
            book.Language = languageDAO.Find(book.LanguageID);
            book.Genre = genreDAO.Find(book.GenreID);
            book.Authors = bookAuthorsDAO.FindAuthorsForId(book.Id);
            PopulateGenresDropDownList();
            PopulateLanguagesDropDownList();
            PopulateBookAuthors(book);
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateBookAuthors(selectedAuthors, book);
                    if (book.Authors.Count == 0)
                    {
                        ModelState.AddModelError("", "At least one author should be selected");
                        return View(book);
                    }
                    dao.Update(book);
                    return RedirectToAction("Index");
                }
                return View(book);
            }
            catch
            {
                return View(book);
            }
        }

        private void UpdateBookAuthors(string[] selectedAuthors, Book bookToUpdate)
        {
            if (selectedAuthors == null)
            {
                bookToUpdate.Authors = new List<Author>();
                return;
            }

            var selectedAuthorsHS = new HashSet<string>(selectedAuthors);
            var bookAuthors = new HashSet<int>();

            foreach (var author in bookToUpdate.Authors)
            {
                bookAuthors.Add(author.Id);
            }

            List<Author> Authors = authorDAO.Read();
            foreach (var author in Authors)
            {
                if (selectedAuthorsHS.Contains(author.Id.ToString()))
                {
                    if (!bookAuthors.Contains(author.Id))
                    {
                        bookToUpdate.Authors.Add(author);
                    }
                }
                else
                {
                    if (bookAuthors.Contains(author.Id))
                    {
                        bookToUpdate.Authors.Remove(author);
                    }
                }
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = dao.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Book book = dao.Find(id);
            bookAuthorsDAO.Delete(book);
            dao.Delete(book);
            return RedirectToAction("Index");
        }
    }
}