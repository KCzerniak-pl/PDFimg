using PDFimg.Helpers;
using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

            PdfFiles = new ObservableCollection<string>();
            // Add a delegate to the event associated with collection item count change.
            PdfFiles.CollectionChanged += ContentCollectionChanged;
        }

        // Path to the selected folder.
        private string _pathToFolderFull = default!;
        public string PathToFolderFull
        {
            get { return _pathToFolderFull; }
            set { SetProperty(ref _pathToFolderFull, value); }
        }

        private string _pathToFolderShort = "Click the button to select a folder containing PDF files.";
        public string PathToFolderShort
        {
            get { return _pathToFolderShort; }
            set { SetProperty(ref _pathToFolderShort, value); }
        }

        // Collection with PDF.
        private ObservableCollection<string> _pdfFiles = default!;
        public ObservableCollection<string> PdfFiles
        {
            get { return _pdfFiles; }
            set { SetProperty(ref _pdfFiles, value); }
        }

        // Total files in the selected folder.
        private string _countFiles = "0";
        public string CountFiles
        {
            get { return _countFiles; }
            set { SetProperty(ref _countFiles, value); }
        }

        // Collection of PDF files.
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
                int maxChar = 55;
                PathToFolderFull = dialog.SelectedPath;
                PathToFolderShort = PathToFolderFull.Length > maxChar ? $"{PathToFolderFull.Substring(0, maxChar)}..." : PathToFolderFull;

                // Add PDF to collection.
                PdfFiles.AddRange(from file in Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly) select file);

                // Count elements in collection.
                CountFiles = PdfFiles.Count().ToString();
            }
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
            AddImageToPdf.Execute(PdfFiles.ToList(), DataPage.ToList());
        }

        // Delegate related to updating the collection.
        private void ContentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (PdfFiles != null && PdfFiles.Any() && DataPage != null && DataPage.Any())
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
