using MessengerApp.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Shared
{
    public class AuthClient
    {
        private string app_path;
        private string token;
        protected string App_Path => app_path ?? (app_path = ConfigurationSettings.AppSettings.Get("WebApi"));
        protected string Token => token ?? (token = GetTokenFromLocalDb());

        // создаем http-клиента с токеном 
        protected HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        protected string GetTokenFromLocalDb()
        {
            string token = null;
            using (var unitOfWork = new UnitOfWork())
            {
                token = unitOfWork.SessionKeyRepository.GetEntities().FirstOrDefault()?.Value;
            }
            return token;
        }
    }
}
