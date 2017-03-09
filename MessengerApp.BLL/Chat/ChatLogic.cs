using MessengerApp.BLL.Models;
using MessengerApp.BLL.Security;
using MessengerApp.BLL.Shared;
using MessengerApp.Data;
using MessengerApp.Entities.Chat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Chat
{
    public class ChatLogic : AuthClient
    {
        private string _currentUserName;
        public string CurrentUserName => _currentUserName ?? (_currentUserName = new SecurityLogic().GetUserInfo()["Email"]);

        public void SendMessage(string userNameTo, string message)
        {
            using (var client = CreateClient(Token))
            {
                string dataJson = JsonConvert.SerializeObject(new { UserNameTo = userNameTo, MessageText = message });
                HttpContent content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var response = client.PostAsync(App_Path + "/api/values", content).Result;
            }
        }
        public IEnumerable<UserInfo> GetUsers()
        {
            using (var client = CreateClient(Token))
            {
                var response = client.GetAsync(App_Path + "/api/Account/UserInfoAll").Result;
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<UserInfo>>(result);
            }
        }

        public IEnumerable<MessageEntity> GetChattingDataFromLocalDb(string userNameWith)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                return unitOfWork.MessageRepository.GetEntities().Where(p => 
                (p.UserLoginFrom == CurrentUserName && p.UserLoginTo == userNameWith) || 
                (p.UserLoginFrom == userNameWith && p.UserLoginTo == CurrentUserName))
                .ToList();
            }
        }

        public void SaveChattingDataFromServiceInLocalDb(string userNameWith)
        {
            using (var client = CreateClient(Token))
            {
                DateTime date = DateTime.MinValue;
                using (var unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.MessageRepository.GetEntities().Where(p => (p.UserLoginFrom == CurrentUserName && p.UserLoginTo == userNameWith) || (p.UserLoginFrom == userNameWith && p.UserLoginTo == CurrentUserName)).Count() != 0)
                        date = unitOfWork.MessageRepository.GetEntities().Where(p => (p.UserLoginFrom == CurrentUserName && p.UserLoginTo == userNameWith) || (p.UserLoginFrom == userNameWith && p.UserLoginTo == CurrentUserName)).Select(p => p.Date).Max();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.GetAsync(App_Path + string.Format("/api/values/Get?userNameTo={0}&dateFrom={1}", userNameWith, date.ToString("dd.MM.yyyy HH:mm:ss.fff"))).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var messagesList = JsonConvert.DeserializeObject<List<MessageEntity>>(result);

                        foreach (var item in messagesList)
                        {
                            unitOfWork.MessageRepository.Create(item);
                        }
                        unitOfWork.Save();
                    }
                    else
                    {
                        throw new Exception("Fail request");
                    }
                }
            }
        }
        public string GenerateChatText(string userNameWith)
        {
            if (userNameWith == null) return string.Empty; 
            var messagesList = GetChattingDataFromLocalDb(userNameWith);
            if (messagesList.Count() == 0) return string.Empty;
            StringBuilder chatText = new StringBuilder();
            foreach (var item in messagesList.OrderBy(p => p.Date))
            {
                chatText.Append(string.Format("[{0} {1}]\n{2}\n", item.Date.ToString("dd.MM.yyyy HH:mm"), item.UserLoginFrom, item.Message));
            }
            return chatText.ToString();
        }
    }
}
