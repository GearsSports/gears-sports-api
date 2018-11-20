using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace BuildProtoFiles
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("{0} args provided", args.Length);
            for (int i = 0; i < args.Length; i++)
                Console.WriteLine("arg[{0}] = '{1}'", i, args[i]);

            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.WriteLine("userDir = '{0}', exists = {1}", userDir, Directory.Exists(userDir));
            var grpcTools = Path.Combine(userDir, ".nuget", "packages", "grpc.tools", "1.16.0", "tools");
            Console.WriteLine("grpcTools = '{0}', exists = {1}", grpcTools, Directory.Exists(grpcTools));

            var exeSuffix = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                grpcTools = Path.Combine(grpcTools, "linux_" + GetSuffix());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                grpcTools = Path.Combine(grpcTools, "macosx_" + GetSuffix());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                grpcTools = Path.Combine(grpcTools, "windows_" + GetSuffix());
                exeSuffix = ".exe";
            }
            else
            {
                throw new Exception("Failed to determine os.");
            }

            var protoc = Path.Combine(grpcTools, "protoc" + exeSuffix);
            var grpcPlugin = Path.Combine(grpcTools, "grpc_csharp_plugin" + exeSuffix);
            Console.WriteLine("grpcTools = '{0}', exists = {1}", grpcTools, Directory.Exists(grpcTools));
            Console.WriteLine("protoc = '{0}', exists = {1}", protoc, File.Exists(protoc));
            Console.WriteLine("grpcPlugin = '{0}', exists = {1}", grpcPlugin, File.Exists(grpcPlugin));

            var protoFilesDir = Path.GetDirectoryName(args[0]);
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            protoFilesDir = Path.GetDirectoryName(protoFilesDir);
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            protoFilesDir = Path.GetDirectoryName(protoFilesDir);
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            protoFilesDir = Path.GetDirectoryName(protoFilesDir);
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            protoFilesDir = Path.GetDirectoryName(protoFilesDir);
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            var rootDir = protoFilesDir;
            var thirdPartyDir = Path.Combine(rootDir, "third_party");
            Console.WriteLine("rootDir = '{0}', exists = {1}", rootDir, Directory.Exists(rootDir));
            Console.WriteLine("thirdPartyDir = '{0}', exists = {1}", thirdPartyDir, Directory.Exists(thirdPartyDir));
            protoFilesDir = Path.Combine(protoFilesDir, "gears");
            Console.WriteLine("protoFilesDir = '{0}', exists = {1}", protoFilesDir, Directory.Exists(protoFilesDir));
            var outDir = Path.Combine(rootDir, "examples", "csharp", "GearsSportsApi", "GearsSportsApi");
            Console.WriteLine("outDir = '{0}', exists = {1}", outDir, Directory.Exists(outDir));
            foreach (var protoFile in Directory.EnumerateFiles(protoFilesDir, "*.proto", SearchOption.AllDirectories))
            {
                var outFolder = protoFile.Replace(rootDir + Path.DirectorySeparatorChar, "")
                                         .Split(Path.DirectorySeparatorChar)
                                         .Where(o => !o.EndsWith(".proto"))
                                         .Select(FirstLetterToUpper)
                                         .Aggregate(outDir, Path.Combine);

                if (!Directory.Exists(outFolder))
                    Directory.CreateDirectory(outFolder);
                var cmd = new List<string>
                {
                    "-I" + rootDir,
                    "-I" + thirdPartyDir,
                    "--csharp_out",
                    outFolder,
                    "--grpc_out",
                    outFolder,
                    "--plugin=protoc-gen-grpc=" + grpcPlugin,
                    protoFile
                };
                var info = new ProcessStartInfo
                {
                    FileName = protoc,
                    Arguments = string.Join(" ", cmd),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Console.Write("Running command: {0} {1} -> ", info.FileName, info.Arguments);
                var process = Process.Start(info);
                if (process == null)
                    Console.WriteLine("Failed to create process");
                else
                {
                    process.WaitForExit();
                    var stdOut = process.StandardOutput.ReadToEnd().Trim();
                    if (!string.IsNullOrWhiteSpace(stdOut))
                        Console.Write(stdOut);
                    var stdErr = process.StandardError.ReadToEnd().Trim();
                    if (!string.IsNullOrWhiteSpace(stdErr))
                        Console.Write(stdOut);
                    Console.WriteLine("ExitCode = {0}", process.ExitCode);
                }
            }
        }

        private static string GetSuffix()
        {
            return IntPtr.Size == 8 ? "x64" : "x86";
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
