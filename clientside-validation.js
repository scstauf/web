/*
 
 minimalist client-side form validation

 mark fields for validation by giving them a v-msg attribute,
 use a span to display the message on failure.

 specify validation constraints using attributes like:

    <input type="text" name="zipCode" v-msg="Please enter a valid zip code." minlength="5" isnumeric="true" />
    <span v-target="zipCode" v-label></span>

    <input type="text" name="emailAddr" v-msg="Please enter a valid email address." isemailaddress="true" />
    <span v-target="emailAddr" v-label></span>

    <input type="text" name="phoneNum" v-msg="Please enter a valid phone number." isphonenumber="true" />
    <span v-target="phoneNum" v-label></span>

 */
 
var VForms = {
    validate: function () {
        var spans = $('*[v-label]'),
            fields = $('*[v-msg]'),
            name = '',
            field = '',
            validationMessage = '',
            fieldValue = '',
            scrollTo = null,
            passed = true;
            
        var minLength = 0,
            isEmailAddress = false,
            isPhoneNumber = false,
            isNumeric = false,
            isInvalid = false;

        for (var i = 0; i < spans.length; i++) {
            name = $(spans[i]).attr('v-target');
            field = $(fields).filter('[name="'+ name + '"]');
            fieldValue = $(field).val();
            validationMessage = $(field).attr('v-msg');

            minLength = $(field).attr('minlength');
            isEmailAddress = $(field).attr('isemailaddress');
            isPhoneNumber = $(field).attr('isphonenumber');
            isNumeric = $(field).attr('isnumeric');
            isInvalid = false;

            if (fieldValue.trim() === '') {
                isInvalid = true;
            }
        
            if (!isInvalid && minLength !== undefined) {
                isInvalid = (fieldValue.length < minLength);
            }
            
            if (!isInvalid && isEmailAddress !== undefined) {
                isInvalid = !VForms.isValidEmailAddress(fieldValue);
            }
            
            if (!isInvalid && isPhoneNumber !== undefined) {
                isInvalid = !VForms.isValidPhoneNumber(fieldValue);
            }
            
            if (!isInvalid && isNumeric !== undefined) {
                isInvalid = isNaN(fieldValue);
            }

            if (isInvalid) {
                $(spans[i]).text(validationMessage);
                passed = false;

                if (!scrollTo) {
                    scrollTo = field;
                }
            }
            else {
                $(spans[i]).text('');
            }
        }

        return passed;
    },
    // probably should use regex in the following functions
    isValidEmailAddress: function (emailAddress) {
        if (emailAddress.trim().length === 0) {
            return false;
        }

        var atSymbols = 0, periods = 0, i = 0, len = emailAddress.length;

        if (emailAddress[len - 1] === '.') {
            return false;
        }

        for (i = 0; i < len; i++) {
            switch (emailAddress[i]) {
                case '@':
                    if (atSymbols > 1) {
                        return false;
                    }

                    atSymbols++;
                    break;

                case '.':
                    if (atSymbols === 0) {
                        return false;
                    }

                    periods++;
                    break;
            }   
        }

        return atSymbols === 1 && periods > 0;
    },
    isValidPhoneNumber: function (phoneNumber) {
        var digits = 0;

        for (var i = 0; i < phoneNumber.length; i++) {
            if (!isNaN(phoneNumber[i]) && phoneNumber[i] !== ' ') {
                digits++;
            }
        }

        return digits >= 10;
    }
};

$(document).ready(function () {
    $('form').submit(function (e) {
        if (!VForms.validate()) {
            e.preventDefault();
        }
    });
});
