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
using Microsoft.AspNetCore.Http;

namespace EDU.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentInfoRepository _EnrollmentRepo;
        private readonly IEnrollmentDetailsRepository _EnrollmentDetailsRepo;
        private readonly IStudentRepository _StudentRepo;
        private readonly ISemesterInfoRepository _SemesterRepo;
        private readonly ICourseInfoRepository _CourseInfoRepo;
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
            IEnumerable<EnrollmentInfo> objList = _EnrollmentRepo.GetAll(includeProperties: "studentInfo,semesterInfo,departmentInfo");
            return View(objList);
        }
       
        public IActionResult Create(string searchDepartment,string StudentId, string SemesterId, string DepartmentId)
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
                SemesterSelectList = _EnrollmentRepo.GetAllDropDownList(WC.SemesterId),
                DepartmentSelectList = _EnrollmentRepo.GetAllDropDownList(WC.DepartmentName),
                CourseList= coursechecklist
            };
            /*enrollmentVM.CourseList = coursechecklist;*/
            ViewData["SearchString"] = searchDepartment;
            ViewData["StudentId"] = StudentId;
            ViewData["SemesterId"] = SemesterId;
            ViewData["DepartmentId"] = DepartmentId;
            if (!String.IsNullOrEmpty(searchDepartment))
            {
                enrollmentVM.CourseList = enrollmentVM.CourseList.Where(u => u.DepartmentName.ToLower().Contains(searchDepartment.ToLower()));
            }
            return View(enrollmentVM);
        }

        /*[HttpGet]
        public IActionResult Create(EnrollmentVM enrollmentVM, string searchDepartment)
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
            enrollmentVM.enrollmentInfo = new EnrollmentInfo();
            enrollmentVM.StudentSelectList = _EnrollmentRepo.GetAllDropDownList(WC.StudentId);
            enrollmentVM.SemesterSelectList = _EnrollmentRepo.GetAllDropDownList(WC.SemesterId);
            enrollmentVM.DepartmentSelectList = _EnrollmentRepo.GetAllDropDownList(WC.DepartmentName);
            enrollmentVM.CourseList = coursechecklist;
            ViewData["SearchString"] = searchDepartment;
            if (!String.IsNullOrEmpty(searchDepartment))
            {
                enrollmentVM.CourseList = enrollmentVM.CourseList.Where(u => u.DepartmentName.ToLower().Contains(searchDepartment.ToLower()));
            }
            return View(enrollmentVM);
        }*/

        /*public IActionResult listCourse(EnrollmentVM enrollmentVM)
        {
            
        }*/
        /*[HttpGet]
        public async Task<IActionResult> listCourse(EnrollmentVM enrollmentVM,string searchDepartment)
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
            enrollmentVM.CourseList = coursechecklist;
            ViewData["SearchString"] = searchDepartment;
            if (!String.IsNullOrEmpty(searchDepartment))
            {
                enrollmentVM.CourseList = enrollmentVM.CourseList.Where(u => u.DepartmentName.ToLower().Contains(searchDepartment.ToLower()));
            }
            return View(enrollmentVM);
        }*/

        /*[HttpPost]
        [ValidateAntiForgeryToken]*/

        /*  [HttpPost]
          [ValidateAntiForgeryToken]
          [ActionName("SaveEnrollment")]*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEnrollment(IFormCollection collection, EnrollmentVM enrollment)
        {
            EnrollmentVM obj = new EnrollmentVM();
            obj = enrollment;

            EnrollmentInfo enrollmentinfo = new EnrollmentInfo()
            {
                StudentId= obj.enrollmentInfo.StudentId,
                StudentRoll= obj.enrollmentInfo.studentInfo.StudentId,
                StudentName= obj.enrollmentInfo.studentInfo.StudentName,

                SemesterId= obj.enrollmentInfo.SemesterId,
                SemesterName= obj.enrollmentInfo.semesterInfo.Name,
                DepartmentId = obj.enrollmentInfo.DepartmentId,
                DepartmentName= obj.enrollmentInfo.departmentInfo.Name
                
            };
            _EnrollmentRepo.Add(enrollmentinfo);
            _EnrollmentRepo.Save();

            foreach (var course in obj.CourseList)
            {
                if (course.takeCourses.Selected)
                {
                    EnrollmentDetails enrollmentDetail = new EnrollmentDetails()
                    {
                        EnrollmentId = enrollmentinfo.Id,
                        CourseId = course.Id,
                        Coursecode=course.Code,
                        CourseName=course.Name,
                       
                    };
                    _EnrollmentDetailsRepo.Add(enrollmentDetail);
                }
            }
            _EnrollmentDetailsRepo.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
