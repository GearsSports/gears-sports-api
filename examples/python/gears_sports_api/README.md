Python example
--------------

Setup
-----
```sh
$ # cd to this directory
$ pip install -r requirements.txt
$ # generate python from .proto files
$ python build.py
$ # the .py files generated from our .proto files should now be in ./gears/proto/
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
$ python -m unittest discover -s . --verbose
```

To view the source for the examples look at [tests.py](/examples/python/gears_sports_api/tests.py)

These examples are tested against python 3.6 but should work with other versions.

Useful links:
-------------
* [Protobuf python documentation](https://developers.google.com/protocol-buffers/docs/pythontutorial)
* [Grpc python documentation](https://grpc.io/docs/tutorials/basic/python.html)
