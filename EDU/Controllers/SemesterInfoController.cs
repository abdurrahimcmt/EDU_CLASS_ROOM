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
    public class SemesterInfoController : Controller
    {
        private readonly ISemesterInfoRepository _semesterInfo;

        public SemesterInfoController(ISemesterInfoRepository semesterInfo)
        {
            _semesterInfo = semesterInfo;
        }


        public IActionResult Index()
        {
            IEnumerable<SemesterInfo> objList = _semesterInfo.GetAll();
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
        public IActionResult Create(SemesterInfo obj)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                obj.UserId = user.Id;
                _semesterInfo.Add(obj);
                _semesterInfo.Save();

                TempData[WC.Success] = "SemesterInfo Creaded Successfully";
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
            var obj = _semesterInfo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SemesterInfo obj)
        {
            if (ModelState.IsValid)
            {
                _semesterInfo.Update(obj);
                _semesterInfo.Save();
                TempData[WC.Success] = "Semester Updated Successfully";
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
            var obj = _semesterInfo.Find(id.GetValueOrDefault());
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
            var obj = _semesterInfo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _semesterInfo.Remove(obj);
            _semesterInfo.Save();
            TempData[WC.Success] = "Semester Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
