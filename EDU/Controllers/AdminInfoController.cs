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
    public class AdminInfoController : Controller
    {
        private readonly IAdminInfoRepository _adminRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminInfoController(IAdminInfoRepository adminRepo, IWebHostEnvironment webHostEnvironment)
        {
            _adminRepo = adminRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<AdminInfo> objList = _adminRepo.GetAll(includeProperties: "Designation");

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

            AdminInfoVM adminInfoVM = new AdminInfoVM()
            {
                adminInfo = new AdminInfo(),
                DesignationSelectList = _adminRepo.GetAllDropDownList(WC.Designation),
              
            };

            if (id == null)
            {
                //this is for create
                return View(adminInfoVM);
            }
            else
            {
                adminInfoVM.adminInfo = _adminRepo.Find(id.GetValueOrDefault());
                if (adminInfoVM.adminInfo == null)
                {
                    return NotFound();
                }
                return View(adminInfoVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AdminInfoVM adminInfoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                var isUpdate = false;
                if (adminInfoVM.adminInfo.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePathAdmin;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    adminInfoVM.adminInfo.Image = fileName + extension;

                    _adminRepo.Add(adminInfoVM.adminInfo);

                }
                else
                {
                    //updating
                    var objFromDb = _adminRepo.FirstOrDefault(u => u.Id == adminInfoVM.adminInfo.Id,isTraking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePathAdmin;
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

                        adminInfoVM.adminInfo.Image = fileName + extension;
                    }
                    else
                    {
                        adminInfoVM.adminInfo.Image = objFromDb.Image;
                    }
                    _adminRepo.Update(adminInfoVM.adminInfo);
                    isUpdate = true;
                    
                }


                _adminRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Admin Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Admin Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            adminInfoVM.DesignationSelectList = _adminRepo.GetAllDropDownList(WC.Designation);
           
            return View(adminInfoVM);

        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            AdminInfo admininfo= _adminRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Designation");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (admininfo == null)
            {
                return NotFound();
            }

            return View(admininfo);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _adminRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePathAdmin;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _adminRepo.Remove(obj);
            _adminRepo.Save();
            TempData[WC.Success] = "Admin deleted Successfully";
                return RedirectToAction("Index");
        }

    }
}
