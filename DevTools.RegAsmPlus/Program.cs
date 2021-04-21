using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        private static string assemblyCodeBase;
        static void Main(string[] args)
        {
            Console.WriteLine("Start process");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MyResolver);

            Assembly asm = Assembly.LoadFile(args[0].ToString());

            assemblyCodeBase = Path.GetDirectoryName(asm.Location);

            RegistrationServices regAsm = new RegistrationServices();

            try
            {
                bool bResult = regAsm.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);
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
    }
}
