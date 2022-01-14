using GitBlog.Data;
using GitBlog.Models;
using GitBlog.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Controllers
{
    public class CategoryController : Controller
    {

        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(WC.AdminRole) != true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IEnumerable<Category> objList = _db.Category;
                return View(objList);
            }
           
        }


        //Get upsert
        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new CategoryVM()
            {
                Category = new Category(),
            };

            if (id == null)
            {
                //this is for create
                return View(categoryVM);
            }
            else
            {
                categoryVM.Category = _db.Category.Find(id);
                if (categoryVM.Category == null)
                {
                    return NotFound();
                }
                return View(categoryVM);
            }
        }

        //Post upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (categoryVM.Category.Id == 0)
                {
                    //Create
                    _db.Category.Add(categoryVM.Category);
                }
                else
                {
                    //Update
                    var ObjFromDb = _db.Category.AsNoTracking().FirstOrDefault(u => u.Id == categoryVM.Category.Id);
                    _db.Category.Update(categoryVM.Category);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryVM);
        }
        
        //Get delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = _db.Category.FirstOrDefault(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Post delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
