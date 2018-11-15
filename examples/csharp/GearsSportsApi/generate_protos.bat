@rem Generate the C# code for .proto files

setlocal

@rem enter this directory
cd /d %~dp0

@rem restore nuget packages
dotnet restore

@rem packages will be available in nuget cache directory once the project is built or after "dotnet restore"
set PROTOC=%UserProfile%\.nuget\packages\Grpc.Tools\1.6.1\tools\windows_x64\protoc.exe
set PLUGIN=%UserProfile%\.nuget\packages\Grpc.Tools\1.6.1\tools\windows_x64\grpc_csharp_plugin.exe

@rem create directories
if not exist GearsSportsApi\Gears\Proto ( mkdir GearsSportsApi\Gears\Proto )
if not exist GearsSportsApi\Gears\Proto\Actor ( mkdir GearsSportsApi\Gears\Proto\Actor )
if not exist GearsSportsApi\Gears\Proto\Api ( mkdir GearsSportsApi\Gears\Proto\Api )
if not exist GearsSportsApi\Gears\Proto\Api\V1 ( mkdir GearsSportsApi\Gears\Proto\Api\V1 )
if not exist GearsSportsApi\Gears\Proto\Api\V1\Capture ( mkdir GearsSportsApi\Gears\Proto\Api\V1\Capture )
if not exist GearsSportsApi\Gears\Proto\App ( mkdir GearsSportsApi\Gears\Proto\App )
if not exist GearsSportsApi\Gears\Proto\Capture ( mkdir GearsSportsApi\Gears\Proto\Capture )
if not exist GearsSportsApi\Gears\Proto\Golf ( mkdir GearsSportsApi\Gears\Proto\Golf )
if not exist GearsSportsApi\Gears\Proto\Manual ( mkdir GearsSportsApi\Gears\Proto\Manual )
if not exist GearsSportsApi\Gears\Proto\Player ( mkdir GearsSportsApi\Gears\Proto\Player )
if not exist GearsSportsApi\Gears\Proto\Server ( mkdir GearsSportsApi\Gears\Proto\Server )
if not exist GearsSportsApi\Gears\Proto\Server\Capture ( mkdir GearsSportsApi\Gears\Proto\Server\Capture )

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Vector2.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Vector3.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Marker.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Matrix.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Quaternion.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto --grpc_out GearsSportsApi/Gears/Proto --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/Transform.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Actor --grpc_out GearsSportsApi/Gears/Proto/Actor --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/actor/Actor.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Actor --grpc_out GearsSportsApi/Gears/Proto/Actor --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/actor/HandPose.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Actor --grpc_out GearsSportsApi/Gears/Proto/Actor --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/actor/Pose.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api --grpc_out GearsSportsApi/Gears/Proto/Api --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/code.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api --grpc_out GearsSportsApi/Gears/Proto/Api --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/error_details.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api --grpc_out GearsSportsApi/Gears/Proto/Api --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/status.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1 --grpc_out GearsSportsApi/Gears/Proto/Api/V1 --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/AuthService.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1 --grpc_out GearsSportsApi/Gears/Proto/Api/V1 --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/CaptureService.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1 --grpc_out GearsSportsApi/Gears/Proto/Api/V1 --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/InstallationService.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1 --grpc_out GearsSportsApi/Gears/Proto/Api/V1 --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/OrganizationService.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1 --grpc_out GearsSportsApi/Gears/Proto/Api/V1 --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/PlayerService.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1/Capture --grpc_out GearsSportsApi/Gears/Proto/Api/V1/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/capture/ExportService.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Api/V1/Capture --grpc_out GearsSportsApi/Gears/Proto/Api/V1/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/api/v1/capture/SnapshotService.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/App --grpc_out GearsSportsApi/Gears/Proto/App --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/app/GraphicsPackage.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/Capture.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/CaptureType.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/Frame.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/GraphFrame.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/MarkerFrame.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Capture --grpc_out GearsSportsApi/Gears/Proto/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/capture/Summary.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/AddressInfo.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/BodyAngles.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/Capture.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/Club.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/ClubModel.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/ClubType.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/Frame.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/LaunchAngle.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/ShaftFlex.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/StrikeInfo.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Golf --grpc_out GearsSportsApi/Gears/Proto/Golf --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/golf/Summary.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Manual --grpc_out GearsSportsApi/Gears/Proto/Manual --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/manual/BodyAngles.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Manual --grpc_out GearsSportsApi/Gears/Proto/Manual --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/manual/Capture.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Manual --grpc_out GearsSportsApi/Gears/Proto/Manual --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/manual/Frame.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Manual --grpc_out GearsSportsApi/Gears/Proto/Manual --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/manual/Summary.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Player --grpc_out GearsSportsApi/Gears/Proto/Player --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/player/Gender.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Player --grpc_out GearsSportsApi/Gears/Proto/Player --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/player/Handed.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Player --grpc_out GearsSportsApi/Gears/Proto/Player --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/player/Player.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server --grpc_out GearsSportsApi/Gears/Proto/Server --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/CaptureInfo.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server --grpc_out GearsSportsApi/Gears/Proto/Server --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/Filters.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server --grpc_out GearsSportsApi/Gears/Proto/Server --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/Installation.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server --grpc_out GearsSportsApi/Gears/Proto/Server --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/Organization.proto

%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server/Capture --grpc_out GearsSportsApi/Gears/Proto/Server/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/capture/Export.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server/Capture --grpc_out GearsSportsApi/Gears/Proto/Server/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/capture/ExportType.proto
%PROTOC% -I../../../ -I../../../third_party --csharp_out GearsSportsApi/Gears/Proto/Server/Capture --grpc_out GearsSportsApi/Gears/Proto/Server/Capture --plugin=protoc-gen-grpc=%PLUGIN% ../../../gears/proto/server/capture/Snapshot.proto

endlocal
