#!/usr/bin/env bash

dotnet publish Retrospective.sln
docker stop retrospective || true && docker rm retrospective || true
docker build -t "1.1.0" . && docker run --rm -p 80:80 --name retrospective "1.1.0"