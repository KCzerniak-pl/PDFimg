using PDFimg.Helpers;
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
        public DataPageViewModel()
        {
            DataPage = new DataPageModel();
        }

        private DataPageModel _dataPage = default!;
        public DataPageModel DataPage
        {
            get { return _dataPage!; }
            set { SetProperty(ref _dataPage, value); }
        }

        private string _pathToImage = "Click the button to select a image.";
        public string PathToImage
        {
            get { return _pathToImage; }
            set { SetProperty(ref _pathToImage, value); }
        }

        // Button to open the file browser dialog.
        private ICommand? _fileBrowserDialog;
        public ICommand FileBrowserDialog { get => _fileBrowserDialog ?? (_fileBrowserDialog = new DelegateCommand(ExecuteFileBrowserDialog)); }

        // Button to save data page.
        private ICommand? _saveDataPage;
        public ICommand SaveDataPage
        {
            get => _saveDataPage ?? (_saveDataPage = new DelegateCommand(ExecuteSaveDataPage, CanExecuteSaveDataPage)
                .ObservesProperty(() => DataPage.Name)
                .ObservesProperty(() => DataPage.PathToImage)
                .ObservesProperty(() => DataPage.PageNumbers));
        }

        // Save data page.
        private void ExecuteSaveDataPage()
        {
            var buttonResult = ButtonResult.OK;

            // Create callback parameters.
            var parameters = new DialogParameters();
            parameters.Add("DataPage", DataPage);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        private bool CanExecuteSaveDataPage()
        {
            if (String.IsNullOrEmpty(DataPage.Name) || string.IsNullOrEmpty(DataPage.PathToImage) || string.IsNullOrEmpty(DataPage.PageNumbers))
            {
                return false;
            }
            return true;
        }

        // File browser dialog.
        private void ExecuteFileBrowserDialog()
        {
            // Create a file browser dialog using the 'Ookii Dialogs'.
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dialog.Filter = "Image files (jpg, jpeg, png, gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                // Get path to selected file.
                DataPage.PathToImage = dialog.FileName;
                string.IsNullOrEmpty(DataPage.PathToImage);
                PathToImage = DataPage.PathToImage.CutString(30);
            }
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
            var dataPage = parameters.GetValue<DataPageModel>("DataPage");
            if (dataPage != null)
            {
                DataPage = dataPage;
                PathToImage = DataPage.PathToImage.CutString(30);
            }
        }
    }
}
