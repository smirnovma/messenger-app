using MessengerApp.BLL.Chat;
using MessengerApp.BLL.Security;
using MessengerApp.Chat.ScheduledTasks;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace MessengerApp.Chat.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        ChatLogic _chatBL;
        SecurityLogic _securityBL;
        Timer _timer;
        public ChatViewModel()
        {
            SendCommand = new DelegateCommand(ExecuteSendCommand);
            _chatBL = new ChatLogic();
            _securityBL = new SecurityLogic();
            _listUser = new ObservableCollection<string>();
            _listUser.AddRange(_chatBL.GetUsers().Where(p => p.Name != CurrentUserName).Select(p => p.Name).ToList());
            JobScheduler.Start();
            _timer = new Timer();
            _timer.Interval = 2000;
            _timer.Elapsed += (sender, args) => ChatText = _chatBL.GenerateChatText(_selectedUserName);
            _timer.Start();
        }

        public DelegateCommand SendCommand { get; private set; }

        private string _currentUserName;
        public string CurrentUserName {
            get
            {
                return _currentUserName ?? (CurrentUserName = _securityBL.GetUserInfo()["Email"]);
            }
            set
            {
                _currentUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<string> _listUser;
        public ObservableCollection<string> ListBoxUserName
        {
            get
            {
                return _listUser ?? new ObservableCollection<string>();
            }
            set
            {
                _listUser = value;
                OnPropertyChanged("ListBoxUserName");
            }
        }

        private string _selectedUserName;
        public string SelectedUserName
        {
            get
            {
                return _selectedUserName;
            }
            set
            {
                _selectedUserName = value;
                OnPropertyChanged("SelectedUserName");
            }
        }
        private string _chatText;
        public string ChatText
        {
            get
            {
                return _chatText;
            }
            set
            {
                _chatText = value;
                OnPropertyChanged("ChatText");
                BooleanViewModelPropertyThatTriggersScroll = true;
            }
        }

        private string _inputChatText;
        public string InputChatText
        {
            get
            {
                return _inputChatText;
            }
            set
            {
                _inputChatText = value;
                OnPropertyChanged("InputChatText");
            }
        }
        private bool _booleanViewModelPropertyThatTriggersScroll;
        public bool BooleanViewModelPropertyThatTriggersScroll
        {
            get
            {
                return _booleanViewModelPropertyThatTriggersScroll;
            }
            set
            {
                _booleanViewModelPropertyThatTriggersScroll = value;
                OnPropertyChanged("BooleanViewModelPropertyThatTriggersScroll");
            }
        }
        private void ExecuteSendCommand()
        {
            _chatBL.SendMessage(SelectedUserName, InputChatText);
            InputChatText = "";
        }
    }
}
