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
using System.Windows.Shapes;
using BE;
using BL;
using Style = BE.enums.Style;
using Type = BE.enums.Type;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for NewGuestRequest.xaml
    /// </summary>
    public partial class NewGuestRequest : Window
    {
        private readonly IBL _instance = FactoryBl.GetInstance;


        /// <summary>
        ///     the LoginDetails
        /// </summary> 
        private readonly Login _login = new Login();

        public GuestRequest _guestRequest;
        #region C-tor

        public NewGuestRequest(Login login)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();
            _guestRequest = new GuestRequest();
            SystemCommands.MaximizeWindow(this);

            for (var i = 0; i < 9; i++)
            {
                adults.Items.Add(i + 1);
                child.Items.Add(i);
            }


            AccomoType.ItemsSource = Enum.GetValues(typeof(Type));
            roomType.ItemsSource = Enum.GetValues(typeof(Style));

            area.ItemsSource = Enum.GetValues(typeof(enums.Area));


            _login.UserName = login.UserName;
            _login.Password = login.Password;




        }


        #endregion

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

        #region Buttons

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button chooseButton = sender as Button;
            if (chooseButton.Content == FindResource("Maybe"))
            {
                chooseButton.Content = FindResource("True");
            }
            else if (chooseButton.Content == FindResource("True"))
            {
                chooseButton.Content = FindResource("False");
            }
            else
            {
                chooseButton.Content = FindResource("Maybe");
            }
        }


        private void Send_OnClick(object sender, RoutedEventArgs e)
        {

            if (area.SelectedIndex == -1 || AccomoType.SelectedIndex == -1 || roomType.SelectedIndex == -1 ||
                adults.SelectedIndex == -1 || child.SelectedIndex == -1 || checkInTimeDatePicker.Text.Length == 0 ||
                Check_Out_Date.Text.Length == 0)
            {
                MessageBox.Show("You must enter all the details", "Failed", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None);
                return;
            }

            if (checkInTimeDatePicker.SelectedDate >= Check_Out_Date.SelectedDate)
            {
                MessageBox.Show("Check in Date must Start before check out date", "Failed", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.None);
                return;
            }

            var guestRequest = new GuestRequest();

            guestRequest.SpecificRequirements.Breakfast = brrackfast.Content.ToString();
            guestRequest.SpecificRequirements.Lunch = lunch.Content.ToString();
            guestRequest.SpecificRequirements.Dinner = dinner.Content.ToString();
            guestRequest.SpecificRequirements.TwinBeds = tweenB.Content.ToString();
            guestRequest.SpecificRequirements.DoubleBed = doublB.Content.ToString();
            guestRequest.SpecificRequirements.BabyCrib = badyCrib.Content.ToString();
            guestRequest.SpecificRequirements.Bathtub = bathub.Content.ToString();
            guestRequest.SpecificRequirements.PrivateBathroom = privetBath.Content.ToString();
            guestRequest.SpecificRequirements.RoomService = roomSer.Content.ToString();
            guestRequest.SpecificRequirements.WashingMachine = laundryS.Content.ToString();
            guestRequest.SpecificRequirements.Jacuzzi = jacuzzi.Content.ToString();
            guestRequest.SpecificRequirements.Pool = pool.Content.ToString();
            guestRequest.SpecificRequirements.Spa = spa.Content.ToString();
            guestRequest.SpecificRequirements.Gym = gym.Content.ToString();
            guestRequest.SpecificRequirements.Terrace = terrace.Content.ToString();
            guestRequest.SpecificRequirements.Garden = garden.Content.ToString();
            guestRequest.SpecificRequirements.ChildrenAttractions = childrenAt.Content.ToString();
            guestRequest.SpecificRequirements.AirConditioning = airCond.Content.ToString();
            guestRequest.SpecificRequirements.WiFi = wifi.Content.ToString();
            guestRequest.SpecificRequirements.Tv = tv.Content.ToString();


            guestRequest.AmountOfAdults = (uint)adults.SelectedItem;
            guestRequest.AmountOfChildren = (uint)child.SelectedItem;
            guestRequest.Area = (enums.Area)area.SelectedItem;
            guestRequest.SubArea = (enums.Districts)subArea.SelectedItem;
            guestRequest.TypeOfAccommodationRequested = (System.Type)AccomoType.SelectedItem;
            guestRequest.StyleOfUnitRequested = (Style)roomType.SelectionBoxItem;
            guestRequest.CheckInDate = checkInTimeDatePicker.DisplayDate;
            guestRequest.CheckOutDate = Check_Out_Date.DisplayDate;
            guestRequest.ClientLoginDetails = _login;



            //guestRequest.GuestRequestKey
            //guestRequest.OrderStatus
            try
            {
                _instance.AddACustomerRequirement(guestRequest);
                MessageBox.Show(
                    "Your booking request has been received successfully, vacation offers will be sent to you soon ...");
                var welcome = new Welcome();
                welcome.Show();
                Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Failed", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.None);
            }
        }

        #endregion



        private void Check_in_date_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            checkInTimeDatePicker.DisplayDateStart = DateTime.Now.AddDays(-1);
            checkInTimeDatePicker.DisplayDateEnd = DateTime.Now.AddMonths(11);
            checkInTimeDatePicker.BlackoutDates.Clear();

            //Blackout all the dates in the past and in more that 11 month
            checkInTimeDatePicker.BlackoutDates.AddDatesInPast();
            checkInTimeDatePicker.BlackoutDates.Add(
                new CalendarDateRange(DateTime.Now.AddMonths(11), DateTime.MaxValue));

            //Check_Out_Date
            Check_Out_Date.DisplayDateStart = DateTime.Now.AddDays(2);
            Check_Out_Date.DisplayDateEnd = DateTime.Now.AddMonths(11);
            Check_Out_Date.BlackoutDates.Clear();

            //Blackout all the dates in the past and in more that 11 month
            Check_Out_Date.BlackoutDates.AddDatesInPast();
            Check_Out_Date.BlackoutDates.Add(
                new CalendarDateRange(DateTime.Now.AddMonths(11), DateTime.MaxValue));
        }

        private void area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (area.SelectedIndex != -1)
            {
                subArea.Items.Clear();
                switch ((int)area.SelectedItem)
                {
                    case 1:
                        subArea.Items.Add(enums.Districts.Jerusalem);
                        break;
                    case 2:
                        subArea.Items.Add(enums.Districts.Tzfat);
                        subArea.Items.Add(enums.Districts.Kinneret);
                        subArea.Items.Add(enums.Districts.Yizreel);
                        subArea.Items.Add(enums.Districts.Akko);
                        subArea.Items.Add(enums.Districts.Golan);
                        break;
                    case 3:
                        subArea.Items.Add(enums.Districts.Haifa);
                        subArea.Items.Add(enums.Districts.Hadera);
                        break;
                    case 4:
                        subArea.Items.Add(enums.Districts.Sharon);
                        subArea.Items.Add(enums.Districts.PetahTikva);
                        subArea.Items.Add(enums.Districts.Ramla);
                        subArea.Items.Add(enums.Districts.Rehovot);
                        break;
                    case 5:
                        subArea.Items.Add(enums.Districts.TelAviv);

                        break;
                    case 6:
                        subArea.Items.Add(enums.Districts.Ashkelon);
                        subArea.Items.Add(enums.Districts.BeerSheva);
                        break;
                    case 7:
                        subArea.Items.Add(enums.Districts.JudeaAndSamaria);
                        break;
                }

            }
        }
    }
}