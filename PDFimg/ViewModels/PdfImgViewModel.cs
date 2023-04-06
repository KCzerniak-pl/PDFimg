using PDFimg.Helpers;
using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PDFimg.ViewModels
{
    class PdfImgViewModel : BindableBase
    {
        // Dialog service.
        private readonly IDialogService _dialogService;

        public PdfImgViewModel(IDialogService dialogService)
        {
            // Dependency injection from Prism DryIoc.
            _dialogService = dialogService;

            PdfData = new PdfDataModel();

            // Add a delegate to the event associated with property change.
            PdfData.PropertyChanged += ContentPropertyChanged;

            // Add a delegate to the event associated with collection item count change.
            PdfData.ImgData.CollectionChanged += ContentCollectionChanged;
        }

        // PDF and image data.
        private PdfDataModel _pdfData = default!;
        public PdfDataModel PdfData
        {
            get { return _pdfData; }
            set { SetProperty(ref _pdfData, value); }
        }

        // Path to the selected folder.
        private string _pathToFolder = "Click the button to select a folder containing PDF files.";
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { SetProperty(ref _pathToFolder, value); }
        }

        // Property that determines whether the add images to PDF button is enabled.
        private bool _isEnabledAddImagesToPdf = false;
        public bool IsEnabledAddImagesToPdf
        {
            get { return _isEnabledAddImagesToPdf; }
            set { SetProperty(ref _isEnabledAddImagesToPdf, value); }
        }

        // Button to open the folder browser dialog.
        private ICommand? _folderBrowserDialogCommand;
        public ICommand FolderBrowserDialogCommand { get => _folderBrowserDialogCommand ?? (_folderBrowserDialogCommand = new DelegateCommand(ExecuteFolderBrowserDialog)); }

        // button to edit chosen data from collection
        private ICommand? _editImgDataCommand;
        public ICommand EditImgDataCommand { get => _editImgDataCommand ?? (_editImgDataCommand = new DelegateCommand<ImgDataModel>(ExecuteEditImgDataCommand)); }

        // Button to remove chosen data from collection
        private ICommand? _removeImgDataCommand;
        public ICommand RemoveImgDataCommand { get => _removeImgDataCommand ?? (_removeImgDataCommand = new DelegateCommand<ImgDataModel>(ExecuteRemoveImgDataCommand)); }

        // Button to open the image data dialog.
        private ICommand? _addImgDataDialogCommand;
        public ICommand AddImgDataDialogCommand { get => _addImgDataDialogCommand ?? (_addImgDataDialogCommand = new DelegateCommand(ExecuteAddImgDataDialog)); }

        // Button to execute tasks that add images to PDF.
        private ICommand? _addImagesToPdfCommand;
        public ICommand AddImagesToPdfCommand { get => _addImagesToPdfCommand ?? (_addImagesToPdfCommand = new DelegateCommand(ExecuteAddImagesToPdf).ObservesCanExecute(() => IsEnabledAddImagesToPdf)); }

        // Folder browser dialog.
        private void ExecuteFolderBrowserDialog()
        {
            // Create a folder browser dialog using the 'Ookii Dialogs'
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                // Get path to selected folder.
                PdfData.PathToFolder = dialog.SelectedPath;
                PathToFolder = PdfData.PathToFolder.CutString(55);

                // Add PDF to collection.
                PdfData.Files = Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly).ToList();

                // Count elements in collection.
                PdfData.CountFiles = PdfData.Files.Count();
            }
        }

        // Edit chosen data from collection.
        private void ExecuteEditImgDataCommand(ImgDataModel imgData)
        {
            // Create parameters.
            var parameters = new DialogParameters();
            parameters.Add("Title", "Edit image data");
            parameters.Add("ImgData", imgData);

            // Open modal dialog.
            _dialogService.ShowDialog("ImgDataDialog", parameters, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    // Update the image data.
                    var updateImgData = callback.Parameters.GetValue<ImgDataModel>("ImgData");

                    if (imgData.Guid == updateImgData.Guid)
                    {
                        imgData.Name = updateImgData.Name;
                        imgData.PathToImage = updateImgData.PathToImage;
                        imgData.PageNumbers = updateImgData.PageNumbers;
                        imgData.PositionX = updateImgData.PositionX;
                        imgData.PositionY = updateImgData.PositionY;
                    }
                }
            });
        }

        // Remove chosen data from collection.
        private void ExecuteRemoveImgDataCommand(ImgDataModel imgData)
        {
            PdfData.ImgData.Remove(imgData);
        }

        // Image data dialog using the 'Prism'.
        private void ExecuteAddImgDataDialog()
        {
            // Create parameters.
            var parameters = new DialogParameters();
            parameters.Add("Title", "Add new image data");

            // Open modal dialog.
            _dialogService.ShowDialog("ImgDataDialog", parameters, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    // Add new item to the collection.
                    var imgData = callback.Parameters.GetValue<ImgDataModel>("ImgData");
                    if (imgData != null)
                    {
                        PdfData.ImgData.Add(imgData);
                    }
                }
            });
        }

        // Add images to PDF.
        private void ExecuteAddImagesToPdf()
        {
            AddImageToPdf.Execute(PdfData.Files, PdfData.ImgData);
        }

        // Delegate related to updating the property in object.
        private void ContentPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SetEnabledAddImagesToPdf();
        }

        // Delegate related to updating the collection.
        private void ContentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SetEnabledAddImagesToPdf();
        }

        // Set the method to enable/disable the button for adding images to a PDF.
        private void SetEnabledAddImagesToPdf()
        {
            if (PdfData.Files != null && PdfData.Files.Any() && PdfData.ImgData != null && PdfData.ImgData.Any())
            {
                IsEnabledAddImagesToPdf = true;
            }
            else
            {
                IsEnabledAddImagesToPdf = false;
            }
        }
    }
}
