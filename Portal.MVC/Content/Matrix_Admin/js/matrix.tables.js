
$(document).ready(function(){
	
    var table = $('.data-table').dataTable({
		"bJQueryUI": true,
		"sPaginationType": "full_numbers",
		"sDom": '<""l>t<"F"fp>'
    });

	
    $('#button').click(function () {
        table.row('.selected').remove();
    });
    $('.data-table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
	
	$('input[type=checkbox],input[type=radio],input[type=file]').uniform();

	$('select').select2();

    
});
