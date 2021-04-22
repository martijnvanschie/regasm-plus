using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.RegAsmPlus
{
    internal class ExitCodeManager
    {
        private static List<ExitCodeDescription> _exitCodes = new List<ExitCodeDescription>();

        internal static ExitCodeDescription GetDescription(int exitCode)
        {
            return _exitCodes.FirstOrDefault(d => d.ExitCode == exitCode);
        }

        static ExitCodeManager()
        {
            _exitCodes.Add(new ExitCodeDescription(ExitCode.SUCCES, "Succesfull."));
            _exitCodes.Add(new ExitCodeDescription(ExitCode.ARGUMENTS_INVALID_AMOUNT, "Invalid amount of arguments provided."));
            _exitCodes.Add(new ExitCodeDescription(ExitCode.ARGUMENT_BAD_INPUT, "Unable to process arguments. Bad arguments."));
            _exitCodes.Add(new ExitCodeDescription(ExitCode.ARGUMENT_ASSEMBLY_NOT_FOUND, "Invalid argument. Input assembly not found."));
        }
    }

    internal class ExitCodeDescription
    {
        internal int ExitCode;
        internal string Message;
        internal bool IsFormatter = false;

        public ExitCodeDescription(int exitCode, string message, bool isFormatter = false)
        {
            this.ExitCode = exitCode;
            this.Message = message;
            this.IsFormatter = isFormatter;
        }
    }

    internal static class ExitCode
    {
        // 0 Success 
        public const int SUCCES = 0;

        // 1 - 99 Input Errors
        public const int ARGUMENTS_INVALID_AMOUNT = 100;
        public const int ARGUMENT_BAD_INPUT = 101;
        public const int ARGUMENT_ASSEMBLY_NOT_FOUND = 102;

        // 100 - 199 Process Errors

        // 200-299 Internal Errors
    }
}
