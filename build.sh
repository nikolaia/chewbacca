#!/bin/bash
echo "\e[42m~~~>\e[0m Building solution"
dotnet build -c Release

while test $# -gt 0
do
    case "$1" in
        --clean) 
            echo "\e[42m~~~>\e[0m Removing the build and artifacts directories"
            rm -rf build artifacts
            ;;
        --test) 
            echo "\e[42m~~~>\e[0m Running tests"
            dotnet test -c Release --no-build
            ;;
        --artifact) 
            dotnet publish ./src/WebApi/ -c Release --no-build -o build
            echo "\e[42m~~~>\e[0m Building zip artifact"
            cd build
            zip -9 -r -q chewbacca.zip .
            cd ../
            mkdir artifacts
            mv ./build/chewbacca.zip artifacts/
            ;;
    esac
    shift
done

exit 0