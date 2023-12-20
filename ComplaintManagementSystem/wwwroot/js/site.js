$(document).ready(function () {
    AdminTable(); 
    UserTable();
    BindEvent();
    $('#exportToExcel').click(function () {
        ExportToExcel();
    });    
});  

function AdminTable() {
    $('#ComplaintTableContainer').jtable({
        title: 'All Complaints',
        paging: true,
        pageSize: 8,
        sorting: false,
        actions:
        {
            listAction: '/User/AdminDashboard'
        },
        fields: {
            ComplaintId: {
                title: "Complaint No",
                key: true,
                list: true,
            },
            ComplaintUserId: {
                title: 'UserId',
                width: '10%'
            },
            ComplaintTopic: {
                title: 'Topic',
                width: '20%'
            },
            ComplaintAddress: {
                title: 'Address',
                width: '20%'
            },
            ComplaintDesc: {
                title: 'Description',
                width: '20%'
            },
            ComplaintDate: {
                title: 'Date',
                width: '20%'
            },
            Actions: {
                title: 'Actions',
                width: '10%',
                display: function (data) {
                    return '<div style="display: flex;"><a href="#update-complaint" class="btn-sm btn-primary" style="text-decoration: none;" onclick="showUpdateModal(\'' + data.record.ComplaintId + '\', \'' + data.record.ComplaintUserId + '\', \'' + data.record.ComplaintTopic + '\', \'' + data.record.ComplaintAddress + '\', \'' + data.record.ComplaintDesc + '\', \'' + data.record.ComplaintDate + '\')">Edit</a><a href="#delete-complaint" class="btn-sm btn-danger" style="margin-left: 10px; text-decoration: none;" onclick="showDeleteModal(\'' + data.record.ComplaintId + '\', \'' + data.record.ComplaintUserId + '\', \'' + data.record.ComplaintTopic + '\', \'' + data.record.ComplaintAddress + '\', \'' + data.record.ComplaintDesc + '\', \'' + data.record.ComplaintDate + '\')">Delete</a></div>';
                },
            }, 
        }
    });

    $('#ComplaintTableContainer').jtable('load', SearchPara());
}

function UserTable() {
    $('#ComplaintUserTableContainer').jtable({
        title: 'My Complaints',
        paging: true,
        pageSize: 5,
        sorting: false,
        actions:
        {
            listAction: '/User/UserDashboard',
        },
        fields: {
            ComplaintId: {
                title: 'Complaint Id',
                width: '10%'
            },
            ComplaintTopic: {
                title: 'Topic',
                width: '20%'
            },
            ComplaintAddress: {
                title: 'Address',
                width: '20%'
            },
            ComplaintDesc: {
                title: 'Description',
                width: '20%'
            },
            ComplaintDate: {
                title: 'Date',
                width: '20%'
            },
            Actions: {
                title: 'Actions',
                width: '10%',
                display: function (data) {
                    return '<div style="display: flex;"><a href="#update-complaint" class="btn-sm btn-primary" style="text-decoration: none;" onclick="showUpdateModal(\'' + data.record.ComplaintId + '\', \'' + data.record.ComplaintUserId + '\', \'' + data.record.ComplaintTopic + '\', \'' + data.record.ComplaintAddress + '\', \'' + data.record.ComplaintDesc + '\', \'' + data.record.ComplaintDate + '\')">Edit</a><a href="#delete-complaint" class="btn-sm btn-danger" style="margin-left: 10px; text-decoration: none;" onclick="showDeleteModal(\'' + data.record.ComplaintId + '\', \'' + data.record.ComplaintUserId + '\', \'' + data.record.ComplaintTopic + '\', \'' + data.record.ComplaintAddress + '\', \'' + data.record.ComplaintDesc + '\', \'' + data.record.ComplaintDate + '\')">Delete</a></div>';
                },
            },
        }
    });

    $('#ComplaintUserTableContainer').jtable('load', SearchPara());
};

function showDeleteModal(complaintid, userid, topic, address, desc, date) {
    $("#showComplaintId").text("Complaint Id: " + complaintid);
    $("#showUserId").text("User Id: " + userid);
    $("#showTopic").text("Topic: " + topic);
    $("#showAddress").text("Address: " + address);
    $("#showDesc").text("Description: " + desc);
    $("#showDate").text("Date: " + date);
    document.getElementById('confirmDelete').onclick = function () {
        DeleteComplaint(complaintid);
    };
}

function showUpdateModal(complaintid, userid, topic, address, desc, date) {
    $('input[name="ComplaintIds"]').val(complaintid);
    $('input[name="ComplaintUserIds"]').val(userid);
    $('input[name="ComplaintDates"]').val(date.slice(0, 10));
    $('input[name="ComplaintTopics"]').val(topic);
    $('textarea[name="ComplaintAddresss"]').val(address);
    $('textarea[name="ComplaintDescs"]').val(desc);
    document.getElementById('confirmUpdate').onclick = function () {
        var data = $('#update-form').serializeArray();
        data.forEach((item) => {
            var str = item.name.slice(0, -1);
            item.name = str;
        })
        console.log(data);
        UpdateComplaint(data);
    };
}

function DeleteComplaint(id) {
    $.ajax({
        url: '/User/DeleteComplaint',
        type: 'POST',
        data: { ComplaintId: id },
        success: function () {
            $('#ComplaintTableContainer').jtable('reload');
            $('#ComplaintUserTableContainer').jtable('reload');
        },
        error: function (err) {
            console.log(err);
        }
    });
}


function UpdateComplaint(Data) {
    $.ajax({
        url: '/User/UpdateComplaint',
        type: 'POST',
        data: Data,
        success: function () {
            $('#ComplaintTableContainer').jtable('reload');
            $('#ComplaintUserTableContainer').jtable('reload');
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function ExportToExcel() {
    $.post('/User/ComplaintsExportReport', SearchPara(), function (response) {

        if (response.success === true) {
            location.href = '/Downloads/' + response.FileName;
            alert(response.Message)
        }
        else {
            alert(response.Message);
        }
    });
}  


function BindEvent() {

    $('#search-btn').click(function () {
        SearchGrid();
    });
}

function SearchGrid() {
    $('#ComplaintTableContainer').jtable('load', SearchPara());
    $('#ComplaintUserTableContainer').jtable('load', SearchPara());
}


function SearchPara() {
    var data = {
        SearchType: $('#searchtype').val(),
        SearchValue: $('#search-bar').val(),
    };

    return data;
}