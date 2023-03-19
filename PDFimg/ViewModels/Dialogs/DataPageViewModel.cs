using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace PDFimg.ViewModels.Dialogs
{
    internal class DataPageViewModel : BindableBase, IDialogAware
    {
        public string Title => "Add new data page";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
