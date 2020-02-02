using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int numOfActivatedMainWindow = 0;
        void App_start(object sender, StartupEventArgs e)
        {
            var bl = FactoryBl.GetInstance;
            Welcome window = new Welcome();
            window.Show();
        }
    }
}
