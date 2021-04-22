using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DevTools.RegAsmPlus
{
    class Program
    {
        private static string assemblyCodeBase;
        static void Main(string[] args)
        {
            Console.WriteLine("RegAsm Plus");
            ValidateArguments(args);

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyResolver);

            Console.WriteLine($"Registering assembly [{args[0]}].");
            Assembly asm = Assembly.LoadFile(args[0].ToString());

            Console.WriteLine($"Assembly codebase [{asm.Location}]");
            assemblyCodeBase = Path.GetDirectoryName(asm.Location);

            RegistrationServices regAsm = new RegistrationServices();

            try
            {
                bool bResult = regAsm.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);

                var message = bResult ? $"Assembly registered successfully." : $"Assembly not registered. No eligable types found in assembly.";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static Assembly MyResolver(object sender, ResolveEventArgs args)
        {
            Console.WriteLine($"Unable to resolve [{args.Name}].");
            AppDomain domain = (AppDomain)sender;

            var assemblyName = args.Name.Split(',')[0];
            var assemmblyPath = Path.Combine(assemblyCodeBase, $"{assemblyName}.dll");

            Console.WriteLine($"Loading assembly [{assemmblyPath}].");

            var bytes = loadFile(assemmblyPath);
            Assembly assembly = domain.Load(bytes);
            return assembly;
        }

        static byte[] loadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }

        static void ValidateArguments(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    ExitWithCode(ExitCode.ARGUMENTS_INVALID_AMOUNT);
                }

                if (!File.Exists(args[0].ToString()))
                {
                    ExitWithCode(ExitCode.ARGUMENT_ASSEMBLY_NOT_FOUND);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExitWithCode(ExitCode.ARGUMENT_BAD_INPUT);
            }
        }

        static void ExitWithCode(int code)
        {
            Console.WriteLine(ExitCodeManager.GetDescription(code).Message);
            Environment.Exit(ExitCodeManager.GetDescription(code).ExitCode);
        }
    }
}
