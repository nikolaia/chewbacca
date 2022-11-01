#!/bin/bash
echo "\e[42m~~~>\e[0m Building solution"

while test $# -gt 0
do
    case "$1" in
        --clean) 
            echo "\e[42m~~~>\e[0m Removing the build and artifacts directories"
            rm -rf build artifacts
            ;;
        --build) 
            echo "\e[42m~~~>\e[0m Building solution"
            dotnet build -c Release
            ;;
        --test) 
            echo "\e[42m~~~>\e[0m Running tests"
            dotnet test -c Release
            ;;
        --artifact) 
            dotnet publish ./src/WebApi/ -c Release -r linux-x64 --self-contained false -o build
            echo "\e[42m~~~>\e[0m Building zip artifact"
            cd build
            zip -9 -r chewbacca.zip .
            cd ../
            mkdir artifacts
            mv ./build/chewbacca.zip artifacts/
            ;;
    esac
    shift
done

exit 0