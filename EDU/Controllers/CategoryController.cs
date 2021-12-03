using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EDU_DataAccess.Data;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using EDU_Utility;

namespace EDU.Controllers
{
    //Only loged in user can access this page 
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CatRepository;

        public CategoryController(ICategoryRepository CatRepository)
        {
            _CatRepository = CatRepository;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objList = _CatRepository.GetAll();
            return View(objList);
        }


        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _CatRepository.Add(obj);
                _CatRepository.Save();
                TempData[WC.Success] = "Category created successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an error";
            return View(obj);

        }


        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var obj = _CatRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _CatRepository.Update(obj);
                _CatRepository.Save();
                TempData[WC.Success] = "Category Updated successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an error";
            return View(obj);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _CatRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _CatRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
                _CatRepository.Remove(obj);
                _CatRepository.Save();
            TempData[WC.Success] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
