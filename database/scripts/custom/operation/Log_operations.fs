//@debug off
var res = 'List of operations';
//formula.log.info("Printing operations for " + element.dname);
 var ops = element.operations;
 for(i=0; i < ops.length; i++) {
 //formula.log.info(ops[i].name + "|" + ops[i].command);
 res = res + '\n' + ops[i].name + "|" + ops[i].command
 }
 alert(res);