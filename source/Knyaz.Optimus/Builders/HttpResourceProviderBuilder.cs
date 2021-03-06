﻿using System;
using System.Net;
using System.Net.Http.Headers;

namespace Knyaz.Optimus.ResourceProviders
{
    /// <summary>
    /// Allows to configure and build <see cref="HttpResourceProvider"/> object.
    /// </summary>
    public class HttpResourceProviderBuilder
    {
        private WebProxy _proxy;
        private AuthenticationHeaderValue _auth;

        /// <summary>
        /// setup basic authorization login/password
        /// </summary>
        /// <returns></returns>
        public HttpResourceProviderBuilder Basic(string userName, string password)
        {
            _auth = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.Encoding.ASCII.GetBytes($"{userName}:{password}")));
            return this;
        }
        
        public HttpResourceProviderBuilder Proxy(WebProxy proxy)
        {
            _proxy = proxy;
            return this;
        }

        internal HttpResourceProvider Build()
        {
            return new HttpResourceProvider(_proxy, _auth);
        }
    }
}