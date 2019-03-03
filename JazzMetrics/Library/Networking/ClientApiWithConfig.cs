﻿using Microsoft.Extensions.Configuration;

namespace Library.Networking
{
    /// <summary>
    /// trida pro nacteni URL serveru z appsetting.json
    /// </summary>
    public abstract class ClientApiWithConfig : ClientApi
    {
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// nacte URL serveru z appsetting.json
        /// </summary>
        /// <param name="config">config</param>
        /// <param name="controller">controller, na ktery se posila</param>
        /// <param name="jwt">jwt</param>
        public ClientApiWithConfig(IConfiguration config, string controller, string jwt) : base(controller, jwt)
        {
            Configuration = config;
            ServerUrl = config["ServerApiUrl"];
        }
    }
}