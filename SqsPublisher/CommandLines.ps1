
dotnet lambda deploy-function SimpleHttpLambda
dotnet lambda delete-function SimpleHttpLambda

dotnet lambda invoke-function SimpleLambda --payload "{ \"World\": \"This is from the body request\" }"

dotnet tool install -g Amazon.Lambda.TestTool-8.0
dotnet lambda-test-tool-8.0


