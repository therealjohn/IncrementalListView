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
    public class GroupedIncrementalViewModel : INotifyPropertyChanged, ISupportIncrementalLoading
    {
        int counter;
        int max = 200;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PageTypeGroup> MyItems { get; set; }

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

        public GroupedIncrementalViewModel()
        {
            MyItems = new ObservableCollection<PageTypeGroup>();

            LoadMoreItemsCommand = new Command(async () => await LoadMoreItems());

            HasMoreItems = true;
        }

        async Task LoadMoreItems()
        {
            if (counter >= max)
                return;

            IsLoadingIncrementally = true;

            // Simulate a long running operation
            await Task.Delay(1000);

            int end = counter + PageSize;
            var newGroup = new PageTypeGroup($"{counter} - {end - 1}", "");
            for (; counter < end; counter++)
            {
                newGroup.Add(counter.ToString());
            }

            MyItems.Add(newGroup);

            // artificial way to end ability to load more items.
            HasMoreItems = counter < max;

            IsLoadingIncrementally = false;
        }
    }

    public class PageTypeGroup : List<string>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public PageTypeGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }

        public static IList<PageTypeGroup> All { private set; get; }
    }
}
