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
    public class BatchinfoController : Controller
    {
        private readonly IBatchinfoRepository _BatchinfoRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BatchinfoController(IBatchinfoRepository BatchinfoRepo, IWebHostEnvironment webHostEnvironment)
        {
            _BatchinfoRepo = BatchinfoRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
           

            IEnumerable<InfoBatch> objList = _BatchinfoRepo.GetAll(includeProperties:"DepartmentInfo,ShiftInfo");
            

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

            
            
            BatchinfoVM batchVM = new BatchinfoVM()
            {
                InfoBatch = new InfoBatch(),
                DepartmentSelectList = _BatchinfoRepo.GetAllDropDownList(WC.DepartmentName),
                ShiftInfoSelectList = _BatchinfoRepo.GetAllDropDownList(WC.ShiftInfo),

            };

            if (id == null)
            {
                //this is for create
                return View(batchVM);
            }
            else
            {

                batchVM.InfoBatch = _BatchinfoRepo.Find(id.GetValueOrDefault());
                if (batchVM.InfoBatch == null)
                {
                    return NotFound();
                }
                return View(batchVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BatchinfoVM batchVm)
        {
            if (ModelState.IsValid)
            {
                
                var isUpdate = false;
                if (batchVm.InfoBatch.Id == 0)
                {
                    //Creating
                    ApplicationUser user = new ApplicationUser();
                    batchVm.InfoBatch.UserId = user.Id;
                    _BatchinfoRepo.Add(batchVm.InfoBatch);
                }
                else
                {
                    //updating
                    var objFromDb = _BatchinfoRepo.FirstOrDefault(u => u.Id == batchVm.InfoBatch.Id,isTraking: false);
                    ApplicationUser user = new ApplicationUser();
                    batchVm.InfoBatch.UserId = user.Id;
                    _BatchinfoRepo.Update(batchVm.InfoBatch);
                    isUpdate = true;
                }
                _BatchinfoRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Batchinfo Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Batchinfo Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            batchVm.DepartmentSelectList = _BatchinfoRepo.GetAllDropDownList(WC.DepartmentName);
            
            return View(batchVm);
        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            InfoBatch InfoBatch = _BatchinfoRepo.FirstOrDefault(u => u.Id == id, includeProperties: "DepartmentInfo,ShiftInfo");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (InfoBatch == null)
            {
                return NotFound();
            }

            return View(InfoBatch);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _BatchinfoRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }


            _BatchinfoRepo.Remove(obj);
            _BatchinfoRepo.Save();
            TempData[WC.Success] = "Batchinfo deleted Successfully";
                return RedirectToAction("Index");
            

        }

    }
}
