Java example
----------

(Optional) Setup
----------------
Edit the file [examples/data/TokenRequest.json](/examples/data/TokenRequest.json) so it has valid api credentials.
You get valid credentials for both sandbox and production environments by contacting our customer support.
If you do not complete this step then the server api examples will not work. They are still useful to show how to access our grpc server api however.

Run Examples
-----
```sh
$ # cd to this directory
$ # use gradle to build/run the tests
$ ./gradlew test
```

See [examples/java/public-api/build.gradle](/examples/java/public-api/build.gradle) for details on how our .proto files are converted to java and worked into the build process.

To view the examples look in [examples/java/public-api/src/test/java](/examples/java/public-api/src/test/java)
* [Example of loading a capture from a file.](/examples/java/public-api/src/test/java/LoadCaptureTest.java)
* [Example of loading a player from a file.](/examples/java/public-api/src/test/java/LoadPlayerTest.java)
* [Grpc server access examples.](/examples/java/public-api/src/test/java/ServerApiTest.java)

Useful links:
-------------
* [Protobuf java documentation](https://developers.google.com/protocol-buffers/docs/javatutorial)
* [Grpc java documentation](https://grpc.io/docs/tutorials/basic/java.html)
