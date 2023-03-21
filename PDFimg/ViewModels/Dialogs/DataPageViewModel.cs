using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace PDFimg.ViewModels.Dialogs
{
    internal class DataPageViewModel : BindableBase, IDialogAware
    {
        public DataPageModel DataPage { get; set; }

        public DataPageViewModel()
        {
            DataPage = new DataPageModel();
        }

        // Button to save data page.
        private ICommand? _saveDataPage;
        public ICommand SaveDataPage { get => _saveDataPage ?? (_saveDataPage = new DelegateCommand(ExecuteSaveDataPage)); }

        // Save data page.
        private void ExecuteSaveDataPage()
        {
            var buttonResult = ButtonResult.OK;

            var parameters = new DialogParameters();
            parameters.Add("DataPage", DataPage);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        public string Title { get; set; } = default!;

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }
    }
}
