#!/bin/bash
set -x

scriptDir=$(dirname "$0")
echo "scriptDir: $scriptDir"

# Find the latest coverage file (macOS compatible)
coverageFilePath=$(find "$scriptDir/../TestResults" -name "coverage.cobertura.xml" -exec ls -t {} + | head -n 1)
echo "coverageFilePath: $coverageFilePath"

if [ -z "$coverageFilePath" ]; then
    echo "No coverage file found. Please run tests first."
    exit 1
fi

# Create coverage report directory
mkdir -p "$scriptDir/../coverage-report"

# Copy the coverage file to a more accessible location
cp "$coverageFilePath" "$scriptDir/../coverage-report/coverage.cobertura.xml"

echo "Coverage file copied to: $scriptDir/../coverage-report/coverage.cobertura.xml"
echo "You can view the coverage data in this XML file or use it with other tools."
echo ""
echo "To generate an HTML report, you can:"
echo "1. Install reportgenerator: dotnet tool install -g dotnet-reportgenerator-globaltool"
echo "2. Run: reportgenerator -reports:coverage-report/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html"
