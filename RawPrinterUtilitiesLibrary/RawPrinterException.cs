using System;
using System.Runtime.Serialization;

namespace RawPrinterUtilities
{
    [Serializable]
    internal class RawPrinterException : Exception
    {
        public int Code { get; private set; }
        public RawPrinterException(int code = 0)
        {
            Code = code;
        }

        public RawPrinterException(string message, int code = 0) : base(message)
        {
            Code = code;
        }

        public RawPrinterException(string message, Exception innerException, int code = 0) : base(message, innerException)
        {
            Code = code;
        }

        protected RawPrinterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
