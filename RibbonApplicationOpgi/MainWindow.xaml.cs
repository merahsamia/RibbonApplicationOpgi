using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RibbonApplicationOpgi.UserControls;
using RibbonApplicationOpgi.Windows;

namespace RibbonApplicationOpgi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            PostulantUC postulantUc = new PostulantUC();
            PostulantTab.Content = postulantUc;
            BenificiereUC benificiereUc = new BenificiereUC();
            BenificierTab.Content = benificiereUc;
        }

        private void ribbonControl_SelectedPageChanged(object sender, DevExpress.Xpf.Ribbon.RibbonPropertyChangedEventArgs e)
        {
            if (ribbonControl.SelectedPage == PostulantRibbonPage)
            {
                tabControl.SelectedItem = PostulantTab;
            }
            if (ribbonControl.SelectedPage == BenificierRibbonPage)
            {
                tabControl.SelectedItem = BenificierTab;
            }
            if (ribbonControl.SelectedPage == ProjetRibbonPage)
            {
                tabControl.SelectedItem = ProjetTab;
            }
            if (ribbonControl.SelectedPage == ReglageRibbonPage)
            {
                tabControl.SelectedItem = ReglageTab;
            }
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ProgrammeWind programmeWind = new ProgrammeWind();
            programmeWind.ShowDialog();
        }
    }
}
