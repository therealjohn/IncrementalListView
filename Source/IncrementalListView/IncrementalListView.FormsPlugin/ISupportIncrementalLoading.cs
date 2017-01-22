using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IncrementalListView.FormsPlugin
{
    public interface ISupportIncrementalLoading
    {
        int PageSize { get; set; }

        bool HasMoreItems { get; set; }

        bool IsLoadingIncrementally { get; set; }

        ICommand LoadMoreItemsCommand { get; set; }
    }
}
