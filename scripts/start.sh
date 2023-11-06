#!/bin/bash

BUILD_NUMBER=$(ls target/ | sort -r | head -n1)

echo "Starting build $BUILD_NUMBER";

dotnet target/$BUILD_NUMBER/build/app.dll
