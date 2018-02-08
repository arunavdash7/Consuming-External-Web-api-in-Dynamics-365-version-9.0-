    function WeatherForecast() {
	var serverUrl = Xrm.Page.context.getClientUrl();
var actionName = "new_WheatherForecastCustomAction";
var accountId = Xrm.Page.data.entity.getId().replace("{","").replace("}","");
var ODataPath = serverUrl +"/api/data/v9.0/accounts("+ accountId + ")/Microsoft.Dynamics.CRM."+actionName;

 var userRequest = new XMLHttpRequest();
userRequest.open("POST", encodeURI(ODataPath), true);
userRequest.setRequestHeader("OData-MaxVersion", "4.0");
        userRequest.setRequestHeader("OData-Version", "4.0");
        userRequest.setRequestHeader("Accept", "application/json");
        userRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        
        userRequest.send();

 if (userRequest.status === 200) {
        var retrievedUser = JSON.parse(userRequest.responseText);
        debugger;
}
}