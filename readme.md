# SimpleWebServer

An embedIO based webserver to serve static files.

## Publishing

### Win x64

#### Self contained single executable

Thanks to [Wade](https://dotnetcoretutorials.com/2019/06/27/the-publishtrimmed-flag-with-il-linker/) I found this very useful command:

`dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`
