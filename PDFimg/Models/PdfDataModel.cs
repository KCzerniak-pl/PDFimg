using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PDFimg.Models
{
    public class PdfDataModel : BindableBase
    {
        public PdfDataModel()
        {
            Files = new List<string>();
            ImgData = new ObservableCollection<ImgDataModel>();
        }

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

        private ObservableCollection<ImgDataModel> _imgData = default!;
        public ObservableCollection<ImgDataModel> ImgData
        {
            get { return _imgData; }
            set { _imgData = value; RaisePropertyChanged(); }
        }
    }
}
