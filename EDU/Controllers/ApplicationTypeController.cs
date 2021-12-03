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
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _applicationTypeRepo;

        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepo)
        {
            _applicationTypeRepo = applicationTypeRepo;
        }


        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _applicationTypeRepo.GetAll();
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
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _applicationTypeRepo.Add(obj);
                _applicationTypeRepo.Save();

                TempData[WC.Success] = "Application Creaded Successfully";
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
            var obj = _applicationTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _applicationTypeRepo.Update(obj);
                _applicationTypeRepo.Save();
                TempData[WC.Success] = "Application Updated Successfully";
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
            var obj = _applicationTypeRepo.Find(id.GetValueOrDefault());
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
            var obj = _applicationTypeRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _applicationTypeRepo.Remove(obj);
            _applicationTypeRepo.Save();
            TempData[WC.Success] = "Application Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
