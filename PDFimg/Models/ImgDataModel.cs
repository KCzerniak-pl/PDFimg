using PDFimg.Interfaces;
using Prism.Mvvm;
using System;

namespace PDFimg.Models
{
    public class ImgDataModel : BindableBase, IDeepCopy<ImgDataModel>
    {
        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
            init { SetProperty(ref _guid, value); }
        }

        private string _name = default!;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _pathToImage = default!;
        public string PathToImage
        {
            get { return _pathToImage; }
            set { SetProperty(ref _pathToImage, value); }
        }

        private string _pageNumbers = default!;
        public string PageNumbers
        {
            get { return _pageNumbers; }
            set { SetProperty(ref _pageNumbers, value); }
        }

        private int _positionX;
        public int PositionX
        {
            get { return _positionX; }
            set { SetProperty(ref _positionX, value); }
        }

        private int _positionY;
        public int PositionY
        {
            get { return _positionY; }
            set { SetProperty(ref _positionY, value); }
        }

        // Make a deep copy of the current object.
        public ImgDataModel DeepCopy()
        {
            return new ImgDataModel
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
