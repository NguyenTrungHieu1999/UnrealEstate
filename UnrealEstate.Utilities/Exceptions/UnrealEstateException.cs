using System;
using System.Runtime.Serialization;

namespace UnrealEstate.Utilities.Exceptions
{
    public class UnrealEstateException : Exception
    {
        public UnrealEstateException()
        {
        }

        public UnrealEstateException(string message) : base(message)
        {
        }

        public UnrealEstateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnrealEstateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
