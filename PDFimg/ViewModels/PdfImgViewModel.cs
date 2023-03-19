using PDFimg.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
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

        // Total files in the selected folder.
        private string _countFiles = "0";
        public string CountFiles
        {
            get { return _countFiles; }
            set { SetProperty(ref _countFiles, value); }
        }

        // Collection of data for chosen pages in PDF files.
        private ObservableCollection<DataPageModel> _dataPage = default!;
        public ObservableCollection<DataPageModel> DataPage
        {
            get { return _dataPage; }
            set { SetProperty(ref _dataPage, value); }
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

        // Folder browser dialog.
        private void ExecuteFolderBrowserDialog()
        {
            // Create a folder browser dialog using 'Ookii Dialogs'
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                // Get path to selected folder.
                int maxChar = 55;
                PathToFolderFull = dialog.SelectedPath;
                PathToFolderShort = PathToFolderFull.Length > maxChar ? $"{PathToFolderFull.Substring(0, maxChar)}..." : PathToFolderFull;

                // Count files (with chosen extension) in the selected folder.
                int countFiles = (from file in Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly) select file).Count();
                CountFiles = countFiles.ToString();
            }
        }

        // Remove chosen data from collection.
        void ExecuteRemoveItemCommand(DataPageModel dataPage)
        {
            DataPage.Remove(dataPage);
        }

        // Data page dialog.
        private void ExecuteAddDataPageDialog()
        {
            _dialogService.ShowDialog("DataPageDialog");
        }
    }
}
