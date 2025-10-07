#!/bin/bash
set -ex
scriptDir=$(dirname "$0")
echo "scriptDir: $scriptDir"
rm -rf "$scriptDir/TestResults"
dotnet nuget locals all --clear
MAX_RETRIES=3
RETRY_DELAY=5
for i in $(seq 1 $MAX_RETRIES); do
    dotnet restore && break
    echo "Restore failed. Attempt $i/$MAX_RETRIES"
    if [ "$i" -lt "$MAX_RETRIES" ]; then
        echo "Retrying in $RETRY_DELAY seconds..."
        sleep $RETRY_DELAY
    else
        echo "Restore failed after $MAX_RETRIES attempts."
        exit 1
    fi
done
dotnet test --collect:"XPlat Code Coverage" --logger "console;verbosity=detailed"
