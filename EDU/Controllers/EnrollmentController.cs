using Braintree;
using Microsoft.AspNetCore.Mvc;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using EDU_Models.ViewModels;
using EDU_Utility;
using EDU_Utility.BrainTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentInfoRepository _EnrollmentRepo;
        private readonly IEnrollmentDetailsRepository _EnrollmentDetailsRepo;
        private readonly IStudentRepository _StudentRepo;
        private readonly ISemesterInfoRepository _SemesterRepo;
        private readonly ICourseInfoRepository _CourseInfoRepo;

        [BindProperty]
        public OrderVM orderVM { get; set; }
      
        public EnrollmentController(
            IEnrollmentInfoRepository EnrollmentRepo, IEnrollmentDetailsRepository EnrollementDetailRepo, IStudentRepository StudentRepo, ISemesterInfoRepository SemesterRepo, ICourseInfoRepository CourseInfoRepo)
        {
            _EnrollmentRepo = EnrollmentRepo;
            _EnrollmentDetailsRepo = EnrollementDetailRepo;
            _StudentRepo = StudentRepo;
            _SemesterRepo = SemesterRepo;
            _CourseInfoRepo = CourseInfoRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            List<CourseInfoCheckList> coursechecklist = new List<CourseInfoCheckList>();
            var course = _CourseInfoRepo.GetAll(includeProperties: "DepartmentInfo");
            foreach (var item in course)
            {
                coursechecklist.Add(new CourseInfoCheckList
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    DepartmentId = item.DepartmentId,
                    DepartmentName = item.DepartmentName,
                });
            }
            EnrollmentVM enrollmentVM = new EnrollmentVM()
            {
                enrollmentInfo = new EnrollmentInfo(),
                StudentSelectList = _EnrollmentRepo.GetAllDropDownList(WC.StudentId),
                CourseList = coursechecklist,
                /*DepartmentSelectList = _EnrollmentRepo.GetAllDropDownList(WC.DepartmentName),*/
                /*CourseList = _CourseInfoRepo.GetAll(includeProperties: "DepartmentInfo"),*/


            };
            /*if (!string.IsNullOrEmpty(studentId))
            {
                orderlistVM.OrderHeaderList = orderlistVM.OrderHeaderList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderlistVM.OrderHeaderList = orderlistVM.OrderHeaderList.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderlistVM.OrderHeaderList = orderlistVM.OrderHeaderList.Where(u => u.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(Status) && Status != "---Status List---")

            {
                orderlistVM.OrderHeaderList = orderlistVM.OrderHeaderList.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }*/
            return View(enrollmentVM);
        }
        /* public IActionResult Search(EnrollmentVM enrollmentVM)
         {
             var obj = enrollmentVM;
             return View(obj);
         }*/
        /*
                public IActionResult Details(int id)
                {
                    orderVM = new OrderVM()
                    {
                        orderHeader = _OrderHeaderRepo.FirstOrDefault(u => u.Id == id),
                        orderDetails = _OrderDetailRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product")
                    };
                    return View(orderVM);
                }
                [HttpPost]
                public IActionResult StartProcessing()
                {
                    OrderHeader orderHeader = _OrderHeaderRepo.FirstOrDefault(u => u.Id == orderVM.orderHeader.Id);
                    orderHeader.OrderStatus = WC.StatusInProcess;
                    _OrderHeaderRepo.Save();
                    TempData[WC.Success] = "Order Processed Successfully";
                    return RedirectToAction(nameof(Index));
                }
                [HttpPost]
                public IActionResult ShipOrder()
                {
                    OrderHeader orderHeader = _OrderHeaderRepo.FirstOrDefault(u => u.Id == orderVM.orderHeader.Id);
                    orderHeader.OrderStatus = WC.StatusShipped;
                    orderHeader.ShippingDate = DateTime.Now;
                    _OrderHeaderRepo.Save();
                    TempData[WC.Success] = "Order shipted Successfully";
                    return RedirectToAction(nameof(Index));
                }

                [HttpPost]
                public IActionResult CancelOrder()
                {
                    OrderHeader orderHeader = _OrderHeaderRepo.FirstOrDefault(u => u.Id == orderVM.orderHeader.Id);

                    var geteway = _brainTreeGate.GetGeteWay();
                    Transaction transaction = geteway.Transaction.Find(orderHeader.TransactionId);
                    if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
                    {
                        //no refund
                        Result<Transaction> resultVoid = geteway.Transaction.Void(orderHeader.TransactionId);
                    }
                    else
                    {
                        Result<Transaction> resultRefund = geteway.Transaction.Refund(orderHeader.TransactionId);
                        //refund
                    }
                    orderHeader.OrderStatus = WC.StatusRefunded;
                    _OrderHeaderRepo.Save();
                    TempData[WC.Success] = "Order Canceled Successfully";
                    return RedirectToAction(nameof(Index));
                }

                [HttpPost]
                public IActionResult UpdateOrderDetails()
                {
                    OrderHeader orderHeaderfromDB = _OrderHeaderRepo.FirstOrDefault(u => u.Id == orderVM.orderHeader.Id);
                    orderHeaderfromDB.FullName = orderVM.orderHeader.FullName;
                    orderHeaderfromDB.PhoneNumber = orderVM.orderHeader.PhoneNumber;
                    orderHeaderfromDB.StreetAddress = orderVM.orderHeader.StreetAddress;
                    orderHeaderfromDB.City = orderVM.orderHeader.City;
                    orderHeaderfromDB.State = orderVM.orderHeader.State;
                    orderHeaderfromDB.PostalCode = orderVM.orderHeader.PostalCode;
                    orderHeaderfromDB.Email = orderVM.orderHeader.Email;
                    _OrderHeaderRepo.Save();
                    TempData[WC.Success] = "Order Details Updated Successfully";
                    return RedirectToAction("Datails", "Order", new { id = orderHeaderfromDB.Id });
                }*/
        #region API CALLS
        [HttpGet]
        public IActionResult GetEnrollmentList()
        {
            return Json(new { data = _EnrollmentRepo.GetAll() });
        }
        #endregion
    }
}
