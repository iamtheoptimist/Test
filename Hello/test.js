var mcb = (function(){
	var A = function()
	{
		this.test = function(){
			alert('Go!');
		}
	}
	
	return {
		A: A
	};
})();

var t = new mcb.A();
t.test();