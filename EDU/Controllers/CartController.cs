using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using EDU_DataAccess.Data;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using EDU_Models.ViewModels;
using EDU_Utility;
using EDU_Utility.BrainTree;

namespace EDU.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IApplicationUserRepository _AppUserRepo;
        private readonly IProductRepository _ProductRepo;
        private readonly IInquiryHeaderRepository _InquiryHeaderRepo;
        private readonly IInquiryDetailRepository _InquiryDetailRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        private readonly IOrderHeaderRepository _IOrderHeaderRepo;
        private readonly IOrderDetailRepository _IOrderDetailRepo;
        private readonly IBrainTreeGate _brainTreeGate;


        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
            IApplicationUserRepository AppUserRepo, IProductRepository ProductRepo,
            IInquiryHeaderRepository InquiryHeaderRepo, IInquiryDetailRepository InquiryDetailRepo,
            IOrderHeaderRepository OrderHeaderRepo, IOrderDetailRepository OrderDetailRepo, IBrainTreeGate brainTreeGate)
        {
            _AppUserRepo = AppUserRepo;
            _ProductRepo = ProductRepo;
            _InquiryHeaderRepo = InquiryHeaderRepo;
            _InquiryDetailRepo = InquiryDetailRepo;
            _AppUserRepo = AppUserRepo;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _IOrderHeaderRepo = OrderHeaderRepo;
            _IOrderDetailRepo = OrderDetailRepo;
            _brainTreeGate = brainTreeGate;
        }
        public IActionResult Index()
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            /// collect all productId from ShoppingCart
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            /// searching product Data from Database which productId contains in shopping cart 
            IEnumerable<Product> prodListTemp = _ProductRepo.GetAll(u => prodInCart.Contains(u.Id));
            // create a list for send to index view
            IList<Product> prodList = new List<Product>();
            foreach (var cartObj in shoppingCartList)
            {
                // adding sqFt to all productData 
                Product productTemp = prodListTemp.FirstOrDefault(u => u.Id == cartObj.ProductId);
                productTemp.TempSqrt = cartObj.SqFt;
                prodList.Add(productTemp);
            }


            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> prodList)
        {
            List<ShoppingCart> shoppintCartList = new List<ShoppingCart>();
            foreach (var prod in prodList)
            {
                shoppintCartList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    SqFt = prod.TempSqrt
                });

            }
            HttpContext.Session.Set(WC.SessionCart, shoppintCartList);
            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            ApplicationUser applicationUser;
            if (User.IsInRole(WC.AdminRole))
            {
                if (HttpContext.Session.Get<int>(WC.SessionInquiryId) != 0)
                {
                    InquiryHeader inquiryHeader = _InquiryHeaderRepo.FirstOrDefault(u => u.Id == HttpContext.Session.Get<int>(WC.SessionInquiryId));
                    // if come from inquiry table 
                    //Email, FullName,PhoneNumber has been loaded from Inquiry  
                    applicationUser = new ApplicationUser()
                    {
                        Email = inquiryHeader.Email,
                        FullName = inquiryHeader.FullName,
                        PhoneNumber = inquiryHeader.PhoneNumber
                    };

                }
                else
                {
                    // AdminUser see email,fullname,phonenumber field is blank 
                    applicationUser = new ApplicationUser();
                }
                var gateway = _brainTreeGate.GetGeteWay();
                var clientToken = gateway.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }
            else
            {
                // Customer see his Email,FullName,PhoneNumber
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                applicationUser = _AppUserRepo.FirstOrDefault(u => u.Id == claim.Value);
                //var userId = User.FindFirstValue(ClaimTypes.Name);
            }


            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _ProductRepo.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = applicationUser,
                //ProductList = prodList.ToList()
            };
            // Add sqFt to all product which belongs in shopping cart
            foreach (var cartobj in shoppingCartList)
            {
                Product productlist = _ProductRepo.FirstOrDefault(u => u.Id == cartobj.ProductId);
                productlist.TempSqrt = cartobj.SqFt;
                ProductUserVM.ProductList.Add(productlist);
            }
            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(IFormCollection collection, ProductUserVM ProductUserVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(WC.AdminRole))
            {
                // Submit Order
                /* var orderTotal = 0.0;
                 foreach (var prod in ProductUserVM.ProductList)
                 {
                     orderTotal += prod.Price * prod.TempSqrt;
                 }*/

                OrderHeader orderHeader = new OrderHeader()
                {
                    CreatedByUserId = claim.Value,
                    FinalOrderTotal = ProductUserVM.ProductList.Sum(x => x.Price * x.TempSqrt),
                    City = ProductUserVM.ApplicationUser.City,
                    StreetAddress = ProductUserVM.ApplicationUser.StreetAddress,
                    State = ProductUserVM.ApplicationUser.State,
                    PostalCode = ProductUserVM.ApplicationUser.PostalCode,
                    FullName = ProductUserVM.ApplicationUser.FullName,
                    Email = ProductUserVM.ApplicationUser.Email,
                    PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                    OrderDate = DateTime.Now,
                    OrderStatus = WC.StatusPending
                };
                _IOrderHeaderRepo.Add(orderHeader);
                _InquiryHeaderRepo.Save();

                foreach (var prod in ProductUserVM.ProductList)
                {
                    OrderDetails orderDetail = new OrderDetails()
                    {
                        OrderHeaderId = orderHeader.Id,
                        PricePerSqFt = prod.Price,
                        Sqft = prod.TempSqrt,
                        ProductId = prod.Id
                    };
                    _IOrderDetailRepo.Add(orderDetail);

                }

                _IOrderDetailRepo.Save();
                TempData[WC.Success] = "Your Inquiry has been successfully included";

                string nonceFromTheClient = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(orderHeader.FinalOrderTotal),
                    PaymentMethodNonce = nonceFromTheClient,
                    OrderId = orderHeader.Id.ToString(),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                var gateway = _brainTreeGate.GetGeteWay();
                Result<Transaction> result = gateway.Transaction.Sale(request);

                if (result.Target.ProcessorResponseText == "Approved")
                {
                    orderHeader.TransactionId = result.Target.Id;
                    orderHeader.OrderStatus = WC.StatusApproved;

                }
                else
                {
                    orderHeader.OrderStatus = WC.StatusCancelled;
                }
                _IOrderHeaderRepo.Save();
                return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
            }
            else
            {
                // Submit Inquiry 
                var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() +
                "Inquiry.html";

                var subject = "New Inquiry";
                string HtmlBody = "";
                using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
                {
                    HtmlBody = sr.ReadToEnd();
                }
                //Name: { 0}
                //Email: { 1}
                //Phone: { 2}
                //Products: {3}

                StringBuilder productListSB = new StringBuilder();
                foreach (var prod in ProductUserVM.ProductList)
                {
                    productListSB.Append($" - Name: { prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
                }

                string messageBody = string.Format(HtmlBody,
                    ProductUserVM.ApplicationUser.FullName,
                    ProductUserVM.ApplicationUser.Email,
                    ProductUserVM.ApplicationUser.PhoneNumber,
                    productListSB.ToString());
                await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

                /// Save into Database
                InquiryHeader inquiryHeader = new InquiryHeader()
                {
                    ApplicationUserId = claim.Value,
                    FullName = ProductUserVM.ApplicationUser.FullName,
                    Email = ProductUserVM.ApplicationUser.Email,
                    PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                    InquiryDate = DateTime.Now
                };
                _InquiryHeaderRepo.Add(inquiryHeader);
                _InquiryHeaderRepo.Save();

                foreach (var prod in ProductUserVM.ProductList)
                {
                    InquiryDetail inquiryDetail = new InquiryDetail()
                    {
                        InquiryHeaderId = inquiryHeader.Id,
                        ProductId = prod.Id
                    };
                    _InquiryDetailRepo.Add(inquiryDetail);

                }
                TempData[WC.Success] = "Your Inquiry has been successfully included";
                _InquiryDetailRepo.Save();
            }
            return RedirectToAction(nameof(InquiryConfirmation));
        }
        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = _IOrderHeaderRepo.FirstOrDefault(u => u.Id == id);
            HttpContext.Session.Clear();
            return View(orderHeader);
        }

        public IActionResult Remove(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            TempData[WC.Success] = "Your product has been successfully removed from the cart";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> prodList)
        {
            List<ShoppingCart> shoppintCartList = new List<ShoppingCart>();
            foreach (var prod in prodList)
            {
                shoppintCartList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    SqFt = prod.TempSqrt
                });

            }
            HttpContext.Session.Set(WC.SessionCart, shoppintCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            TempData[WC.Success] = "Cart Cleared successfully";
            return RedirectToAction("Index", "Home");
        }

    }
}

