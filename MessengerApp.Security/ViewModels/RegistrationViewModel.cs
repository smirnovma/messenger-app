using MessengerApp.BLL.Security;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerApp.Security.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {

        private string _login;
        private string _password;
        private string _confirmPassword;
        private SecurityLogic _securityBL;
        public DelegateCommand RegistrationCommand { get; private set; }
        public string Login
        {
            get { return _login; }
            set
            {
                if (value != _login)
                {
                    _login = value;
                    OnPropertyChanged("Login");

                    RegistrationCommand.RaiseCanExecuteChanged();
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

                    RegistrationCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (value != _confirmPassword)
                {
                    _confirmPassword = value;
                    OnPropertyChanged("ConfirmPassword");

                    RegistrationCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Window Window { get; set; }
        public RegistrationViewModel()
        {
            _securityBL = new SecurityLogic();
            RegistrationCommand = new DelegateCommand(ExecuteLoginCommand, () => ValidateLogin() == null && ValidatePassword() == null);
        }

        private void ExecuteLoginCommand()
        {
            string result = _securityBL.Register(Login, Password);
            Window.Close();
            MessageBox.Show(result);
        }

        private object ValidatePassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return "Password can not be empty.";
            }
            else if (Password != ConfirmPassword)
            {
                return "Passwords do not match.";
            }
            return null;
        }

        private object ValidateLogin()
        {
            return string.IsNullOrEmpty(Login) ? "Username can not be empty." : null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
