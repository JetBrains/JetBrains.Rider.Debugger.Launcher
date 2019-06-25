using System;
using System.Diagnostics;

namespace JetBrains.Debugger.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine(
                    $"Missing arguments in command string: {string.Join(", ", args)}. Expected: 5, got: {args.Length}. " +
                    "Required set or arguments: debuggerRuntimePath, debuggerAssemblyPath, debuggerMode, frontendPort, backendPort");
                return;
            }

            Console.WriteLine("Arguments passed to application: " + Environment.NewLine +
                              string.Join(Environment.NewLine, args));

            var debuggerRuntimePath = args[0];
            var debuggerAssemblyPath = args[1];
            var debuggerMode = $"--mode={args[2]}";
            var frontendPort = $"--frontend-port={args[3]}";
            var backendPort = $"--backend-port={args[4]}";

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = debuggerRuntimePath,
                    Arguments = $"{debuggerAssemblyPath} {debuggerMode} {frontendPort} {backendPort}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                var process = Process.Start(processInfo);

                Debug.Assert(process != null, nameof(process) + " != null");
                while (process != null && !process.StandardOutput.EndOfStream)
                {
                    Console.WriteLine(process.StandardOutput.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
