using PDFimg.Helpers;
using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
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

            DataPage = new ObservableCollection<DataPageModel>();
            // Add a delegate to the event associated with collection item count change.
            DataPage.CollectionChanged += ContentCollectionChanged;

            PdfFiles = new PdfFilesModel();
            // Add a delegate to the event associated with property change.
            PdfFiles.PropertyChanged += ContentPropertyChanged;
        }

        // Collection of PDF files.
        private PdfFilesModel _pdfFiles = default!;
        public PdfFilesModel PdfFiles
        {
            get { return _pdfFiles; }
            set { SetProperty(ref _pdfFiles, value); }
        }

        // Path to the selected folder.
        private string _pathToFolder = "Click the button to select a folder containing PDF files.";
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { SetProperty(ref _pathToFolder, value); }
        }

        // Collection of data page.
        internal ObservableCollection<DataPageModel> _dataPage = default!;
        public ObservableCollection<DataPageModel> DataPage
        {
            get { return _dataPage; }
            set { SetProperty(ref _dataPage, value); }
        }

        // Property that determines whether the add images to PDF button is enabled.
        private bool _isEnabledAddImagesToPdf = false;
        public bool IsEnabledAddImagesToPdf
        {
            get { return _isEnabledAddImagesToPdf; }
            set { SetProperty(ref _isEnabledAddImagesToPdf, value); }
        }

        // Button to open the folder browser dialog.
        private ICommand? _folderBrowserDialog;
        public ICommand FolderBrowserDialog { get => _folderBrowserDialog ?? (_folderBrowserDialog = new DelegateCommand(ExecuteFolderBrowserDialog)); }

        // Button to edit chosen data from collection
        private ICommand? _editDataPageCommand;
        public ICommand EditDataPageCommand { get => _editDataPageCommand ?? (_editDataPageCommand = new DelegateCommand<DataPageModel>(ExecuteEditItemCommand)); }

        // Button to remove chosen data from collection
        private ICommand? _removeDataPageCommand;
        public ICommand RemoveDataPageCommand { get => _removeDataPageCommand ?? (_removeDataPageCommand = new DelegateCommand<DataPageModel>(ExecuteRemoveItemCommand)); }

        // Button to open the data page dialog.
        private ICommand? _addDataPageDialogCommand;
        public ICommand AddDataPageDialogCommand { get => _addDataPageDialogCommand ?? (_addDataPageDialogCommand = new DelegateCommand(ExecuteAddDataPageDialog)); }

        // Button to execute tasks that add images to PDF.
        private ICommand? _addImagesToPdf;
        public ICommand AddImagesToPdf { get => _addImagesToPdf ?? (_addImagesToPdf = new DelegateCommand(ExecuteAddImagesToPdf).ObservesCanExecute(() => IsEnabledAddImagesToPdf)); }

        // Folder browser dialog.
        private void ExecuteFolderBrowserDialog()
        {
            // Create a folder browser dialog using the 'Ookii Dialogs'
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                // Get path to selected folder.
                PdfFiles.PathToFolder = dialog.SelectedPath;
                PathToFolder = PdfFiles.PathToFolder.CutString(55);

                // Add PDF to collection.
                PdfFiles.Files = Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly).ToList();

                // Count elements in collection.
                PdfFiles.CountFiles = PdfFiles.Files.Count();
            }
        }

        // Edit chosen data from collection.
        private void ExecuteEditItemCommand(DataPageModel dataPage)
        {
            // Create parameters.
            var parameters = new DialogParameters();
            parameters.Add("Title", "Edit data page");
            parameters.Add("DataPage", dataPage);

            var item = DataPage.IndexOf(dataPage);

            // Open modal dialog.
            _dialogService.ShowDialog("DataPageDialog", parameters, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    var updateDataPage = callback.Parameters.GetValue<DataPageModel>("DataPage");

                    // Find and update item in collection.
                    var dataPage = DataPage.FirstOrDefault(d => d.Guid == updateDataPage.Guid);
                    if (dataPage != null)
                    {
                        dataPage.Name = updateDataPage.Name;
                        dataPage.PathToImage = updateDataPage.PathToImage;
                        dataPage.PageNumbers = updateDataPage.PageNumbers;
                        dataPage.PositionX = updateDataPage.PositionX;
                        dataPage.PositionY = updateDataPage.PositionY;
                    }
                }
            });
        }

        // Remove chosen data from collection.
        private void ExecuteRemoveItemCommand(DataPageModel dataPage)
        {
            DataPage.Remove(dataPage);
        }

        // Data page dialog using the 'Prism'.
        private void ExecuteAddDataPageDialog()
        {
            // Create parameters.
            var parameters = new DialogParameters();
            parameters.Add("Title", "Add new data page");

            // Open modal dialog.
            _dialogService.ShowDialog("DataPageDialog", parameters, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    var dataPage = callback.Parameters.GetValue<DataPageModel>("DataPage");

                    // Add new item to the collection.
                    DataPage.Add(dataPage);
                }
            });
        }

        // Add images to PDF.
        private void ExecuteAddImagesToPdf()
        {
            AddImageToPdf.Execute(PdfFiles.Files, DataPage);
        }

        // Delegate related to updating the collection.
        private void ContentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (PdfFiles.Files != null && PdfFiles.Files.Any() && DataPage != null && DataPage.Any())
            {
                IsEnabledAddImagesToPdf = true;
            }
            else
            {
                IsEnabledAddImagesToPdf = false;
            }
        }

        // Delegate related to updating the property in object.
        private void ContentPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (PdfFiles.Files != null && PdfFiles.Files.Any() && DataPage != null && DataPage.Any())
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
