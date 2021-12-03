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

    [Authorize(Roles = WC.AdminRole)]
    public class DepartmentInfoController : Controller
    {
        private readonly IDepartmentInfoRepository _departmentInfoRepo;

        public DepartmentInfoController(IDepartmentInfoRepository departmentInfoRepo)
        {
            _departmentInfoRepo = departmentInfoRepo;
        }


        public IActionResult Index()
        {
            IEnumerable<DepartmentInfo> objList = _departmentInfoRepo.GetAll();
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
        public IActionResult Create(DepartmentInfo obj)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                obj.UserId = user.Id;
                /*obj.UserId = 1;*/
                _departmentInfoRepo.Add(obj);
                _departmentInfoRepo.Save();

                TempData[WC.Success] = "Department Created Successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Thare was and Error";
            return View(obj);
        }


        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _departmentInfoRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentInfo obj)
        {
            if (ModelState.IsValid)
            {
                _departmentInfoRepo.Update(obj);
                _departmentInfoRepo.Save();
                TempData[WC.Success] = "Department Updated Successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            return View(obj);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _departmentInfoRepo.Find(id.GetValueOrDefault());
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
            var obj = _departmentInfoRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _departmentInfoRepo.Remove(obj);
            _departmentInfoRepo.Save();
            TempData[WC.Success] = "Department Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
