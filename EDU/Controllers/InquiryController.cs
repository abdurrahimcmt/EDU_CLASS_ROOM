using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using EDU_Models.ViewModels;
using EDU_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RahimShop.Controllers
{
    [Authorize(Roles=WC.AdminRole)] 
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _InquiryHeaderRepo;
        private readonly IInquiryDetailRepository _InquiryDetailRepo;

        [BindProperty]
        public InquiryVM inquiryVM { get; set; }
        public InquiryController(IInquiryHeaderRepository InquiryHeaderRepo, IInquiryDetailRepository InquiryDetailRepo)
        {
            _InquiryHeaderRepo = InquiryHeaderRepo;
            _InquiryDetailRepo = InquiryDetailRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            inquiryVM = new InquiryVM()
            {
                inquiryHeader = _InquiryHeaderRepo.FirstOrDefault(u => u.Id == id),
                inquiryDetails = _InquiryDetailRepo.GetAll(u => u.InquiryHeader.Id == id,includeProperties: "Product")
            };
            return View(inquiryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            //Search Product from inquiryDetails table according to inpuiryHeaderIn from inquiryHeader table
            inquiryVM.inquiryDetails = _InquiryDetailRepo.GetAll(u => u.InquiryHeaderId == inquiryVM.inquiryHeader.Id);

            //Add productId in ShoppingCart
            foreach (var detail in inquiryVM.inquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId= detail.ProductId
                };
                
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart,shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId,inquiryVM.inquiryHeader.Id);
            TempData[WC.Success] = "Converted Successfully";
            return RedirectToAction("Index","Cart");
        }
        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _InquiryHeaderRepo.FirstOrDefault(u=> u.Id==inquiryVM.inquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetail = _InquiryDetailRepo.GetAll(u=> u.InquiryHeaderId == inquiryVM.inquiryHeader.Id);

            _InquiryDetailRepo.RemoveRange(inquiryDetail);
            _InquiryHeaderRepo.Remove(inquiryHeader);
            _InquiryHeaderRepo.Save();
            TempData[WC.Error] = "Inquiry Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data= _InquiryHeaderRepo.GetAll() });
        }
        #endregion
    }
}
