using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Common.Services.Identity
{
    public class AuthorizeRequest
    {
        private readonly Uri _authorizeEndpoint;

        public AuthorizeRequest(string authorizeEndpoint)
        {
            _authorizeEndpoint = new Uri(authorizeEndpoint);
        }

        public string Create(IDictionary<string, string> values)
        {
            var queryString = string.Join("&", values.Select(kvp =>
                $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}").ToArray());
            return $"{_authorizeEndpoint.AbsoluteUri}?{queryString}";
        }
    }
}