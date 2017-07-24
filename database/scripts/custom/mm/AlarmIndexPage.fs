try{
    var dn='AcronimoData=' + formula.util.encodeURL(element.Ambiente) + '_'+formula.util.encodeURL(element.name)+'/Acronimi=Acronimi/gen_folder='+formula.util.encodeURL(element.Ambiente)+'/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    //formula.log.info('looking for ' + dn);
    var el=formula.Root.findElement(dn);
    el.OpenTicketNum
}catch(e){
    //formula.log.error('excp ' + e);
    0
}
}else if(element.elementClassname=='Servizio'){
    try{
        var dn='ServizioData=' + formula.util.encodeURL(element.Ambiente) + '_'+formula.util.encodeURL(element.name)+'/Servizi=Servizi/gen_folder='+formula.util.encodeURL(element.Ambiente)+'/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
        //formula.log.info('looking for ' + dn);
        var el=formula.Root.findElement(dn);
        el.OpenTicketNum
    }catch(e){
        //formula.log.error('excp ' + e);
        0
    }
}else{
    0
}