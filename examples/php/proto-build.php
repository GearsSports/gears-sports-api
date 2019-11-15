<?php

$thisDir = dirname(__FILE__);
$dir = new RecursiveDirectoryIterator($thisDir.'/../../gears/proto');
$itr = new RecursiveIteratorIterator($dir);

$protoIncludePath = "../../third_party/";
$protoPath  = "../../";
$outPath    = 'Compiled/';
$grpcPlugin = $thisDir . '/bins/grpc_php_plugin';

foreach ($itr as $fileInfo) {
    if ($fileInfo->isFile()) {
        echo ".";
        $name = $fileInfo->getFilename();

        if(strpos($name,'.proto') === false) {
            continue;
        }

        $path = $fileInfo->getPathname();
        $pathName = 'gears/proto' . explode('gears/proto',$path)[1];

        $command = "protoc --proto_path=\"{$protoIncludePath}\" --proto_path=\"{$protoPath}\" --php_out=\"{$outPath}\" \"{$pathName}\" ";

        // add flags to service files
        if(stripos($name,'service.proto') !== false) {
            $command .= " --grpc_out=\"{$outPath}\"";
            $command .= " --plugin=protoc-gen-grpc={$grpcPlugin}";
        }

        exec($command);
    }
}

echo "\n";
die;