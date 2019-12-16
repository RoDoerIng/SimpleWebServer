# SimpleWebServer

A very simple, embedIO based webserver to serve static files.

## Usage

When publishing this project using the command from [here](#self-contained-single-executable) a single executable will be created. Open it from the command line Calling `SimpleWebServer.exe [path\from\where\the\server\runs]`.

The path parameter is required! If no path is provided, the webserver will not fire up. 

If the path is valid, SimpleWebServer will check for any of the default files from the following list:

- `index.html`
- `index.htm`
- `default.html`
- `default.htm`

If none of the files can be found, execution is terminated.

In case one of these files can be found, it will automatically be opened in the default browser.

## Publishing

### Win x64

#### Self contained single executable

Thanks to [Wade](https://dotnetcoretutorials.com/2019/06/27/the-publishtrimmed-flag-with-il-linker/) I found this very useful command:

`dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`
