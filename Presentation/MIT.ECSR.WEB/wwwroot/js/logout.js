$("#logout").on('click', function () {
    ConfirmMessage('Apakah Anda Yakin Akan Log Out?', isConfirm => {
        if (isConfirm) {
            openMenu('/Account/Logoff');
        }
    });
});
