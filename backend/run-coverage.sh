#!/bin/bash

# Script för att köra tester med code coverage och generera HTML-rapport

set -e

echo "🔍 Kör tester med code coverage..."

# Kör tester med coverage
dotnet test \
  --collect:"XPlat Code Coverage" \
  --results-directory:"./coverage" \
  --settings:coverlet.runsettings \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

echo "📊 Genererar HTML-rapport..."

# Installera reportgenerator om den inte redan är installerad
if ! dotnet tool list -g | grep -q "reportgenerator"; then
    echo "📦 Installerar reportgenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# Generera HTML-rapport
reportgenerator \
  -reports:"./coverage/**/coverage.opencover.xml" \
  -targetdir:"./coverage/report" \
  -reporttypes:"Html;Badges" \
  -classfilters:"-*Migrations*;-*Program*;-*GlobalUsings*"

echo "✅ Code coverage rapport genererad!"
echo "📁 Öppna coverage/report/index.html i din webbläsare för att se rapporten"
echo ""
echo "📈 Coverage-badges finns i: coverage/report/badges/"

