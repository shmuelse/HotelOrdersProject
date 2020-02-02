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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        private readonly IBL _blimp = FactoryBl.GetInstance;

        public Welcome()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            App.numOfActivatedMainWindow++;
            FlowDirection = FlowDirection.LeftToRight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

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


        #region Button

        private void SignIn_OnClick(object sender, RoutedEventArgs e)
        {

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        #endregion

        private void Welcome_OnClosed(object sender, EventArgs e)
        {
            if (--App.numOfActivatedMainWindow == 0)
                Environment.Exit(Environment.ExitCode);
        }

        private void Welcome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


    }
}