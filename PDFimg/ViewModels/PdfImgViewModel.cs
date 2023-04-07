using PDFimg.Helpers;
using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace PDFimg.ViewModels
{
    class PdfImgViewModel : BindableBase
    {
        // Dialog service.
        private readonly IDialogService _dialogService;

        public PdfImgViewModel(IDialogService dialogService)
        {
            // Dependency injection using 'Prism DryIoc'.
            _dialogService = dialogService;

            PdfData = new PdfDataModel();

            GetJsonFilesWithPdfData();
        }

        private string _pathToJsonFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "json");
        public string PathToJsonFiles
        {
            get { return _pathToJsonFiles; }
            set { SetProperty(ref _pathToJsonFiles, value); }
        }

        private List<JsonDataModel> _jsonFilesCollection = default!;
        public List<JsonDataModel> JsonFilesCollection
        {
            get { return _jsonFilesCollection; }
            set { SetProperty(ref _jsonFilesCollection, value); }
        }

        private JsonDataModel _selectedJsonFile = default!;
        public JsonDataModel SelectedJsonFile
        {
            get { return _selectedJsonFile; }
            set
            {
                SetProperty(ref _selectedJsonFile, value);
                GetDataFromJsonFile(SelectedJsonFile.Guid);
            }
        }

        private PdfDataModel _pdfData = default!;
        public PdfDataModel PdfData
        {
            get { return _pdfData; }
            set { SetProperty(ref _pdfData, value); RaisePropertyChanged(); }
        }

        private string _pathToFolder = "Click the button to select a folder containing PDF files.";
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { SetProperty(ref _pathToFolder, value); }
        }

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

                // Add PDF from directory to collection.
                PdfData.Files = Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly).ToList();

                // Count elements in collection.
                PdfData.CountFiles = PdfData.Files.Count();

                SetEnabledAddImagesToPdf();
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
            SetEnabledAddImagesToPdf();
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
                        SetEnabledAddImagesToPdf();
                    }
                }
            });
        }

        // Add images to PDF.
        private void ExecuteAddImagesToPdf()
        {
            AddImageToPdf.Execute(PdfData.Files, PdfData.ImgData);
        }

        // Get JSON files with data about PDF.
        private void GetJsonFilesWithPdfData()
        {
            try
            {
                if (Directory.Exists(PathToJsonFiles))
                {
                    JsonFilesCollection = new List<JsonDataModel>();

                    // Add JSON file from directory to collection.
                    IEnumerable<string> files = Directory.EnumerateFiles(PathToJsonFiles, "*.json", SearchOption.TopDirectoryOnly);

                    foreach (string file in files)
                    {
                        try
                        {
                            string jsonFile = File.ReadAllText(file);
                            JsonDataModel jsonData = JsonSerializer.Deserialize<JsonDataModel>(jsonFile)!;
                            JsonFilesCollection.Add(jsonData);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Cannot read JSON file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot read JSON files.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Get data from chosen JSON file.
        private void GetDataFromJsonFile(Guid guid)
        {
            string path = Path.Combine(PathToJsonFiles, $"{guid}.json");

            if (File.Exists(path))
            {
                PdfData = new PdfDataModel();

                try
                {
                    // Get data from JSON file.
                    string jsonFile = File.ReadAllText(path);
                    PdfData = JsonSerializer.Deserialize<PdfDataModel>(jsonFile)!;

                    // Add PDF from directory to collection.
                    PdfData.Files = Directory.EnumerateFiles(PdfData.PathToFolder, "*.pdf", SearchOption.TopDirectoryOnly).ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Cannot read chosen JSON file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Create short path.
                PathToFolder = PdfData.PathToFolder.CutString(55);

                // Count elements in collection.
                PdfData.CountFiles = PdfData.Files.Count();

                SetEnabledAddImagesToPdf();
            }
        }

        // Set the toggle to enable/disable the button for adding images to a PDF.
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
