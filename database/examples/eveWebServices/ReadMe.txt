
READ ME: For using the XML format in the Event Integrator.


When creating an Evenet Integrator Agent that will be processing XML, 
(which is selected by the "Data Type" in the agent creation dialog) 
the xml must conform to the Managed Objects Normalized Event Format:

<events>
   <event class="event_class">
      name=value,
      name2=value2,
         ...
      nameN=valueN
  </event>
  <event...
  </event>
</events>

For xml that does not originally conform to this format, the selection 
"XSLT Transform" must be checked on the agent, and an XSLT file added 
that will format the input XML into the normalized format.

This is very tersely explained in the Event Integrator documentation 
under "Creating an Agent".

The Eve agent requires that either the XML already be in the normalized 
format, or that you attach the xslt to translate it into this format.  
The event_format.xml  shows 4 examples of the manner in which events 
may be accepted, but basically you need a "class"  and a set of 
name=value pairs which then become the event/alarm in Eve.

