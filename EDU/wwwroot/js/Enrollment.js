var dataTable;

$(document).ready(function () {
    loadDataTable("GetCourseList");
});

function loadDataTable(url) {
    dataTable = $('#tblEnrollmentData').DataTable({
        "ajax": {
            "url": "/Enrollment/" + url
        },
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                }
            }
        ],
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "Code", "width": "15%" },
            { "data": "Name", "width": "15%" },
            { "data": "DepartmentId", "width": "15%" },
            /*{
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Inquiry/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "5%"
            }*/
        ]
    });
}
