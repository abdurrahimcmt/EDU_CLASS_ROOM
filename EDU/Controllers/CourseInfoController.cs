using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDU_DataAccess.Data;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using EDU_Models.ViewModels;
using EDU_Utility;

namespace EDU.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CourseInfoController : Controller
    {
        private readonly ICourseInfoRepository _CourseInfoRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CourseInfoController(ICourseInfoRepository CourseInfoRepo, IWebHostEnvironment webHostEnvironment)
        {
            _CourseInfoRepo = CourseInfoRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
           

            IEnumerable<CourseInfo> objList = _CourseInfoRepo.GetAll(includeProperties:"DepartmentInfo");
            

            //foreach(var obj in objList
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
            //};

            return View(objList);
        }


        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {

            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            ////ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();

            
            
            CourseInfoVM courseVM = new CourseInfoVM()
            {
                CourseInfo = new CourseInfo(),
                DepartmentSelectList = _CourseInfoRepo.GetAllDropDownList(WC.DepartmentName)

            };

            if (id == null)
            {
                //this is for create
                return View(courseVM);
            }
            else
            {

                courseVM.CourseInfo = _CourseInfoRepo.Find(id.GetValueOrDefault());
                if (courseVM.CourseInfo == null)
                {
                    return NotFound();
                }
                return View(courseVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CourseInfoVM courseVM)
        {
            if (ModelState.IsValid)
            {
                
                var isUpdate = false;
                if (courseVM.CourseInfo.Id == 0)
                {
                    //Creating
                    ApplicationUser user = new ApplicationUser();
                    courseVM.CourseInfo.UserId = user.Id;
                    _CourseInfoRepo.Add(courseVM.CourseInfo);
                }
                else
                {
                    //updating
                    var objFromDb = _CourseInfoRepo.FirstOrDefault(u => u.Id == courseVM.CourseInfo.Id,isTraking: false);
                    ApplicationUser user = new ApplicationUser();
                    courseVM.CourseInfo.UserId = user.Id;
                    _CourseInfoRepo.Update(courseVM.CourseInfo);
                    isUpdate = true;
                }
                _CourseInfoRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Couse Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Couse Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            courseVM.DepartmentSelectList = _CourseInfoRepo.GetAllDropDownList(WC.DepartmentName);
            
            return View(courseVM);
        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            CourseInfo courseInfo = _CourseInfoRepo.FirstOrDefault(u => u.Id == id, includeProperties: "DepartmentInfo");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (courseInfo == null)
            {
                return NotFound();
            }

            return View(courseInfo);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _CourseInfoRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }


            _CourseInfoRepo.Remove(obj);
            _CourseInfoRepo.Save();
            TempData[WC.Success] = "Course deleted Successfully";
                return RedirectToAction("Index");
            

        }

    }
}
