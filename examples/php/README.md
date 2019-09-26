PHP example
--------------

Prerequisites
-------------
Using this library in PHP requires that you have the GRPC PHP extension added and enabled (https://grpc.io/docs/quickstart/php#install-the-grpc-php-extension),
as well as the protobuf compiler `protoc` installed (https://grpc.io/docs/quickstart/php#install-protobuf-compiler),
and the PHP protoc plugin (https://grpc.io/docs/quickstart/php#php-protoc-plugin). 

**Move or copy the grpc_php_plugin file (from above) to <repo_root>/examples/php/bins/.**  

This example also depends on composer (the PHP package manager).  
https://getcomposer.org/doc/00-intro.md


Setup
-----
```sh
$ # First you'll need to 
$ # cd to this directory
$ # Use composer to install the example's dependencies
$ composer install
$ # generate php from .proto files (requires protoc is an executable command on your machine)
$ php proto-build.php
$ # the .php files generated from our .proto files should now be in ./examples/php/Compiled/
```

(Optional) Setup
----------------
Edit the file [<repo_root>/examples/data/TokenRequest.json](/examples/data/TokenRequest.json) so it has valid api credentials.
You get valid credentials for both sandbox and production environments by contacting our customer support.
If you do not complete this step then the server api examples will not work. They are still useful to show how to access our grpc server api however.

Run Examples
------------
Since the examples take the form of unit tests running them is the same as running any other unit tests.
```sh
$ # cd to this directory
$ phpunit tests
```

To view the source for the examples look at [tests.php](/examples/php/gears_sports_api/tests.php)

These examples are tested against php 7.1.23 but should work with other versions.

Useful links:
-------------
* [Protobuf php documentation](https://github.com/protocolbuffers/protobuf/tree/master/php)
* [Grpc php documentation](https://grpc.io/docs/tutorials/basic/php)
