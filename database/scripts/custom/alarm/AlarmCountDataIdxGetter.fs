/*

 Script AlarmIdxGetter.fs

 Author: Bomitali Evelino - Hogwart s.r.l.
 Tested with versions: 5.0

 Allow to count ticket associated to an element

 */

var alarmIdxGetter = {
if(element["Adapter: AlarmsCountData.OpenTicketNum"]){
  element["Adapter: AlarmsCountData.OpenTicketNum"]
}else if(element.elementClassname=='Acronimo'||element.elementClassname=='AcronimoOpVw'){
  try{
    var dn='AcronimoData='+formula.util.encodeURL(element.name)+'/Acronimi=Acronimi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    //formula.log.info('looking for ' + dn);
    var el=formula.Root.findElement(dn);
    el.OpenTicketNum
  }catch(e){
    //formula.log.error('excp ' + e);
    0
  }
}else if(element.elementClassname=='Servizio'){
  try{
    var dn='ServizioData='+formula.util.encodeURL(element.name)+'/Servizi=Servizi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
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




if(element["Adapter: AlarmsCountData.AckTicketNum"]){
  element["Adapter: AlarmsCountData.AckTicketNum"]
}else if(element.elementClassname=='Acronimo'||element.elementClassname=='AcronimoOpVw'){
  try{
    var dn='AcronimoData='+formula.util.encodeURL(element.name)+'/Acronimi=Acronimi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    var el=formula.Root.findElement(dn);
    el.AckTicketNum
  }catch(e){
    0
  }
}else if(element.elementClassname=='Servizio'){
  try{
    var dn='ServizioData='+formula.util.encodeURL(element.name)+'/Servizi=Servizi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    var el=formula.Root.findElement(dn);
    el.AckTicketNum
  }catch(e){
    //formula.log.error('excp ' + e);
    0
  }
}else{
  0
}

if(element["Adapter: AlarmsCountData.OpenAlarmMinCondition"]){
  element["Adapter: AlarmsCountData.OpenAlarmMinCondition"]
}else if(element.elementClassname=='Acronimo'||element.elementClassname=='AcronimoOpVw'){
  try{
    var dn='AcronimoData='+formula.util.encodeURL(element.name)+'/Acronimi=Acronimi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    var el=formula.Root.findElement(dn);
    el.OpenAlarmMinCondition
  }catch(e){
    0
  }
}else if(element.elementClassname=='Servizio'){
  try{
    var dn='ServizioData='+formula.util.encodeURL(element.name)+'/Servizi=Servizi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    //formula.log.info('looking for ' + dn);
    var el=formula.Root.findElement(dn);
    el.OpenAlarmMinCondition
  }catch(e){
    //formula.log.error('excp ' + e);
    0
  }
}else{
  0
}


if(element["Adapter: AlarmsCountData.AlarmWithTicket"]){
  element["Adapter: AlarmsCountData.AlarmWithTicket"]
}else if(element.elementClassname=='Acronimo'||element.elementClassname=='AcronimoOpVw'){
  try{
    var dn='AcronimoData='+formula.util.encodeURL(element.name)+'/Acronimi=Acronimi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    var el=formula.Root.findElement(dn);
    el.AlarmWithTicket
  }catch(e){
    0
  }
}else if(element.elementClassname=='Servizio'){
  try{
    var dn='ServizioData='+formula.util.encodeURL(element.name)+'/Servizi=Servizi/AlarmsCountData=Adapter%3A+AlarmsCountData/root=Elements';
    //formula.log.info('looking for ' + dn);
    var el=formula.Root.findElement(dn);
    el.AlarmWithTicket
  }catch(e){
    //formula.log.error('excp ' + e);
    0
  }
}else{
  0
}

}