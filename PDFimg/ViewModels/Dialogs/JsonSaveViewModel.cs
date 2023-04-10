using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows.Input;
using System;

namespace PDFimg.ViewModels.Dialogs
{
    internal class JsonSaveViewModel : BindableBase, IDialogAware
    {
        private string _name = default!;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        // Button to save data.
        private ICommand? _saveDataCommand;
        public ICommand SaveDataCommand
        {
            get => _saveDataCommand ?? (_saveDataCommand = new DelegateCommand(ExecuteSaveData, CanExecuteSaveData).ObservesProperty(() => Name));
        }

        // Save data.
        private void ExecuteSaveData()
        {
            var buttonResult = ButtonResult.OK;

            // Create callback parameters.
            var parameters = new DialogParameters();
            parameters.Add("Name", Name);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        private bool CanExecuteSaveData()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
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
