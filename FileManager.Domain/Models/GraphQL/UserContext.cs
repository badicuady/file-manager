using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace FileManager.Domain.Models.GraphQL
{
    [Serializable]
    public class UserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal User { get; set; }

        public UserContext()
        {
        }

        protected UserContext(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
