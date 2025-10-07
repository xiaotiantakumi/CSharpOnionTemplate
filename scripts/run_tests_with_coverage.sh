#!/bin/bash
set -ex

scriptDir=$(dirname "$0")
echo "Running tests with coverage report generation..."

# Run tests
"$scriptDir/run_tests.sh"

# Generate coverage report
"$scriptDir/generate_coverage_report.sh"

echo "All done! Check the coverage report in: $scriptDir/../coverage-report/index.html"
