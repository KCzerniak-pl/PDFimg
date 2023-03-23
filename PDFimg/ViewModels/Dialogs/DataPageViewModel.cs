using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PDFimg.ViewModels.Dialogs
{
    internal class DataPageViewModel : BindableBase, IDialogAware
    {
        private string _name = default!;
        public string Name
        {
            get { return _name!; }
            set { SetProperty(ref _name, value); }
        }

        private string _pathToImageFull = default!;
        public string PathToImageFull
        {
            get { return _pathToImageFull; }
            set { SetProperty(ref _pathToImageFull, value); }
        }

        private string _pathToImageShort = "Click the button to select a image.";
        public string PathToImageShort
        {
            get { return _pathToImageShort; }
            set { SetProperty(ref _pathToImageShort, value); }
        }

        private string _pageNumbers = default!;
        public string PageNumbers
        {
            get { return _pageNumbers; }
            set { SetProperty(ref _pageNumbers, value); }
        }

        private int _positionX = default!;
        public int PositionX
        {
            get { return _positionX; }
            set { SetProperty(ref _positionX, value); }
        }

        private int _positionY = default!;
        public int PositionY
        {
            get { return _positionY; }
            set { SetProperty(ref _positionY, value); }
        }

        // Button to open the file browser dialog.
        private ICommand? _fileBrowserDialog;
        public ICommand FileBrowserDialog { get => _fileBrowserDialog ?? (_fileBrowserDialog = new DelegateCommand(ExecuteFileBrowserDialog)); }

        // Button to save data page.
        private ICommand? _saveDataPage;
        public ICommand SaveDataPage { get => _saveDataPage ?? (_saveDataPage = new DelegateCommand(ExecuteSaveDataPage, CanExecuteSaveDataPage)
                .ObservesProperty(() => Name).ObservesProperty(() => PathToImageFull).ObservesProperty(() => PageNumbers)); }

        // Save data page.
        private void ExecuteSaveDataPage()
        {
            var buttonResult = ButtonResult.OK;

            // Create callback parameters.
            var parameters = new DialogParameters();
            var dataPage = new DataPageModel()
            {
                Name = this.Name,
                PathToImage = this.PathToImageFull,
                PageNumbers = this.PageNumbers,
                PositionX = this.PositionX,
                PositionY = this.PositionY
            };
            parameters.Add("DataPage", dataPage);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        private bool CanExecuteSaveDataPage()
        {
            if (String.IsNullOrEmpty(Name) || string.IsNullOrEmpty(PathToImageFull) || string.IsNullOrEmpty(PageNumbers))
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
                int maxChar = 30;
                PathToImageFull = dialog.FileName;
                PathToImageShort = PathToImageFull.Length > maxChar ? $"{PathToImageFull.Substring(0, maxChar)}..." : PathToImageFull;
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
        }
    }
}
