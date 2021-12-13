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
    public class OnlineClassController : Controller
    {
        private readonly IOnlineClassInfoRepository _OnlineClassRepo;
        private readonly IOnlineClassDetailsRepository _OnlineClassDetailsRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ISemesterInfoRepository _SemesterRepo;
        private readonly ICourseInfoRepository _CourseInfoRepo;
        private readonly IDepartmentInfoRepository _DepartmentInfoRepo;
        public OnlineClassController(
            IOnlineClassInfoRepository OnlineClassRepo, IOnlineClassDetailsRepository OnlineClassDetailsRepo, ISemesterInfoRepository SemesterRepo, ICourseInfoRepository CourseInfoRepo, IDepartmentInfoRepository DepartmentInfoRepo, IStudentRepository StudentRepo)
        {
            _OnlineClassRepo = OnlineClassRepo;
            _OnlineClassDetailsRepo = OnlineClassDetailsRepo;
            _SemesterRepo = SemesterRepo;
            _CourseInfoRepo = CourseInfoRepo;
            _DepartmentInfoRepo = DepartmentInfoRepo;
            _studentRepo = StudentRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<OnlineClassInfo> objList = _OnlineClassRepo.GetAll(includeProperties: "courseInfo,semesterInfo,departmentInfo,teacherInfo");
            return View(objList);
        }

        public IActionResult Create(string searchDepartment)
        {
            List<StudentInfoCheckList> StudentChecklist = new List<StudentInfoCheckList>();
            var student = _studentRepo.GetAll(includeProperties: "ShiftInfo,DepartmentInfo,InfoBatch");
            foreach (var item in student)
            {
                StudentChecklist.Add(new StudentInfoCheckList
                {
                    Id = item.Id,
                    StudentId = item.StudentId,
                    StudentName = item.StudentName,
                    FathersName = item.FathersName,
                    MothersName = item.MothersName,
                    MobileNo = item.MobileNo,
                    Email= item.Email,
                    Address= item.Address,
                    Date=item.Date,
                    ShiftId=item.ShiftId,
                    DepartmentId=item.DepartmentId,
                    BatchId=item.BatchId,
                    Image=item.Image,
                    Description=item.Description,
                });
            }
            OnlineClassInfoVM OnlineClassVM = new OnlineClassInfoVM()
            {
                OnlineClassInfo = new OnlineClassInfo(),
                SemesterSelectList = _OnlineClassRepo.GetAllDropDownList(WC.SemesterId),
                CourseSelectList = _OnlineClassRepo.GetAllDropDownList(WC.CourseId),
                DepartmentSelectList = _OnlineClassRepo.GetAllDropDownList(WC.DepartmentName),
                TeacherSelectList = _OnlineClassRepo.GetAllDropDownList(WC.TeacherId),
                StudentList = StudentChecklist
            };
            OnlineClassVM.StudentList = StudentChecklist;
           
            /*if (!String.IsNullOrEmpty(searchDepartment))
            {
                OnlineClassVM.StudentList = OnlineClassVM.StudentList.Where(u => u.DepartmentName.ToLower().Contains(searchDepartment.ToLower()));
            }*/
            return View(OnlineClassVM);
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

        /* [HttpPost]
         public IActionResult SaveEnrollment(EnrollmentVM enrollment)
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
         }*/

    }
}
