using PDFimg.Helpers;
using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PDFimg.ViewModels.Dialogs
{
    internal class ImgDataViewModel : BindableBase, IDialogAware
    {
        public ImgDataViewModel()
        {
            ImgData = new ImgDataModel() { Guid = Guid.NewGuid() };
        }

        private ImgDataModel _imgData = default!;
        public ImgDataModel ImgData
        {
            get { return _imgData!; }
            set { SetProperty(ref _imgData, value); }
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

        // Button to save image data.
        private ICommand? _saveImageData;
        public ICommand SaveImageData
        {
            get => _saveImageData ?? (_saveImageData = new DelegateCommand(ExecuteSaveImageData, CanExecuteSaveImageData)
                .ObservesProperty(() => ImgData.Name)
                .ObservesProperty(() => ImgData.PathToImage)
                .ObservesProperty(() => ImgData.PageNumbers));
        }

        // Save image data.
        private void ExecuteSaveImageData()
        {
            var buttonResult = ButtonResult.OK;

            // Create callback parameters.
            var parameters = new DialogParameters();
            parameters.Add("ImgData", ImgData);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        private bool CanExecuteSaveImageData()
        {
            if (String.IsNullOrEmpty(ImgData.Name) || string.IsNullOrEmpty(ImgData.PathToImage) || string.IsNullOrEmpty(ImgData.PageNumbers))
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
                ImgData.PathToImage = dialog.FileName;

                // Get selected file name.
                var fileName = Path.GetFileName(ImgData.PathToImage);
                PathToImage = fileName.CutString(30);
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

            // If the image data is being updated.
            var imgData = parameters.GetValue<ImgDataModel>("ImgData");
            if (imgData != null)
            {
                // Make a copy the object.
                ImgData = imgData.DeepCopy();

                // Get selected file name.
                var fileName = Path.GetFileName(ImgData.PathToImage);
                PathToImage = fileName.CutString(30);
            }
        }
    }
}
