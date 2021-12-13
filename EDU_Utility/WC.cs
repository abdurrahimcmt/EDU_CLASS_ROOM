using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EDU_Utility
{
    public static class WC
    {
        public const string ImagePath = @"\images\product\";
        public const string ImagePathStudents = @"\images\Students\";
        public const string ImagePathAdmin = @"\images\Admin\";
        public const string ImagePathTeacher = @"\images\Teacher\";
        public const string ImagePathAnnouncement = @"\images\Announcement\";
        public const string SessionCart = "ShoppingCartSession";
        public const string SessionInquiryId = "InquirySession";

        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";
        public const string StudentRole = "Student";
        public const string TeacherRole = "Teacher";

        public const string ShiftInfo = "Shift";
        public const string batchinfo = "Batch";
        public const string ShiftEvening = "Evening";
        public const string ShiftDay = "Day";

        public const string EmailAdmin = "abdurrahimrrs@abdurrahimrrs.com";

        public const string CategoryName = "Category";

        public const string DepartmentName = "DepartmentName";
        public const string StudentId = "StudentId";
        public const string SemesterId = "SemesterId";
        public const string CourseId = "CourseId";

        public const string TeacherId = "TeacherId";

        public const string Designation = "Designation";

        public const string University = "University";

        public const string ApplicationTypeName = "ApplicationType";
        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(
            new List<string> {
                StatusPending,StatusApproved,StatusInProcess,StatusShipped,StatusCancelled,StatusRefunded
            });

    }
}
