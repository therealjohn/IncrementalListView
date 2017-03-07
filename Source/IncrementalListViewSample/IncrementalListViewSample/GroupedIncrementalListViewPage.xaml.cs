using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncrementalListViewSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupedIncrementalListViewPage : ContentPage
    {
        public GroupedIncrementalListViewPage()
        {
            InitializeComponent();
            BindingContext = new GroupedIncrementalViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as GroupedIncrementalViewModel;

            vm.LoadMoreItemsCommand.Execute(null);
        }
    }
}
