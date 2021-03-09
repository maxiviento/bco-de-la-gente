using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Infraestructura.Core.CiDi.OAuth
{
    public class TokenRequest
    {
        private TokenRequest()
        {
        }

        public string GrantType { get; set; }
        public string CidiHash { get; set; }
        public string ClientId { get; set; }
       

        public static TokenRequest From(IOwinRequest request)
        {
            var tokenRequest = new TokenRequest();
            var content = request.ReadFormAsync().Result;
            
            tokenRequest.GrantType = content["grant_type"];
            tokenRequest.ClientId = content["client_id"];
            tokenRequest.CidiHash = request.Cookies["CiDi"];

            return tokenRequest;
        }
    }
}