using IncrementalListView.FormsPlugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IncrementalListViewSample
{
    public class IncrementalViewModel : INotifyPropertyChanged, ISupportIncrementalLoading
    {
        int counter;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> MyItems { get; set; }

        #region ISupportIncrementalLoading Implementation

        public int PageSize { get; set; } = 20;

        public ICommand LoadMoreItemsCommand { get; set; }

        bool isLoadingIncrementally;
        public bool IsLoadingIncrementally
        {
            get { return isLoadingIncrementally; }
            set
            {
                isLoadingIncrementally = value;
                OnPropertyChanged("IsLoadingIncrementally");
            }
        }

        bool hasMoreItems;
        public bool HasMoreItems
        {
            get { return hasMoreItems; }
            set
            {
                hasMoreItems = value;
                OnPropertyChanged("HasMoreItems");
            }
        }

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IncrementalViewModel()
        {
            MyItems = new ObservableCollection<string>();

            LoadMoreItemsCommand = new Command(async () => await LoadMoreItems());

            HasMoreItems = true;
        }

        async Task LoadMoreItems()
        {
            IsLoadingIncrementally = true;

            // Simulate a long running operation
            await Task.Delay(1000);

            int end = counter + PageSize;
            for (; counter < end; counter++)
            {
                MyItems.Add(counter.ToString());
            }

            // artificial way to end ability to load more items.
            HasMoreItems = counter < 200;

            IsLoadingIncrementally = false;
        }
    }
}
