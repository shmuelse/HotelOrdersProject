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
using System.Xml.Linq;
using BE;
using BL;
using Microsoft.Win32;
using Utilities;
using Style = BE.enums.Style;
using Type = BE.enums.Type;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AccomUnitUpdateWindow.xaml
    /// </summary>
    public partial class AccomUnitUpdateWindow : Window
    {

        private readonly IBL _instance = FactoryBl.GetInstance;

        public static List<Address> AddressesList = new List<Address>();
        public static List<Accommodations> AccommodationToBeUpdated = new List<Accommodations>();
        // Accommodations accommodations = new Accommodations();


        public string HostId;

        //private readonly string HostId;


        public AccomUnitUpdateWindow(string hostId)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();

            SystemCommands.MaximizeWindow(this);

            HostId = hostId;

            TypeOfAccommodation.ItemsSource = Enum.GetValues(typeof(Type));
            StyleOfAccommodation.ItemsSource = Enum.GetValues(typeof(Style));

            for (var i = 0; i <= 7; i++) { Stars.Items.Add(i); }

            XElement AddressesRoot = XElement.Load(@"..\..\..\xml files\Adressess.xml");
            try
            {
                AddressesList = (from item in AddressesRoot.Elements()
                                 select new Address()
                                 {
                                     Street = item.Element("street_name").Value.ToString(),
                                     Building = null,
                                     City = item.Element("city_name").Value.ToString(),
                                     ZipCode = null,
                                     Country = "Israel"
                                 }).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sorry we not success to load Banks file");
                //Do nothing
            }

            City.ItemsSource = AddressesList;
            City.DisplayMemberPath = "City";

            // Building.ItemsSource = 
            // Area.ItemsSource = 
            Country.ItemsSource = "Israel";

            AccommodationToBeUpdated = _instance.GetAllAccommodations(x => x.HostId == HostId).ToList();

            ChooseTheAccommodation.ItemsSource = AccommodationToBeUpdated;
            ChooseTheAccommodation.DisplayMemberPath = "AccommodationName";

            NameOfAccommodationToDelete.ItemsSource = AccommodationToBeUpdated;
            NameOfAccommodationToDelete.DisplayMemberPath = "AccommodationName";

            UpdateChoosAccommodation.ItemsSource = AccommodationToBeUpdated;
            UpdateChoosAccommodation.DisplayMemberPath = "AccommodationName";


        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (City.SelectedIndex != -1)
            {
                Street.Items.Clear();

                foreach (var item in AddressesList.Where(item => item.City == City.Text))
                {
                    Street.Items.Add(item.Street);
                }
            }
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

        #region filters

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button chooseButton = sender as Button;

            chooseButton.Content = FindResource(chooseButton.Content != FindResource("True") ? "True" : "False");
        }


        #endregion

        #region Accommodation

        #region AddNewAccommodation

        //accommodation name
        private void AccommodationName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (AccommodationName.Text.Length == 0 || !AccommodationName.Text.All(x => x == ' ' || char.IsLetter(x)) || !Utilities.Validation.IsValidName(AccommodationName.Text))
                AccommodationName.Background = Brushes.Red;
            else
                AccommodationName.BorderBrush = Brushes.Green;
        }

        private void AccommodationName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AccommodationName.Background = Brushes.White;
            AccommodationName.BorderBrush = Brushes.Gray;
        }

        //sum of units
        private void SumOfUnits_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sumOfUnits.Text.Length <= 0 || !sumOfUnits.Text.All(char.IsDigit))
                sumOfUnits.Background = Brushes.Red;
            else
                sumOfUnits.BorderBrush = Brushes.Green;
        }

        private void SumOfUnits_OnGotFocus(object sender, RoutedEventArgs e)
        {
            sumOfUnits.Background = Brushes.White;
            sumOfUnits.BorderBrush = Brushes.Gray;
        }
        //zip code
        private void ZipCode_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (ZipCode.Text.Length < 4 || !ZipCode.Text.All(char.IsDigit))
                ZipCode.Background = Brushes.Red;
            else
                ZipCode.BorderBrush = Brushes.Green;
        }

        private void ZipCode_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ZipCode.Background = Brushes.White;
            ZipCode.BorderBrush = Brushes.Gray;
        }

        //Building
        private void Building_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (Building.Text.Length < 4 || !Building.Text.All(char.IsDigit))
                Building.Background = Brushes.Red;
            else
                Building.BorderBrush = Brushes.Green;
        }

        private void Building_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Building.Background = Brushes.White;
            Building.BorderBrush = Brushes.Gray;
        }


        //add accommodation
        private void AddAccommodation_OnClick(object sender, RoutedEventArgs e)
        {
            if (AccommodationName.Background != Brushes.White && AccommodationName.BorderBrush != Brushes.Gray
                || sumOfUnits.Background != Brushes.White && sumOfUnits.BorderBrush != Brushes.Gray
                || ZipCode.Background != Brushes.White && ZipCode.BorderBrush != Brushes.Gray
                || TypeOfAccommodation.SelectedIndex == -1
                || StyleOfAccommodation.SelectedIndex == -1
                || Stars.SelectedIndex == -1
                || Street.SelectedIndex == -1
                || Building.Background != Brushes.White && Building.BorderBrush != Brushes.Gray
                || Building.Text.Length == 0
                || City.SelectedIndex == -1)
            {
                MessageBox.Show("Some details you entered are incorrect, please try again", "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);

                return;
            }

            var accommodations = new Accommodations();

            accommodations.HostId = HostId;
            accommodations.AccommodationAddress.Street = Street.SelectionBoxItem.ToString();
            accommodations.AccommodationAddress.Building = Building.Text;
            accommodations.AccommodationAddress.City = City.SelectionBoxItem.ToString();
            accommodations.AccommodationAddress.Country = Country.SelectionBoxItem.ToString();
            accommodations.AccommodationAddress.ZipCode = ZipCode.Text;
            accommodations.AccommodationName = AccommodationName.Text;
            accommodations.Stars = (uint)Stars.SelectionBoxItem;
            accommodations.StyleOfAccommodation = (List<Style>)StyleOfAccommodation.SelectionBoxItem;
            accommodations.SumOfUnits = Convert.ToUInt32(sumOfUnits.Text);
            try
            {
                _instance.AddAccommodation(accommodations.Clone());
                MessageBox.Show("successfully updated");
                var welcome = new Welcome();
                welcome.Show();
                Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());

            }




        }

        #endregion

        #region UpdateAccommodation

        private void UpdateAccommodationName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (UpdateAccommodationName.Text.Length == 0 || !AccommodationName.Text.All(x => x == ' ' || char.IsLetter(x)) || !Utilities.Validation.IsValidName(UpdateAccommodationName.Text))
                UpdateAccommodationName.Background = Brushes.Red;
            else
                UpdateAccommodationName.BorderBrush = Brushes.Green;
        }

        private void UpdateAccommodationName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            UpdateAccommodationName.Background = Brushes.White;
            UpdateAccommodationName.BorderBrush = Brushes.Gray;
        }

        private void UpdateSumOfUnits_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (UpdateSumOfUnits.Text.Length <= 0 || !sumOfUnits.Text.All(char.IsDigit))
                UpdateSumOfUnits.Background = Brushes.Red;
            else
                UpdateSumOfUnits.BorderBrush = Brushes.Green;
        }

        private void UpdateSumOfUnits_OnGotFocus(object sender, RoutedEventArgs e)
        {
            UpdateSumOfUnits.Background = Brushes.White;
            UpdateSumOfUnits.BorderBrush = Brushes.Gray;
        }

        //Update Accommodation 
        private void UpdateAccommodation_OnClick(object sender1, RoutedEventArgs e1)
        {
            var updatedStars = new Accommodations();

            foreach (var item in AccommodationToBeUpdated)
            {
                if (item.AccommodationName == ChooseTheAccommodation.Text)
                    updatedStars = item;
            }

            if (UpdateChoosAccommodation.SelectedIndex == -1 || Stars.SelectedIndex == -1)
            {
                MessageBox.Show("Some details you entered are incorrect, please try again", "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    MessageBoxResult.None);

                return;
            }

            //int accommodationKey, string hostKey, string detailsInAccommodationUpdate,
            try
            {
                _instance.AccommodationStarsUpdate(updatedStars.AccommodationKey, HostId, Convert.ToUInt32(Stars.Text));
                MessageBox.Show("successfully updated");
                var welcome = new Welcome();
                welcome.Show();
                Close();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());

            }
        }

        #endregion

        #region DeleteAccommodation

        //delete accommodation
        void DeleteAccommodation_OnClick(object sender, RoutedEventArgs e)
        {
            if (NameOfAccommodationToDelete.SelectedIndex == -1)
            {
                MessageBox.Show("You must choose accommodation to delete");
                return;
            }

            try
            {
                foreach (var item in AccommodationToBeUpdated.Where(item =>
                    item.AccommodationName == ChooseTheAccommodation.Text))
                {
                    _instance.DeleteAccommodation(item.AccommodationKey, item.HostId);
                    var welcome = new Welcome();
                    welcome.Show();
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Failed", MessageBoxButton.OK,
                    MessageBoxImage.Error, MessageBoxResult.None);
            }

        }


        #endregion

        #endregion

        #region Unit

        #region addUnit

        private void UnitNumber_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!UnitNumber.Text.All(char.IsDigit) || (UnitNumber.Text.Length <= 0))
                UnitNumber.Background = Brushes.Red;
            else
                UnitNumber.BorderBrush = Brushes.Green;
        }

        private void UnitNumber_OnGotFocus(object sender, RoutedEventArgs e)
        {
            UnitNumber.Background = Brushes.White;
            UnitNumber.BorderBrush = Brushes.Gray;
        }

        private void UnitPrice_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!UnitPrice.Text.All(char.IsDigit) || (UnitPrice.Text.Length <= 0))
                UnitPrice.Background = Brushes.Red;
            else
                UnitPrice.BorderBrush = Brushes.Green;
        }

        private void UnitPrice_OnGotFocus(object sender, RoutedEventArgs e)
        {
            UnitPrice.Background = Brushes.White;
            UnitPrice.BorderBrush = Brushes.Gray;
        }

        //add unit
        private void AddUnit_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChoosAccommodationToAddUnit.SelectedIndex == -1
                || UnitNumber.Background != Brushes.White && UnitNumber.BorderBrush != Brushes.Gray
                || UnitPrice.Background != Brushes.White && UnitPrice.BorderBrush != Brushes.Gray)
            {
                MessageBox.Show("You must enter all the details", "Failed", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None);
                return;
            }


            var hostingUnit = new HostingUnit();

            hostingUnit.UnitOptions.Breakfast = (bool)ButtonBreakfast.Content;
            hostingUnit.UnitOptions.Lunch = (bool)ButtonLunch.Content;
            hostingUnit.UnitOptions.Dinner = (bool)ButtonDinner.Content;
            hostingUnit.UnitOptions.TwinBeds = (bool)ButtonTweenBed.Content;
            hostingUnit.UnitOptions.DoubleBed = (bool)ButtonDoubleBed.Content;
            hostingUnit.UnitOptions.BabyCrib = (bool)ButtonBadyCrib.Content;
            hostingUnit.UnitOptions.Bathtub = (bool)ButtonBathub.Content;
            hostingUnit.UnitOptions.PrivateBathroom = (bool)ButtonPrivetBathroom.Content;
            hostingUnit.UnitOptions.RoomService = (bool)ButtonRoomServis.Content;
            hostingUnit.UnitOptions.WashingMachine = (bool)ButtonLaundyservices.Content;
            hostingUnit.UnitOptions.Jacuzzi = (bool)UpdateButtonJacuzzi.Content;
            hostingUnit.UnitOptions.Pool = (bool)UpdateButtonPool.Content;
            hostingUnit.UnitOptions.Spa = (bool)UpdateButtonSpa.Content;
            hostingUnit.UnitOptions.Gym = (bool)UpdateButtonGym.Content;
            hostingUnit.UnitOptions.Terrace = (bool)UpdateButtonTerrece.Content;
            hostingUnit.UnitOptions.Garden = (bool)UpdateButtonGarden.Content;
            hostingUnit.UnitOptions.ChildrenAttractions = (bool)UpdateButtonChildrenAttraction.Content;
            hostingUnit.UnitOptions.AirConditioning = (bool)UpdateButtonAirConditioning.Content;
            hostingUnit.UnitOptions.WiFi = (bool)UpdateButtonWifi.Content;
            hostingUnit.UnitOptions.Tv = (bool)UpdateButtonTv.Content;

            hostingUnit.HostId = HostId;
            hostingUnit.UnitNumber = UnitNumber.Text;
            hostingUnit.UnitPrice = Convert.ToDecimal(UnitPrice.Text);
            foreach (var item in AccommodationToBeUpdated)
            {
                if (item.AccommodationName == ChoosAccommodationToAddUnit.Text)
                {
                    hostingUnit.AccommodationKey = item.AccommodationKey;
                    break;
                }
            }
            try
            {
                _instance.AddAHostingUnit(hostingUnit);
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
        /**/

        #endregion

        #region updateUnit

        private void UpdateUnitPrice_OnLostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateUnitPrice_OnGotFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //update
        private void UpdateUnit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteUnit

        //delete unit
        private void DeleteUnit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region loadImge

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }


        #endregion

        private void ChooseTheAccommodation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ChooseTheAccommodation.SelectedIndex != -1)
            {
                foreach (var item in AccommodationToBeUpdated)
                {
                    if (item.AccommodationName == ChooseTheAccommodation.Text)
                    {
                        AccommodationName.Text = ChooseTheAccommodation.Text;
                        TypeOfAccommodation.Items.Add(item.TypeOfAccommodation);
                        foreach (var item2 in item.StyleOfAccommodation)
                        {
                            StyleOfAccommodation.Items.Add(item2.ToString());
                        }

                        sumOfUnits.Text = item.SumOfUnits.ToString();
                        Street.Items.Add(item.AccommodationAddress.Street.ToString());
                        City.Items.Add(item.AccommodationAddress.City.ToString());
                        ZipCode.Text = item.AccommodationAddress.ZipCode;
                        Country.Items.Add("Israel");


                    }
                }

            }
        }


    }

}
