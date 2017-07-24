// function for selecting some user

var EcmOp = (function () {

	var _checkUser = function(user_dn) {
        var patt = /user=(\w*)\/.*/
        var user = patt.exec(user_dn)[1].toUpperCase();
        if (user == 'U305499' || user == 'ADMIN' || user == 'U086412' || user == 'U0F4445' || user == 'U0F4568' || user == 'U086356' || user == 'U0F4436' || user == 'U0F3302' || user == 'U0F5746' || user == 'U086380')
            return true;
        else
            return false;
    }
	
   return {
        checkUser:_checkUser
    };

})();