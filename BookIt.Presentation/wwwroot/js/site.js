// Delete confirmation
function confirmDelete(formId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById(formId).submit();
        }
    });
}

// Show success/error messages
$(document).ready(function() {
    if (typeof TempData !== 'undefined') {
        if (TempData['Success']) {
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: TempData['Success']
            });
        }
        if (TempData['Error']) {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: TempData['Error']
            });
        }
    }
}); 