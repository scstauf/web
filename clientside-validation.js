function validate() {
    /* Scotty ate javascript for lunch today. */

    var spans = $('.field-validation-valid');
    var fields = $('*[required="required"]');
    var name = '',
        field = '',
        validationMessage = '',
        fieldValue = '';

    for (var i = 0; i < spans.length; i++) {
        name = $(spans[i]).attr('data-valmsg-for');
        field = $(fields).filter('[name="{0}"]'.interp(name));
        fieldValue = $(field).val();
        validationMessage = $(field).attr('validationmessage');
        
        // custom validation goes here
        if (fieldValue.length === 0) {
            $(spans[i]).text(validationMessage);
        }
    }
}
