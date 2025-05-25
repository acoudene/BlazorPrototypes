_2023-12-23 - Anthony Coudène - Creation_

# VerticalSliceArchitectureGenerator

A generator to produce a vertical slice architecture from API to integration coded tests

# DotnetNewTemplate

## By CLI

### Install

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-install

```
dotnet new install <PATH|NUGET_ID>  [--interactive] [--add-source|--nuget-source <SOURCE>] [--force] 
    [-d|--diagnostics] [--verbosity <LEVEL>] [-h|--help]
```

For example: 

`dotnet new install . --force`

### Use

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new

```
dotnet new <TEMPLATE> [--dry-run] [--force] [-lang|--language {"C#"|"F#"|VB}]
    [-n|--name <OUTPUT_NAME>] [-f|--framework <FRAMEWORK>] [--no-update-check]
    [-o|--output <OUTPUT_DIRECTORY>] [--project <PROJECT_PATH>]
    [-d|--diagnostics] [--verbosity <LEVEL>] [Template options]

dotnet new -h|--help
```

For example: 

`dotnet new vsa_generator -n MyMyFeature -o MyMyFeatureFolder -e MyEntity`

### Uninstall

See: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-uninstall

```
dotnet new uninstall <PATH|NUGET_ID> 
    [-d|--diagnostics] [--verbosity <LEVEL>] [-h|--help]
```

For example: 

`dotnet new uninstall .`

### Template definition

Use `.template.config\template.json` with this schema: http://json.schemastore.org/template 

For example: 

```
{
  "$schema": "http://json.schemastore.org/template",
  "author": "Anthony COUDENE",
  "classifications": [ "Api", "Architecture" ],
  "name": "Vertical Slice Architecture generator",
  "identity": "acoudene.vsa.generator", // Unique name for this template
  "groupIdentity": "acoudene.vsa.generator",
  "shortName": "vsa_generator", // Short name that can be used on the cli
  "tags": {
    "language": "C#",
    "type": "solution"
  },
  "sourceName": "MyFeature", // Will replace this string with the value provided via -n.
  "symbols": {
    "entityContentName": {
      "type": "parameter",
      "defaultValue": "MyEntity",
      "replaces": "MyEntity"
    },
    "entityCamelCaseContentName": {
      "type": "derived",
      "valueSource": "entityContentName",
      "valueTransform": "replace", // TODO - Find a right way of preserving camel case here...
      "replaces": "myEntity"
    },
    "entityFileName": {
      "type": "derived",
      "valueSource": "entityContentName",
      "valueTransform": "replace",
      "fileRename": "MyEntity"
    },
    "myfeatureContentName": {
      "type": "derived",
      "valueSource": "name",
      "valueTransform": "replace",
      "replaces": "MyFeature"
    },
    "myfeatureSolutionName": {
      "type": "derived",
      "valueSource": "name",
      "valueTransform": "replace",
      "fileRename": "VerticalSliceArchitecture"
    }
  },
  "sources": [
    {
      "modifiers": [
        {
          "exclude": [ ".vs/**" ]
        }
      ]
    }
  ],
  "preferNameDirectory": "true"
}
```

`shortName` is useful for CLI.
`groupIdentity` value is important to link with Visual Studio wizards.
`symbols` array is used to template options given by user or computed.

## By Visual Studio

### Use

1. Search for templates "Vertical Slice Architecture" and use it
2. Give "Project name", "Location", "Solution name" and options.
3. Set template options like "Class name of entity"
4. Create solution

### Template definition

Use `.template.config\ide.host.json` with this schema: http://json.schemastore.org/vs-2017.3.host 
associated to previous template definition in `.template.config\template.json`.

For example: 

```
{
  "$schema": "http://json.schemastore.org/vs-2017.3.host",
  "icon": "VerticalSliceArchitecture.png",
  "symbolInfo": [
    {
      "id": "entityContentName",
      "name": {
        "text": "Class name of entity"
      },
      "isVisible": "true"
    }
  ]
}
```

`groupIdentity` value in `.template.config\template.json` remains important to link with Visual Studio wizards.
`symbolInfo` array is used to link with template options given by user or computed.

# Runing solution

## Prerequesites

Having Docker Desktop installed on the hosted computer.
Run a MongoDb container (default port: 27017 on local machine) to use and manipulate API Host : [MyFeature.Host](./MyFeature.Host/)

```
docker run --name mymongo -d mongo:tag
```

See: https://hub.docker.com/_/mongo

Nothing to install for Integration tests : [MyFeature.Host.Tests](./MyFeature.Host.Tests/)

