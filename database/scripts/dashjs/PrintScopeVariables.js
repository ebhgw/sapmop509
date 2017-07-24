if(identity !== undefined){
console.log('identity: '+ identity);
}

if(element !== undefined){
	console.log('Displaying element properties\n');
	for (var property in element) {
		if (element.hasOwnProperty(property)) {
			console.log(property + '=' + element.property+'\n');
		}
	}
}

if(alarms !== undefined){
	for (var i = 0; i < alarms.length; i ++){
		console.log('alarm '+ i);
		var alarm = alarms[i];
		for (var property in alarm) {
			if (element.hasOwnProperty(property)) {
				console.log(property + '=' + alarm.property+'\n');
			}
		}
	}
}