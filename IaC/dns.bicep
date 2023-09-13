@description('Base Name of Resources')
param commonResourceName string = 'PartyTrivia'
var webAppName = toLower(commonResourceName)

resource staticWebApp 'Microsoft.Web/staticSites@2020-12-01' existing = {
  name: webAppName
}

resource staticWebAppDNS 'Microsoft.Web/staticSites/customDomains@2022-09-01' = {
  name: 'play.triviagame.party'
  parent: staticWebApp
  properties: {
    validationMethod: 'cname-delegation'
  }
}
