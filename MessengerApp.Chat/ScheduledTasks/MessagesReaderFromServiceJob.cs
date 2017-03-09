using MessengerApp.BLL.Chat;
using MessengerApp.BLL.Security;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Chat.ScheduledTasks
{
    public class MessagesReaderFromServiceJob : IJob
    {
        private ChatLogic _chatBL;
        private SecurityLogic _securityBL;
        private List<string> _userList;

        private string _currentUserName;
        private string CurrentUserName => _currentUserName ?? (_currentUserName = _securityBL.GetUserInfo()["Email"]);

        public MessagesReaderFromServiceJob()
        {
            _chatBL = new ChatLogic();
            _securityBL = new SecurityLogic();
            _userList = new List<string>();
            _userList.AddRange(_chatBL.GetUsers().Where(p => p.Name != CurrentUserName).Select(p => p.Name));
        }

        public void Execute(IJobExecutionContext context)
        {
            foreach (var item in _userList)
            {
                _chatBL.SaveChattingDataFromServiceInLocalDb(item);
            }
        }
    }
}
