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
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(IStudentRepository studentRepo, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepo = studentRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<StudentInfo> objList = _studentRepo.GetAll(includeProperties: "ShiftInfo,DepartmentInfo,InfoBatch");

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

            StudentInfoVM studentInfoVM = new StudentInfoVM()
            {
                StudentInfo = new StudentInfo(),
                DepartmentSelectList = _studentRepo.GetAllDropDownList(WC.DepartmentName),
                ShiftSelectList = _studentRepo.GetAllDropDownList(WC.ShiftInfo),
                BatchSelectList = _studentRepo.GetAllDropDownList(WC.batchinfo)
            };

            if (id == null)
            {
                //this is for create
                return View(studentInfoVM);
            }
            else
            {
                studentInfoVM.StudentInfo = _studentRepo.Find(id.GetValueOrDefault());
                if (studentInfoVM.StudentInfo == null)
                {
                    return NotFound();
                }
                return View(studentInfoVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StudentInfoVM studentInfoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                var isUpdate = false;
                if (studentInfoVM.StudentInfo.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePathStudents;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    studentInfoVM.StudentInfo.Image = fileName + extension;

                    _studentRepo.Add(studentInfoVM.StudentInfo);

                }
                else
                {
                    //updating
                    var objFromDb = _studentRepo.FirstOrDefault(u => u.Id == studentInfoVM.StudentInfo.Id,isTraking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePathStudents;
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

                        studentInfoVM.StudentInfo.Image = fileName + extension;
                    }
                    else
                    {
                        studentInfoVM.StudentInfo.Image = objFromDb.Image;
                    }
                    _studentRepo.Update(studentInfoVM.StudentInfo);
                    isUpdate = true;
                    
                }


                _studentRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Product Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Product Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            studentInfoVM.DepartmentSelectList = _studentRepo.GetAllDropDownList(WC.DepartmentName);
            studentInfoVM.ShiftSelectList = _studentRepo.GetAllDropDownList(WC.ShiftInfo);
            studentInfoVM.BatchSelectList = _studentRepo.GetAllDropDownList(WC.batchinfo);
            return View(studentInfoVM);

        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            StudentInfo studentinfo= _studentRepo.FirstOrDefault(u => u.Id == id, includeProperties: "ShiftInfo,DepartmentInfo,InfoBatch");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (studentinfo == null)
            {
                return NotFound();
            }

            return View(studentinfo);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _studentRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePathStudents;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _studentRepo.Remove(obj);
            _studentRepo.Save();
            TempData[WC.Success] = "Product deleted Successfully";
                return RedirectToAction("Index");
            

        }

    }
}
