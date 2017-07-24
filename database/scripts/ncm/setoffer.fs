// @opt -1

if( element.elementClassName == 'wlIcon' )
{
   if( ! state.offerDefinitionTemplate )
      state.offerDefinitionTemplate = 
      '<offers>\n' +
      '  <offerDefinitionList>\n' +
      '    <offerDefinition>\n' +
      '      <objectives>\n' +
      '        <objectiveDefinition>\n' +
      '          <name>Availability - On</name>\n' +
      '          <constraintFlags>18</constraintFlags>\n' +
      '          <low>${availability}</low>\n' +
      '          <high>100.0</high>\n' +
      '          <durationFlags>2</durationFlags>\n' +
      '          <durationLow>0.0</durationLow>\n' +
      '          <durationHigh>2.0</durationHigh>\n' +
      '          <durationUnits>1</durationUnits>\n' +
      '          <occurrences>0.0</occurrences>\n' +
      '          <expectedNumberForDenominator>0.0</expectedNumberForDenominator>\n' +
      '          <actions/>\n' +
      '          <conditionOnViolation>1</conditionOnViolation>\n' +
      '          <conditionOnWarning>2</conditionOnWarning>\n' +
      '          <pointSelectMask>0</pointSelectMask>\n' +
      '          <pointDivisorMask>0</pointDivisorMask>\n' +
      '          <pointAggregator>0</pointAggregator>\n' +
      '          <pointClassificationLow>-Infinity</pointClassificationLow>\n' +
      '          <pointClassificationHigh>1.7976931348623157E308</pointClassificationHigh>\n' +
      '          <pointClassification>0</pointClassification>\n' +
      '          <outlierLow>0.0</outlierLow>\n' +
      '          <outlierHigh>0.0</outlierHigh>\n' +
      '          <intervalEndFireOnly>false</intervalEndFireOnly>\n' +
      '          <matchGood>true</matchGood>\n' +
      '          <numericLatest>false</numericLatest>\n' +
      '          <aligned>false</aligned>\n' +
      '          <numericInterval>1</numericInterval>\n' +
      '          <numericUnits>-1</numericUnits>\n' +
      '          <type>0</type>\n' +
      '          <inheritance>0</inheritance>\n' +
      '          <rolling>false</rolling>\n' +
      '          <mode>0</mode>\n' +
      '          <interval>1</interval>\n' +
      '          <intervalUnits>5</intervalUnits>\n' +
      '          <calendar>Default</calendar>\n' +
      '          <categories>\n' +
      '            <string>On</string>\n' +
      '          </categories>\n' +
      '          <categoryIds>\n' +
      '            <int>3</int>\n' +
      '          </categoryIds>\n' +
      '          <minimumLength>0.0</minimumLength>\n' +
      '          <conditionThreshold>1</conditionThreshold>\n' +
      '          <overrideRemove>false</overrideRemove>\n' +
      '          <propagateRules>false</propagateRules>\n' +
      '          <inactive>false</inactive>\n' +
      '          <activationTime>0</activationTime>\n' +
      '          <deactivationTime>0</deactivationTime>\n' +
      '          <includeWeightInHealthRules>false</includeWeightInHealthRules>\n' +
      '          <weight>1.0</weight>\n' +
      '          <minimumLengthUnits>0</minimumLengthUnits>\n' +
      '        </objectiveDefinition>\n' +
      '      </objectives>\n' +
      '      <name>NCM Agreement</name>\n' +
      '      <overrideFlg>false</overrideFlg>\n' +
      '      <suppress>false</suppress>\n' +
      '      <showAsRoot>false</showAsRoot>\n' +
      '      <includeChildOffers>false</includeChildOffers>\n' +
      '      <defaultCondition>1</defaultCondition>\n' +
      '      <propagation>\n' +
      '        <propagateNam>true</propagateNam>\n' +
      '        <depthNam>5</depthNam>\n' +
      '        <propagateOrg>true</propagateOrg>\n' +
      '        <depthOrg>5</depthOrg>\n' +
      '        <applyToSelf>true</applyToSelf>\n' +
      '        <namAfterOrg>false</namAfterOrg>\n' +
      '        <orgAfterName>false</orgAfterName>\n' +
      '        <mode>0</mode>\n' +
      '      </propagation>\n' +
      '      <threshold>100.0</threshold>\n' +
      '      <remoteAccessible>true</remoteAccessible>\n' +
      '      <interval>1</interval>\n' +
      '      <intervalUnits>2</intervalUnits>\n' +
      '      <calendar>Default</calendar>\n' +
      '      <categories/>\n' +
      '      <categoryIds/>\n' +
      '      <minimumLength>0.0</minimumLength>\n' +
      '      <conditionThreshold>1</conditionThreshold>\n' +
      '      <algorithm>worst</algorithm>\n' +
      '      <overrideRemove>false</overrideRemove>\n' +
      '      <propagateRules>true</propagateRules>\n' +
      '      <inactive>false</inactive>\n' +
      '      <activationTime>0</activationTime>\n' +
      '      <deactivationTime>0</deactivationTime>\n' +
      '      <includeWeightInHealthRules>false</includeWeightInHealthRules>\n' +
      '      <weight>1.0</weight>\n' +
      '      <minimumLengthUnits>0</minimumLengthUnits>\n' +
      '    </offerDefinition>\n' +
      '  </offerDefinitionList>\n' +
      '</offers>\n'


   // Use transaction to set property.
   var tx = server.elementPersistence.beginTransaction()
   var BroadcastModify = Packages.com.mosol.configstore.ConfigTransaction.Policy.BroadcastModify
   var hadPolicy = tx.hasPolicy( BroadcastModify )
   tx.addPolicy( BroadcastModify )
   try
   {
      var availability = '99'
      if( structure && structure.availabilitySLA && structure[structure.availabilitySLA] )
         availability = structure[structure.availabilitySLA]
      formula.log.info( 'Creating availability agreement of ' + availability + ' on ' + element.dname )
      element.OfferDefinitionList = state.offerDefinitionTemplate.replace( '${availability}', String( availability ) )
   }
   finally
   {
      tx.commit()
      if( ! hadPolicy )
         tx.removePolicy( BroadcastModify )
   }

   // Comment out for production
   delete state.offerDefinitionTemplate
}

// @internal setoffer.fs bb20g32
