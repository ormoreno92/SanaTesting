$(function () {
    $("form").submit(function (e) {
        e.preventDefault();
        var form = this.elements;
        if (isNullOrEmpty(form.title.value) || isNullOrEmpty(form.number.value) || isNullOrEmpty(form.price.value)) {
            alert("fill all the (*) fields, are mandatory!.");
            return;
        }
        var product = {
            productQuantity: form.number.value,
            Title: form.title.value,
            Price: form.price.value
        };
        createProduct(product, this);
    });
});

var createProduct = function (postData, form) {
    $.ajax({
        url: form.attributes.action.nodeValue
        , type: 'POST'
        , contentType: 'application/json'
        , data: JSON.stringify(postData)
        , success: function (data) {
            if (data) {
                alert("Product added successfully!.");
                window.location = '/'
            }
            else
                alert("error saving the product, please try again.");
        }
        , error: function (xhr) {
            debugger;
            alert("error saving the product, please try again.");
        }
    });
}

var changeStorage = function (type) {
    $.get("/Products/GetProductsStorage", function (data) {
        //eraseAllStorages();
        $.ajax({
            url: '/Products/ChangeStorage'
            , type: 'POST'
            , contentType: 'application/json'
            , data: JSON.stringify({ prods: data, type: type })
            , success: function (Pdata) {
            }
            , error: function (xhr) {
                debugger;
                alert("error chaging the storage of the data.");
            }
        });
    });
}

var setLocalStorage = function (data) {
    storage.setItem("productslocalstorage", JSON.stringify(data));
}

var setServerSession = function (data) {

}

var setServerMemory = function (data) {

}

var setCustomFile = function (type, data) {

}

var eraseAllStorages = function () {
    $.post("/Products/CleanStorage", function (data) { });
}

var getProducts = function () {

}