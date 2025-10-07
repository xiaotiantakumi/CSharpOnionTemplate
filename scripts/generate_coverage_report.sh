#!/bin/bash
set -x
scriptDir=$(dirname "$0")
echo "scriptDir: $scriptDir"
coverageFilePath=$(find "$scriptDir/TestResults" -name "coverage.cobertura.xml" -printf "%T+ %p\n" | sort -r | head -n 1 | cut -d' ' -f2-)
echo "coverageFilePath: $coverageFilePath"
export PATH="$PATH:/root/.dotnet/tools"
reportgenerator -reports:"$coverageFilePath" -targetdir:"coveragereport" -reporttypes:Html
