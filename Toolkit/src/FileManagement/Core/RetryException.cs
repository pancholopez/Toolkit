using System;

namespace FileManagement.Core
{
    [Serializable()]
    public class RetryException : Exception
    {
        public RetryException() { }
        public RetryException(string message) : base(message) { }
        public RetryException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected RetryException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}