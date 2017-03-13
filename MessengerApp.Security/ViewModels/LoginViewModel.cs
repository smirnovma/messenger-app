using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MessengerApp.BLL.Security;
using MessengerApp.BLL.Chat;
using MessengerApp.Security.Views;

namespace MessengerApp.Security.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _login;
        private string _password;

        public DelegateCommand<object> SigninCommand { get; private set; }
        public DelegateCommand RegistrationCommand { get; private set; }


        public LoginViewModel()
        {
            SigninCommand = new DelegateCommand<object>(ExecuteLoginCommand/*, (p) => ValidateLogin(p) == null && ValidatePassword(p) == null*/);
            RegistrationCommand = new DelegateCommand(ExecuteRegistrationCommand);
        }
        private void GetPassword(object param)
        {
            var passwordBox = param as PasswordBox;
            if (passwordBox == null)
                return;
            Password = passwordBox.Password;
        }
        private void ExecuteRegistrationCommand()
        {
            Window window = new Window
            {
                Title = "Registration",
                Height = 230,
                Width = 300,
                ResizeMode = ResizeMode.NoResize
            };
            window.Content = new RegistrationWindow(new RegistrationViewModel() { Window = window });
            window.ShowDialog();
        }

        //private object ValidatePassword(object param)
        //{
        //    GetPassword(param);
        //    return string.IsNullOrEmpty(Password) ? "Password can not be empty." : null;
        //}

        //private object ValidateLogin(object param)
        //{
        //    GetPassword(param);
        //    return string.IsNullOrEmpty(Login) ? "Username can not be empty." : null;
        //}

        private void ExecuteLoginCommand(object param)
        {
            try
            {
                GetPassword(param);
                SecurityLogic securityLogic = new SecurityLogic();
                Dictionary<string, string> tokenDictionary = securityLogic.GetTokenDictionary(Login, Password);
                string token = tokenDictionary.ContainsKey("access_token") ? tokenDictionary["access_token"] : null;
                if (token == null)
                {
                    throw new Exception("Could not log in.");
                }
                securityLogic.SaveToken(token);

                ((System.Windows.Window)Window.Parent).Close();
            }
            catch (Exception e)
            {
                ShowErrorMessage(e.Message);
            }
        }
        public UserControl Window { get; set; }

        public string Login
        {
            get { return _login; }
            set
            {
                if (value != _login)
                {
                    _login = value;
                    OnPropertyChanged("Login");

                    SigninCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");

                    SigninCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowErrorMessage(string text)
        {
            Window window = new Window()
            {
                Title = "Error",
                Content = text,
                Height = 100,
                Width = 300
            };
            window.ShowDialog();
        }
    }
}
