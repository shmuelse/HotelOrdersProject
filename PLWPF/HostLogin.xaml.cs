using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using BE;
using BL;
using Utilities;


namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Host_contact.xaml
    /// </summary>
    public partial class Host_contact : Window
    {
        private readonly IBL _instance = FactoryBl.GetInstance;

        public static List<BankBranch> BankBrunchesList = new List<BankBranch>();
        IEnumerable<BankBranch> bankAccunts;

        Host hostWindow;



        public Host_contact()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            SystemCommands.MaximizeWindow(this);



            Gender.ItemsSource = Enum.GetValues(typeof(enums.Gender));

            hostWindow = new Host
            {

                HostInfo = new Contact
                {
                    FirstName = null,
                    LastName = null,
                    PhoneNumber = null,
                    Gender = enums.Gender.Male,
                    EmailAddress = null,
                    DateOfBirth = default,
                    SystemRegistrationDate = default,
                    LoginDetails = new Login
                    {
                        UserName = null,
                        Password = null
                    }
                },
                Id = null,
                BankBranchDetails = new BankBranch
                {
                    BankNumber = null,
                    BankName = null,
                    BranchNumber = null,
                    BranchAddress = new Address
                    {
                        Street = null,
                        Building = null,
                        City = null,
                        ZipCode = null,
                        Country = null
                    },
                    BranchPhoneNumber = null,
                    BranchFaxNumber = null
                },
                BankAccountNumber = null,
                CollectionClearance = false,
                SumOfUnits = 0,
                HostAccommodationsList = null
            };

            GridHostLoginDetails.DataContext = hostWindow;


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

        #region validations

        //First name
        private void FirstName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text.Length == 0 || !FirstName.Text.All(x => x == ' ' || char.IsLetter(x)) ||
                !Utilities.Validation.IsValidName(FirstName.Text))
                FirstName.Background = Brushes.Red;
            else
                FirstName.BorderBrush = Brushes.Green;
        }

        private void FirstName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            FirstName.Background = Brushes.White;
            FirstName.BorderBrush = Brushes.Gray;
        }

        //last name
        private void LastName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (LastName.Text.Length == 0 || !LastName.Text.All(x => x == ' ' || char.IsLetter(x)) ||
                !Utilities.Validation.IsValidName(LastName.Text))
                LastName.Background = Brushes.Red;
            else
                LastName.BorderBrush = Brushes.Green;
        }

        private void LastName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LastName.Background = Brushes.White;
            LastName.BorderBrush = Brushes.Gray;
        }

        //phone number
        private void Phone_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!PhoneNumber.Text.All(char.IsDigit) || (PhoneNumber.Text.Length != 10) ||
                !Utilities.Validation.IsValidPhoneNumber(PhoneNumber.Text))
                PhoneNumber.Background = Brushes.Red;
            else
                PhoneNumber.BorderBrush = Brushes.Green;
        }

        private void Phone_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PhoneNumber.Background = Brushes.White;
            PhoneNumber.BorderBrush = Brushes.Gray;
        }

        //email
        private void Email_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!EmailAddress.Text.All(char.IsDigit) || (EmailAddress.Text.Length < 6) ||
                !Utilities.Validation.IsValidEmail(EmailAddress.Text))
                EmailAddress.Background = Brushes.Red;
            else
                EmailAddress.BorderBrush = Brushes.Green;
        }

        private void Email_OnGotFocus(object sender, RoutedEventArgs e)
        {
            EmailAddress.Background = Brushes.White;
            EmailAddress.BorderBrush = Brushes.Gray;
        }

        private void IdNum_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (IdNum.Text.All(char.IsDigit) || Utilities.Validation.IsValidId(IdNum.Text))
                IdNum.Background = Brushes.Red;
            else
                IdNum.BorderBrush = Brushes.Green;
        }

        private void IdNum_OnGotFocus(object sender, RoutedEventArgs e)
        {
            IdNum.Background = Brushes.White;
            IdNum.BorderBrush = Brushes.Gray;
        }

        #region UserName_password

        private void UserName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Length == 0 || !UserName.Text.All(x => x == ' ' || char.IsLetter(x)) ||
                !Utilities.Validation.IsValidUserName(UserName.Text))
                UserName.Background = Brushes.Red;
            else
                UserName.BorderBrush = Brushes.Green;
        }

        private void UserName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            UserName.Background = Brushes.White;
            UserName.BorderBrush = Brushes.Gray;
        }

        //password
        private void PasswordBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Utilities.Validation.IsValidPassword(Password.Password))
                Password.Background = Brushes.Red;
            else
                Password.BorderBrush = Brushes.Green;
        }

        private void PasswordBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Password.Background = Brushes.White;
            Password.BorderBrush = Brushes.Gray;
        }

        //confirm
        private void ConfirmPassword_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Utilities.Validation.IsValidPassword(PasswordConfirm.Password) ||
                Password.Password != PasswordConfirm.Password)
                PasswordConfirm.Background = Brushes.Red;
            else
                PasswordConfirm.BorderBrush = Brushes.Green;
        }

        private void ConfirmPassword_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordConfirm.Background = Brushes.White;
            PasswordConfirm.BorderBrush = Brushes.Gray;
        }

        #endregion


        #endregion

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (FirstName.Background != Brushes.White && FirstName.BorderBrush != Brushes.Gray
                || LastName.Background != Brushes.White && LastName.BorderBrush != Brushes.Gray
                || PhoneNumber.Background != Brushes.White && PhoneNumber.BorderBrush != Brushes.Gray
                || EmailAddress.Background != Brushes.White && EmailAddress.BorderBrush != Brushes.Gray
                || UserName.Background != Brushes.White && UserName.BorderBrush != Brushes.Gray
                || Password.Background != Brushes.White && Password.BorderBrush != Brushes.Gray
                || PasswordConfirm.Background != Brushes.White && PasswordConfirm.BorderBrush != Brushes.Gray
                || IdNum.Background != Brushes.White && IdNum.BorderBrush != Brushes.Gray
                || Gender.SelectedIndex == -1
                || DateOfBirth.Text.Length == 0
                || BankName.SelectedIndex == -1
                || BankNum.SelectedIndex == -1
                || BranchNum.SelectedIndex == -1
                || AccountNumber.Text.Length <= 3)
            {
                MessageBox.Show("Some details you entered are incorrect, please try again", "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);
                return;
            }


            if (DirectDebitSigned.IsChecked == false)
            {
                MessageBox.Show("You must sign on Direct Debit to continue", "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);
                return;
            }



            try
            {
                _instance.AddHostContactInfo(hostWindow.Clone());
                MessageBox.Show("Your registration has been successfully completed");


                var newAccommodation = new AccomUnitUpdateWindow(hostWindow.Id);
                newAccommodation.Show();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }


        private void DateBirth_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateOfBirth.DisplayDateStart = DateTime.Now.AddYears(-80);
            DateOfBirth.DisplayDateEnd = DateTime.Now.AddMonths(0);
            DateOfBirth.BlackoutDates.Clear();
        }
    }
}


