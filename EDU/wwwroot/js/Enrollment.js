var dataTable;

$(document).ready(function () {
    loadDataTable("GetEnrollmentList");
});

function loadDataTable(url) {
    dataTable = $('#tblEnrollmentData').DataTable({
        "ajax": {
            "url": "/Enrollment/" + url
        },
        'order': [[1, 'asc']],
        "columns": [
            { "data": "", "width": "10%" },
            { "data": "code", "width": "30%" },
            { "data": "name", "width": "30%" },
            { "data": "departmentName", "width": "30%" }
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




