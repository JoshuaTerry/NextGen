
function InitializeFileUploader(container, url, callback) {

    var filecounter = 0;
    var totalfiles = 0;

    $(container).find('.file_upload').fileupload({
        url: url,
        headers: GetApiHeaders(),
        sequentialUploads: true,
        dropZone: $(container).find('.fileuploadcontainer'),
        disableImageResize: true,
        done: function (e, data) {

            var responseFile = null;
            if (data._response.result.Data) {
                responseFile = data._response.result.Data;
            }

            $.each(data.files, function (index, file) {
                filecounter++;

                $(container).find('.filemessage').text('File upload in progress, please do not close this window...').show();
                $(container).find('.totalfiles').text(filecounter + ' upload completed').show();

                var progress = Math.floor(filecounter / totalfiles * 100);
                $(container).find('.progress-bar').css('width', progress + '%');

                if (totalfiles == filecounter) {
                    $(container).find('.filemessage').text('File upload complete.');
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

        $(container).find('.filemessage').text('');
        $(container).find('.totalfiles').text('');
    });

}
