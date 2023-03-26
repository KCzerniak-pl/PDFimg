using PDFimg.Interfaces;
using Prism.Mvvm;
using System;

namespace PDFimg.Models
{
    public class DataPageModel : BindableBase, IDeepCopy<DataPageModel>
    {
        public Guid Guid { get; init; }

        private string _name = default!;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(); }
        }

        private string _pathToImage = default!;
        public string PathToImage
        {
            get { return _pathToImage; }
            set { _pathToImage = value; RaisePropertyChanged(); }
        }

        private string _pageNumbers = default!;
        public string PageNumbers
        {
            get { return _pageNumbers; }
            set { _pageNumbers = value; RaisePropertyChanged(); }
        }

        private int _positionX;
        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; RaisePropertyChanged(); }
        }

        private int _positionY;
        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; RaisePropertyChanged(); }
        }

        // Make a deep copy of the current object.
        public DataPageModel DeepCopy()
        {
            return new DataPageModel
            {
                Guid = this.Guid,
                Name = this.Name,
                PathToImage = this.PathToImage,
                PageNumbers = this.PageNumbers,
                PositionX = this.PositionX,
                PositionY = this.PositionY
            };
        }
    }
}
