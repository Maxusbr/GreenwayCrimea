function validateFloat(source, argument) {
    if (argument.Value != "" && parseFloat(argument.Value)) {
        argument.IsValid = true;
    } else {
        argument.IsValid = false;
    }
}