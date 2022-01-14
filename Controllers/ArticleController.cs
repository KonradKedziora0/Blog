using GitBlog.Data;
using GitBlog.Models;
using GitBlog.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(WC.AdminRole) != true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IEnumerable<Article> objList = _db.Articles.Include(u => u.Category);
                return View(objList);
            }
        }
        //Get upsert
        public IActionResult Upsert(int? id)
        {
            ArticleVM articleVM = new ArticleVM()
            {
                Article = new Article(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null)
            {
                //this is for create
                return View(articleVM);
            }
            else
            {
                articleVM.Article = _db.Articles.Find(id);
                if (articleVM.Article == null)
                {
                    return NotFound();
                }
                return View(articleVM);
            }
        }

        //Post upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ArticleVM articleVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (articleVM.Article.Id == 0)
                {
                    //Create
                    string upload = webRootPath + WC.ArticlesImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                        using (var filestram = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestram);
                        }

                        articleVM.Article.Image = filename + extension;

                        _db.Articles.Add(articleVM.Article);
                   
                }
                else
                {
                    //Update
                    var ObjFromDb = _db.Articles.AsNoTracking().FirstOrDefault(u => u.Id == articleVM.Article.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ArticlesImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, ObjFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var filestram = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filestram);
                        }

                        articleVM.Article.Image = filename + extension;
                    }
                    else
                    {
                        articleVM.Article.Image = ObjFromDb.Image;
                    }
                    _db.Articles.Update(articleVM.Article);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            articleVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(articleVM);
        }

        //Get delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Article article = _db.Articles.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        //Post delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Articles.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ArticlesImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Articles.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
