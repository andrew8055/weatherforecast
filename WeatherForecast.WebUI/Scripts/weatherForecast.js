$(document).ready(function() {
    var cities = GetCities();

    cities.forEach(function (city) {
        var liCount = $('#cityList li').length;

        $('#cityList').append('<li class="list-group-item text-center" childNumber="' + liCount + '"><a href="#">' + city + '</a><a href="#" class="glyphicon glyphicon-remove" style="padding-top:3px;float:right;text-decoration:none;" childNumber="' + liCount + '"></a></li>');
    });
});

$(document).on('click', '#addCity button#add', function (e) {
	e.preventDefault();
	
	var inputCity = $('form#addCity input#inputCity');

	if(inputCity.val().length < 2)
	{
		alert('Введенное значение должно содержать минимум два символа');

		inputCity.val('');

		return;
	}

	var liCount = $('#cityList li').length;

	$('#cityList').append('<li class="list-group-item text-center" childNumber="' + liCount + '"><a href="#">' + inputCity.val() + '</a><a href="#" class="glyphicon glyphicon-remove" style="padding-top:3px;float:right;text-decoration:none;" childNumber="' + liCount + '"></a></li>');

	var cities = GetCities();

	cities.push($('#addCity input#inputCity').val());

	$.cookie('cities', JSON.stringify(cities));

	inputCity.val('');
});

$(document).on('click', '#cityList a.glyphicon-remove', function(e){
	var cities = GetCities();
	var childNumber = $(this).attr('childNumber');
	var cityName = $('ul#cityList li[childNumber=' + childNumber + '] a:first-child')[0].innerText;
	var index = cities.indexOf(cityName);

	cities.splice(index, 1);

	$.cookie('cities', JSON.stringify(cities));

	$('#cityList li[childNumber=' + childNumber + ']').remove();
});

$(document).on('click', '#infSelection button', function(e){
	var childNumber = $(this).parent().attr('childNumber');

	if($(this).attr('id') === 'yandexWeather')
	{
		$('div[childNumber=' + childNumber + '] div[weatherInf="YW"]').show();
		$('div[childNumber=' + childNumber + '] div[weatherInf="OWM"]').hide();
	}

	if($(this).attr('id') === 'openWeatherMap')
	{
		$('div[childNumber=' + childNumber + '] div[weatherInf="OWM"]').show();
		$('div[childNumber=' + childNumber + '] div[weatherInf="YW"]').hide();
	}	
});

function GetCities() {
    var citiesStr = $.cookie('cities');
    var cities = new Array();

    if (citiesStr != undefined) {
        cities = $.parseJSON(citiesStr);
    }

    return cities;
}