
function InitializeFileUploader(url, callback) {

    var filecounter = 0;
    var totalfiles = 0;

    $('#file_upload').fileupload({
        url: url,
        headers: GetApiHeaders(),
        sequentialUploads: true,
        dropZone: $('.fileuploadcontainer'),
        disableImageResize: true,
        done: function (e, data) {

            var responseFile = null;
            if (data._response.result.Data) {
                responseFile = data._response.result.Data;
            }

            $.each(data.files, function (index, file) {
                filecounter++;

                $('.filemessage').text('File upload in progress, please do not close this window...').show();
                $('.totalfiles').text(filecounter + ' upload completed').show();

                var progress = Math.floor(filecounter / totalfiles * 100);
                $('.progress-bar').css('width', progress + '%');

                if (totalfiles == filecounter) {
                    $('.filemessage').text('File upload complete.');
                    filecounter = 0;
                    totalfiles = 0;
                }

                if (callback) {
                    callback(responseFile);
                }
            });
        }
    }).bind('fileuploadadd', function (e, data) {
        totalfiles++;

        $('.filemessage').text('');
        $('.totalfiles').text('');
    });

}
