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
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementRepository _announcementRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AnnouncementController(IAnnouncementRepository announcementRepo, IWebHostEnvironment webHostEnvironment)
        {
            _announcementRepo = announcementRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Announcement> objList = _announcementRepo.GetAll();


            return View(objList);
        }


        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            var obj = new Announcement();
            if (id == null)
            {
                //this is for create
                
                return View(obj); 
            }
            else
            {
                obj = _announcementRepo.Find(id.GetValueOrDefault());
                if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                var isUpdate = false;
                if (announcement.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePathAnnouncement;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    announcement.filePath = fileName + extension;

                    _announcementRepo.Add(announcement);

                }
                else
                {
                    //updating
                    var objFromDb = _announcementRepo.FirstOrDefault(u => u.Id == announcement.Id,isTraking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePathAnnouncement;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.filePath);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        announcement.filePath = fileName + extension;
                    }
                    else
                    {
                        announcement.filePath = objFromDb.filePath;
                    }
                    _announcementRepo.Update(announcement);
                    isUpdate = true;
                    
                }


                _announcementRepo.Save();
                if (!isUpdate)
                {
                    TempData[WC.Success] = "Announcement Created Successfully";
                }
                else
                {
                    TempData[WC.Success] = "Announcement Updated Successfully";
                }
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "There was an Error";
            

            return View(announcement);

        }


   
        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Announcement announcement= _announcementRepo.FirstOrDefault(u => u.Id == id);
            //product.Category = _db.Category.Find(product.CategoryId);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _announcementRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePathAnnouncement;
            var oldFile = Path.Combine(upload, obj.filePath);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _announcementRepo.Remove(obj);
            _announcementRepo.Save();
            TempData[WC.Success] = "Announcement deleted Successfully";
                return RedirectToAction("Index");
        }

    }
}
