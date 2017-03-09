﻿using MessengerApp.Interfaces.Security;
using MessengerApp.Security.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessengerApp.Security.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl, ILoginView
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Window = this;
            DataContext = viewModel;
        }

        public bool ShowLoginDialog()
        {

            
            throw new NotImplementedException();
        }
    }
}
