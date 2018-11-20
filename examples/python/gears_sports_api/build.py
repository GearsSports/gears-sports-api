import os
import sys
import glob
import subprocess

folder = os.path.dirname(os.path.realpath(__file__))

root_folder = os.path.realpath(os.path.join(folder, '..', '..', '..'))
proto_folder = os.path.join(root_folder, 'gears')
third_party = os.path.join(root_folder, 'third_party')

# clear out old generated protobuf/grpc files
for suffix in ['*_pb2.py', '*_pb2_grpc.py']:
    for filename in glob.iglob(os.path.join(folder, 'gears', 'proto', '**', suffix), recursive=True):
        os.remove(filename)

# set path to python interpreter that will be used for the build commands.
_python_path = sys.executable
if not _python_path or not os.path.isfile(_python_path):
    _python_path = 'python'


def proto_build(proto_file: str):
    """
    Converts the provided .proto file into the correct .py file.
    :param proto_file: Absolute path to a .proto file
    """
    params = [
        _python_path,
        '-m',
        'grpc_tools.protoc',
        '-I', root_folder,
        '-I', third_party,
        '--python_out', folder,
    ]
    # if the .proto file contains a service add the args needed by grpc
    if proto_file.endswith('Service.proto'):
        params.append('--grpc_python_out=%s' % folder)

    params.append(proto_file)
    sys.stdout.write('command: %s\n' % ' '.join(params))
    sys.stdout.flush()
    result = subprocess.run(
        params,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        check=True
    )
    if len(result.stderr) > 0 or len(result.stdout) > 0:
        raise Exception(result)


num_built = 0
for filename in glob.iglob(os.path.join(proto_folder, '**', '*.proto'), recursive=True):
    proto_build(filename)
    print(filename)
    num_built += 1
print("%s .proto files build" % num_built)
