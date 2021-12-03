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
    public class TeacherInfoController : Controller
    {
        private readonly ITeacherInfoRepository _teacherRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeacherInfoController(ITeacherInfoRepository teacherRepo, IWebHostEnvironment webHostEnvironment)
        {
            _teacherRepo = teacherRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<TeacherInfo> objList = _teacherRepo.GetAll(includeProperties: "Designation,UniversityName");

            //foreach(var obj in objList)
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

            TeacherInfoVM teacherInfoVM = new TeacherInfoVM()
            {
                TeacherInfo = new TeacherInfo(),
                DesignationSelectList = _teacherRepo.GetAllDropDownList(WC.Designation),
                UniversitySelectList = _teacherRepo.GetAllDropDownList(WC.University),

            };

            if (id == null)
            {
                //this is for create
                return View(teacherInfoVM);
            }
            else
            {
                teacherInfoVM.TeacherInfo = _teacherRepo.Find(id.GetValueOrDefault());
                if (teacherInfoVM.TeacherInfo == null)
                {
                    return NotFound();
                }
                return View(teacherInfoVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TeacherInfoVM teacherInfoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                var isUpdate = false;
                if (teacherInfoVM.TeacherInfo.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePathTeacher;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    teacherInfoVM.TeacherInfo.Image = fileName + extension;

                    _teacherRepo.Add(teacherInfoVM.TeacherInfo);

                }
                else
                {
                    //updating
                    var objFromDb = _teacherRepo.FirstOrDefault(u => u.Id == teacherInfoVM.TeacherInfo.Id,isTraking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePathTeacher;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        teacherInfoVM.TeacherInfo.Image = fileName + extension;
                    }
                    else
                    {
                        teacherInfoVM.TeacherInfo.Image = objFromDb.Image;
                    }
                    _teacherRepo.Update(teacherInfoVM.TeacherInfo);
                    isUpdate = true;
                    
                }


                _teacherRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Teacher Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Teacher Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            teacherInfoVM.DesignationSelectList = _teacherRepo.GetAllDropDownList(WC.Designation);
            teacherInfoVM.DesignationSelectList = _teacherRepo.GetAllDropDownList(WC.University);

            return View(teacherInfoVM);

        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            TeacherInfo teacherinfo= _teacherRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Designation,UniversityName");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (teacherinfo == null)
            {
                return NotFound();
            }

            return View(teacherinfo);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _teacherRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePathTeacher;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _teacherRepo.Remove(obj);
            _teacherRepo.Save();
            TempData[WC.Success] = "Teacher deleted Successfully";
                return RedirectToAction("Index");
        }

    }
}
