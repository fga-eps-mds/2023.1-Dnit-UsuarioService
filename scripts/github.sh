#!/bin/bash


dotnet publish -o build

tar -czvf build.tar.gz ./build

DEPLOY_HOST=http://localhost:30001/update

curl --fail -X POST -L -F "file=@build.tar.gz"  $DEPLOY_HOST/build -H "upload_token: secret"