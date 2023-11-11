﻿var table;
var datatable;
var updatedRow;
var exportedCols = [];

function showSuccessMessage(message = 'Saved Successfully!') {
    Swal.fire({
        icon: 'success',
        title: 'Good JOB',
        text: message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function showErrorMessage(message = 'Something went wrong!') {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message.responseText != undefined ? message.responseText : message,
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}

function disableSubmitButton() {
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}

function onModalBegin() {
    disableSubmitButton();
}


function onModalSuccess(row) { 
    showSuccessMessage();
    $('#Modal').modal('hide');
    if (updatedRow !== undefined) {
        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }
    var newRow = $(row);
    datatable.row.add(newRow).draw();

    KTMenu.init();
    KTMenu.initGlobalHandlers();
}

function onModalComplete() {
    $('body :submit').removeAttr('disabled').removeAttr('data-kt-indicator');
}

//Select2
function applaySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        $('form').not('#SignOut').validate().element('#' + $(this).attr('id'));
    });
}

//DataTables
var headers = $('th');
$.each(headers, function (i) {
    if (!$(this).hasClass('js-no-export')) {
        exportedCols.push(i);
    }
});
 // Class definition
var KTDatatables = function () {
    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
            'drawCallback': function () {
                KTMenu.createInstances();
            }
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('document-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

$(document).ready(function () {

    //Disable Submit Button
    $('form').not('#SignOut').on('submit', function () {
        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this)
                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val.content();
            })
        }

        var isValid = $(this).valid();
        if (isValid) disableSubmitButton();
        
    })
    //Tinymce
    if ($('.js-tinymce').length > 0) {
        var options = {
            selector: ".js-tinymce",
            height: "434",
        };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }

        tinymce.init(options);
    }

    //Datepicker
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops: 'up',
        maxDate: new Date(),
    });

    //Select2
    applaySelect2();

    //SweetAlert
    var message = $('#Message').text();
    if (message !== '') {
        showSuccessMessage();
    }

    //DataTables
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });

    //Handle Bootstrap Modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');
        modal.find('#ModalLabel').text(btn.data('title'));
        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }
        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
                applaySelect2();
            },
            error: function () {
                showErrorMessage();
            }
        });
        modal.modal('show');
    });
    //Handle ToggelStatus
    $('body').delegate('.js-toggle-status', 'click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: 'Are you sure that you need to toggle this item status?',
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-light-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (lastUpdatedOn) {
                            var row = btn.parents('tr')
                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                            row.find('.js-updated-on').html(lastUpdatedOn);
                            showSuccessMessage();
                            row.addClass('animate__animated animate__flash animate__delay-2s');
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });
    });

    //Handle Confirm
    $('body').delegate('.js-confirm', 'click', function () {
        var btn = $(this);

        bootbox.confirm({
            message: btn.data('message'),
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-light-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function () {
                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });
    });

    // Handle Sign Out
    $('.js-signout').on('click', function () {
        $('#SignOut').submit();
    });
});