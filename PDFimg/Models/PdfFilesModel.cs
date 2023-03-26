using Prism.Mvvm;
using System.Collections.Generic;

namespace PDFimg.Models
{
    public class PdfFilesModel : BindableBase
    {
        private string _pathToFolder = default!;
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { _pathToFolder = value; RaisePropertyChanged(); }
        }

        private ICollection<string> _files = default!;
        public ICollection<string> Files
        {
            get { return _files; }
            set { _files = value; RaisePropertyChanged(); }
        }

        private int _countFiles = default!;
        public int CountFiles
        {
            get { return _countFiles; }
            set { _countFiles = value; RaisePropertyChanged(); }
        }
    }
}
