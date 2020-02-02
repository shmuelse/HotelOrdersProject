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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for SiteOwnerPanel.xaml
    /// </summary>
    public partial class SiteOwnerPanel : Window
    {
        public SiteOwnerPanel(Login loginExist)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            SystemCommands.MaximizeWindow(this);
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

        #region AccountManage

        #region Commission

        private void TheAmountOfCommission_OnLostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TheAmountOfCommission_OnGotFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Completed order details



        #endregion

        #region Offered order details



        #endregion

        #region Accommodations details



        #endregion

        #region Hosting unit details



        #endregion



        #endregion

        #region CustomerList



        #endregion

        #region Host List



        #endregion

        #region List Of All Accommodation



        #endregion

        #region List Of All Units



        #endregion

        #region Order List



        #endregion



    }
}
