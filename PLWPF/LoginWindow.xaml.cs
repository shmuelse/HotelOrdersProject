using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;
using BE;
using BL;
using Utilities;

namespace PLWPF
{

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IBL _instance = FactoryBl.GetInstance;
        public Login LoginExist;
        public static Customer CustomerWindow;

        public LoginWindow()
        {

            this.LoginExist = null;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            SystemCommands.MaximizeWindow(this);

            LoginExist = new Login
            {
                UserName = null,
                Password = null
            };
            LoginWindowGrid.DataContext = LoginExist;

        }

        #region close_min_max_Windo

        //MinimizeWindow
        private void Button_Click_MinimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        //MaximizeWindow
        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        //CloseWindow
        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Mouse
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Width *= 1.1;
            ((Button)sender).Height *= 1.1;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Width /= 1.1;
            ((Button)sender).Height /= 1.1;
        }

        #endregion

        #region signIn

        private void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            if (
                TextBoxUserName.Background != Brushes.White && TextBoxUserName.BorderBrush != Brushes.Gray
                || PasswordBoxInLoginWindow.Background != Brushes.White && PasswordBoxInLoginWindow.BorderBrush != Brushes.Gray 
                ||TextBoxUserName.Text.Length == 0
                || PasswordBoxInLoginWindow.Password.Length == 0)
            {
                MessageBox.Show("Some details you entered are incorrect, please try again", "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);
                return;
            }

            try
            {

                LoginExist.Password = PasswordBoxInLoginWindow.Password;
                if (_instance.IsUsernameMatchPassword(LoginExist, "Host"))
                {
                    AccomUnitUpdateWindow accomUnitUpdateWindow =
                        new AccomUnitUpdateWindow(_instance.GetHostKey(LoginExist));
                    accomUnitUpdateWindow.Show();
                    this.Close();
                }
                else if (_instance.IsUsernameMatchPassword(LoginExist, "SiteOwner"))
                {
                    SiteOwnerPanel siteOwnerPanel = new SiteOwnerPanel(LoginExist);
                    siteOwnerPanel.Show();
                    this.Close();
                }
                else if (_instance.IsUsernameMatchPassword(LoginExist, "Customer"))
                {
                    var newGuestRequest = new NewGuestRequest(LoginExist);
                    newGuestRequest.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Some details you entered are incorrect, please try again", "Failed",
                        MessageBoxButton.OK, MessageBoxImage.Information,
                        MessageBoxResult.None);
                    return;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);
                return;
            }



        }


        #endregion


        #region signUP


        private void RegisterAsAHost_OnClick(object sender, RoutedEventArgs e)
        {
            Host_contact hostContact = new Host_contact();
            hostContact.Show();
            this.Close();
        }

        private void RegisterAsAGuest_OnClick(object sender, RoutedEventArgs e)
        {
            GuestPanel guestPanel = new GuestPanel();
            guestPanel.Show();
            this.Close();
        }


        #endregion




        #region user_name_pass

        private void TxtBx_userName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxUserName.Text.Length == 0 /*|| !Utilities.Validation.IsValidUserName(TextBoxUserName.Text)*/)
                TextBoxUserName.Background = Brushes.IndianRed;
            else
                TextBoxUserName.BorderBrush = Brushes.Green;
        }

        private void TxtBx_userName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxUserName.Background = Brushes.White;
            TextBoxUserName.BorderBrush = Brushes.Gray;
        }

        private void PasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (/*!Utilities.Validation.IsValidPassword(PasswordBoxInLoginWindow.Password)*/PasswordBoxInLoginWindow.Password.Length <=5)
                PasswordBoxInLoginWindow.Background = Brushes.IndianRed;
            else
                PasswordBoxInLoginWindow.BorderBrush = Brushes.Green;
        }

        private void PasswordBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBoxInLoginWindow.Background = Brushes.White;
            PasswordBoxInLoginWindow.BorderBrush = Brushes.Gray;
        }




        #endregion


        private void Welcome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void SignUp1_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterAsAGuest.Visibility = Visibility;
            RegisterAsAHosst.Visibility = Visibility;
        }

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}