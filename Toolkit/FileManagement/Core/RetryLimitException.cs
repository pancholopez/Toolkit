using System;

namespace FileManagement.Core
{
    [Serializable()]
    public class RetryLimitException : Exception
    {
        public RetryLimitException() { }
        public RetryLimitException(string message) : base(message) { }
        public RetryLimitException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected RetryLimitException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}