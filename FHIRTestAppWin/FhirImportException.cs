using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIRTestAppWin
{
    public class FhirImportException : Exception
    {
        public FhirImportException(string message)
            : base(message)
        {
        }
    }
}
