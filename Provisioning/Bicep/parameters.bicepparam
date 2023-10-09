using './main.bicep'

param aspKind = 'linux'
param aspSku = 'F1'
param location = 'westeurope'
param name = 'photura'
param tags = {
  App: 'Photura'
}
