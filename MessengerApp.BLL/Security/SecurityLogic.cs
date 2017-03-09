using MessengerApp.BLL.Shared;
using MessengerApp.Data;
using MessengerApp.Entities.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Security
{
    public class SecurityLogic : AuthClient
    {
        public string Register(string email, string password)
        {
            var registerModel = new
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(App_Path + "/api/Account/Register", registerModel).Result;
                return response.StatusCode.ToString();
            }
        }

        public Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ),
                    new KeyValuePair<string, string>( "username", userName ),
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(App_Path + "/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }        

        // получаем информацию о клиенте 
        public Dictionary<string, string> GetUserInfo()
        {
            using (var client = CreateClient(Token))
            {
                var response = client.GetAsync(App_Path + "/api/Account/UserInfo").Result;
                var result = response.Content.ReadAsStringAsync().Result;

                Dictionary<string, string> currentUserInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return currentUserInfo;
            }
        }

        // обращаемся по маршруту api/values 
        public string GetValues(string token)
        {
            using (var client = CreateClient(token))
            {
                var response = client.GetAsync(App_Path + "/api/values").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public void SaveToken(string token = "")
        {
            using (var unitOfWork = new UnitOfWork())
            {
                foreach (var item in unitOfWork.SessionKeyRepository.GetEntities())
                {
                    unitOfWork.SessionKeyRepository.Delete(item.Id);
                }
                unitOfWork.SessionKeyRepository.Create(new SessionKeyEntity() { Value = token });
                unitOfWork.Save();
            }
        }
    }
}
