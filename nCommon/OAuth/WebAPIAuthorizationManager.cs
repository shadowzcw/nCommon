using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;

namespace nCommon.OAuth
{
    public class WebAPIAuthorizationManager
    {
        public static IPrincipal VerifyOAuth2(HttpRequestMessage requestMessage, params string[] requiredScopes)
        {
            // for this sample where the auth server and resource server are the same site,
            // we use the same public/private key.
            try
            {
                using (RSACryptoServiceProvider authorizationRas = AuthorizationServerHelper.CreateAuthorizationServerSigningServiceProvider())
                {
                    using (RSACryptoServiceProvider resourceRas = ResourceServerHelper.CreateResourceServerEncryptionServiceProvider())
                    {
                        var resourceServer = new ResourceServer(new StandardAccessTokenAnalyzer(authorizationRas, resourceRas));
                        return resourceServer.GetPrincipal(requestMessage);//, requiredScopes  这一版先不验证scope
                    }
                }
            }
            catch (ProtocolFaultResponseException ex)//处理password auth方式授权过期，暂时没找合适的处理入口
            {
                return null;
            }
        }
    }
}
