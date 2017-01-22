### IncremementalListView Plugin for Xamarin.Forms

IncrementalListView Plugin for Xamarin.Forms which provides an IncrementalListView control for loading data incrementally as a user scrolls.

### Where to get it?

You can clone or download the repo and build it yourself. Or, grab it from [Nuget](https://www.nuget.org/packages/IncrementalListView.FormsPlugin/).

### How to Use

To get started, add a reference to the `IncrementalListView.FormsPlugin.dll` in your PCL project (directly or using NuGet). The plugin is a single PCL and it's not required for every platform project unless using a Shared Assets Project type. 

Next, implement the `ISupportIncrementalLoading` interface on your ViewModel that will be the `BindingContext` of the `IncrementalListViewControl`. Here is an example:

```
public class IncrementalViewModel : INotifyPropertyChanged, ISupportIncrementalLoading
{
    public int PageSize { get; set; } = 20;

    public ICommand LoadMoreItemsCommand { get; set; }

    public bool IsLoadingIncrementally { get; set; } 

    public bool HasMoreItems { get; set; }

    public IncrementalViewModel()
    {
        LoadMoreItemsCommand = new Command(async () => await LoadMoreItems());
    }

    async Task LoadMoreItems()
    {
        IsLoadingIncrementally = true;

        // Download data from a service, etc.
        // Add the newly download data to a collection

        HasMoreItems = ...

        IsLoadingIncrementally = false;
    }
}
```

Then, add a namespace in your XAML page for the plugin:

```
xmlns:plugin="clr-namespace:IncrementalListView.FormsPlugin;assembly=IncrementalListView.FormsPlugin"
```

Now, Add the control to a `ContentPage`:

```
<local:IncrementalListView
    ItemsSource="{Binding MyItems}"
    PreloadCount="5">

    <x:Arguments>
        <ListViewCachingStrategy>RecycleElement</ListViewCachingStrategy>
    </x:Arguments>

    <ListView.ItemTemplate>
        ...      
    </ListView.ItemTemplate>
    <ListView.Footer>
        <ActivityIndicator Margin="20" IsRunning="{Binding IsLoadingIncrementally}" IsVisible="{Binding IsLoadingIncrementally}"/>        
    </ListView.Footer>
</local:IncrementalListView>
```

Finally, customize the `PreloadCount` property for how close to the bottom of the list the user has to get to before a load is started. A value of `0` means the user has to reach the end of the list. In this example, the user would reach the `n - 5`, where `n` is `MyItems.Count`. 

### Feedback

Let me know if you have suggestions or feedback on this control! 

### License

See the LICENSE file.