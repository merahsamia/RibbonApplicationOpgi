using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using RibbonApplicationOpgi.Windows;

namespace RibbonApplicationOpgi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { static App()
        {
            var splashScreenViewModel = new DXSplashScreenViewModel() { Title = "OPGI Application" };
            SplashScreenManager.Create(() => new SplashScreen1(), splashScreenViewModel).ShowOnStartup();
        }
    }
}
