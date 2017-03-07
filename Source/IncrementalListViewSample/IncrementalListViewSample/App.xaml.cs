using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IncrementalListViewSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(new NavigationPage(new IncrementalListViewPage()) { Title = "Normal" });
            tabbedPage.Children.Add(new NavigationPage(new GroupedIncrementalListViewPage()) { Title = "Grouped" });

            MainPage = tabbedPage;
        }
    }
}
