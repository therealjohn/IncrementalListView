using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IncrementalListViewSample
{
    public partial class IncrementalListViewPage : ContentPage
    {
        public IncrementalListViewPage()
        {
            InitializeComponent();

            BindingContext = new IncrementalViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as IncrementalViewModel;

            vm.LoadMoreItemsCommand.Execute(null);
        }
    }
}
