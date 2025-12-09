# PowerShell script för att köra tester med code coverage och generera HTML-rapport

Write-Host "🔍 Kör tester med code coverage..." -ForegroundColor Cyan

# Kör tester med coverage
dotnet test `
  --collect:"XPlat Code Coverage" `
  --results-directory:"./coverage" `
  --settings:coverlet.runsettings `
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

Write-Host "📊 Genererar HTML-rapport..." -ForegroundColor Cyan

# Installera reportgenerator om den inte redan är installerad
$reportGeneratorInstalled = dotnet tool list -g | Select-String "reportgenerator"
if (-not $reportGeneratorInstalled) {
    Write-Host "📦 Installerar reportgenerator..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
}

# Generera HTML-rapport
reportgenerator `
  -reports:"./coverage/**/coverage.opencover.xml" `
  -targetdir:"./coverage/report" `
  -reporttypes:"Html;Badges" `
  -classfilters:"-*Migrations*;-*Program*;-*GlobalUsings*"

Write-Host "✅ Code coverage rapport genererad!" -ForegroundColor Green
Write-Host "📁 Öppna coverage/report/index.html i din webbläsare för att se rapporten" -ForegroundColor Yellow
Write-Host ""
Write-Host "📈 Coverage-badges finns i: coverage/report/badges/" -ForegroundColor Yellow

