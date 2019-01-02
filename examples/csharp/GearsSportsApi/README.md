C# example
----------

Our c# example solution contains 3 projects:
* [GearsSportsApi](/examples/csharp/GearsSportsApi/GearsSportsApi/) - Creates a lib containing c# code generated from our .proto files.
* [GearsSportsApi.Tests](/examples/csharp/GearsSportsApi/GearsSportsApi.Tests/) - A unit test project where each test demonstrates some functionality provided by the c# code generated from our .proto files.
* [BuildProtoFiles](/examples/csharp/GearsSportsApi/BuildProtoFiles/) - Helper project used to work c# code generation from out .proto files into the standard build process.

(Optional) Setup
----------------
Edit the file [<repo_root>/examples/data/TokenRequest.json](/examples/data/TokenRequest.json) so it has valid api credentials.
You get valid credentials for both sandbox and production environments by contacting our customer support.
If you do not complete this step then the server api examples will not work. They are still useful to show how to access our grpc server api however.

Build (Visual Studio)
---------------------
* Load the solution [GearsSportsApi.sln](/examples/csharp/GearsSportsApi/GearsSportsApi.sln)
* Build solution

The first time you build the solution it will fail as it is generating .cs files from our .proto files.
Attempting to build to solution will result in success.

Build (Command line)
--------------------
```sh
$ # cd to this directory
$ # restore nuget packages
$ dotnet restore
$ # Build lib containing the generated .cs files
$ # The .cs files are generated during this projects pre-build event.
$ dotnet build GearsSportsApi/GearsSportsApi.csproj
```

After building the solution you will find the generated .cs files at <repo_root>/examples/csharp/GearsSportsApi/GearsSportsApi/Gears/Proto/

Run Examples
------------
Since the examples take the form of unit tests running them is the same as running any other unit tests.
```sh
$ # from this directory
$ cd GearsSportsApi.Tests/
$ # build examples project
$ dotnet build GearsSportsApi.Tests.csproj
$ # run the examples/tests
$ dotnet test
```

To view the examples look at the project [GearsSportsApi.Tests.csproj](/examples/csharp/GearsSportsApi/GearsSportsApi.Tests/GearsSportsApi.Tests.csproj)
* [Example of loading a capture from a file.](/examples/csharp/GearsSportsApi/GearsSportsApi.Tests/LoadCapture.cs)
* [Example of loading a player from a file.](/examples/csharp/GearsSportsApi/GearsSportsApi.Tests/LoadPlayer.cs)
* [Grpc server access examples.](/examples/csharp/GearsSportsApi/GearsSportsApi.Tests/ServerApiAccess.cs)

Useful links:
-------------
* [Protobuf c# documentation](https://developers.google.com/protocol-buffers/docs/csharptutorial)
* [Grpc c# documentation](https://grpc.io/docs/tutorials/basic/csharp.html)
