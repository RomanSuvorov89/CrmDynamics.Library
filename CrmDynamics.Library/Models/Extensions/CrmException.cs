using System;
using System.Runtime.Serialization;

namespace CrmDynamics.Library.Models.Extensions
{
    internal class CrmException : Exception
    {
        [DataMember(Name = "error")]
        public Error Error { get; set; }
        public CrmException(string message, Exception inner) : base(message, inner) { }
    }

    public class Innererror
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "stacktrace")]
        public string Stacktrace { get; set; }
    }

    public class Error
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "innererror")]
        public Innererror Innererror { get; set; }
    }
}
