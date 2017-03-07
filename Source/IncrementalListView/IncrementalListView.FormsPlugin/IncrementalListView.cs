using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace IncrementalListView.FormsPlugin
{
    /// <summary>
    /// IncrementalListView Interface
    /// </summary>
    public class IncrementalListView : ListView
    {
        // Helper to keep track of what was last visible in the list
        int lastPosition;
        IList itemsSource;
        ISupportIncrementalLoading incrementalLoading;

        public IncrementalListView(ListViewCachingStrategy cachingStrategy)
            : base(cachingStrategy)
        {
            ItemAppearing += OnItemAppearing;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                itemsSource = ItemsSource as IList;

                if(itemsSource == null)
                {
                    throw new Exception($"{nameof(IncrementalListView)} requires that {nameof(ItemsSource)} be of type IList");
                }
            }            
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(BindingContext != null)
            {
                incrementalLoading = BindingContext as ISupportIncrementalLoading;

                if (incrementalLoading == null)
                {
                    System.Diagnostics.Debug.WriteLine($"{nameof(IncrementalListView)} BindingContext does not implement {nameof(ISupportIncrementalLoading)}. This is required for incremental loading to work.");
                }
            }
        }
        void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            int position = itemsSource?.IndexOf(e.Item) ?? 0;
            int pageSize = 0;

            if (IsGroupingEnabled && position == -1)
            {
                int currentGroupIndex = 0;
                foreach(IList groups in itemsSource)
                {
                    position = groups?.IndexOf(e.Item) ?? -1;

                    if (position >= 0)
                    {
                        position += (currentGroupIndex * groups.Count);
                        pageSize = groups.Count;
                        break;
                    }

                    currentGroupIndex++;
                }
            }

            if (itemsSource != null)
            {
                // PreloadCount should never end up to be equal to itemsSource.Count otherwise
                // LoadMoreItems would not be called
                if (PreloadCount <= 0)
                    PreloadCount = 1;

                int preloadIndex = 0;

                if(IsGroupingEnabled)
                    preloadIndex = Math.Max(itemsSource.Count * pageSize - PreloadCount, 0);
                else
                    preloadIndex = Math.Max(itemsSource.Count - PreloadCount, 0);
                
                if ((position > lastPosition || (position == itemsSource.Count - 1)) && (position >= preloadIndex))
                {
                    lastPosition = position;

                    if (!incrementalLoading.IsLoadingIncrementally && !IsRefreshing && incrementalLoading.HasMoreItems)
                    {
                        LoadMoreItems();
                    }
                }
            }
        }

        void LoadMoreItems()
        {
            var command = incrementalLoading.LoadMoreItemsCommand;
            if (command != null && command.CanExecute(null))
                command.Execute(null);
        }

        /// <summary>
        /// Identifies the <see cref="PreloadCount"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PreloadCountProperty =
          BindableProperty.Create(nameof(PreloadCount), typeof(int), typeof(IncrementalListView), 0);

        /// <summary>
        /// How many cells before the end of the ListView before incremental loading should start. Defaults to 0, meaning the end of the list has to be reached before it will try to load more. This is a bindable property.
        /// </summary>
        public int PreloadCount
        {
            get { return (int)GetValue(PreloadCountProperty); }
            set { SetValue(PreloadCountProperty, value); }
        }
    }
}
