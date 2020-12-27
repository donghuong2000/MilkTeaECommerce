$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "/Home/getallcategories",
        
        success: function (data) {
            $('#agileinfo-nav_search').empty();
            $('#ul-categories').empty();
            $('#agileinfo-nav_search').append(' <option value="" disabled selected>All Categories</option>');
            $.each(data, function (index, item) {
                
                $('#agileinfo-nav_search').append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        }
    });
    
    

});