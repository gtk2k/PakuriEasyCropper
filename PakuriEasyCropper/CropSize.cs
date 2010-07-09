using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace PakuriEasyCropper
{
    public class CropManager : INotifyPropertyChanged
    {
        private double borderSize;
        private Color borderColor;
        private double cropMinSize;
        private double imageWidth;
        private double imageHeight;
        private double cropImage_Left;
        private double cropImage_Top;
        private double cropCanvas_Left;
        private double cropCanvas_Top;
        private Rect cropClip;
        private double resizeHandleTopLeft_Left;
        private double resizeHandleTopLeft_Top;
        private double resizeHandleTop_Left;
        private double resizeHandleTop_Top;
        private double resizeHandleTopRight_Left;
        private double resizeHandleTopRight_Top;
        private double resizeHandleLeft_Left;
        private double resizeHandleLeft_Top;
        private double resizeHandleRight_Left;
        private double resizeHandleRight_Top;
        private double resizeHandleBottomLeft_Left;
        private double resizeHandleBottomLeft_Top;
        private double resizeHandleBottom_Left;
        private double resizeHandleBottom_Top;
        private double resizeHandleBottomRight_Left;
        private double resizeHandleBottomRight_Top;
        private double innerWidth;
        private double innerHeight;


        public double ImageWidth
        {
            get
            {
                return imageWidth;
            }
            set
            {
                imageWidth = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                return imageHeight;
            }
            set
            {
                imageHeight = value;
            }
        }

        public double BorderSize
        {
            get
            {
                return borderSize;
            }
            set
            {
                if (borderSize != value)
                {
                    borderSize = value;
                    OnPropertyChanged("BorderSize");
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    OnPropertyChanged("BorderColor");
                }
            }
        }

        public double CropMinSize
        {
            get
            {
                return cropMinSize;
            }
            set
            {
                if (cropMinSize != value)
                {
                    cropMinSize = value;
                    OnPropertyChanged("CropMinSize");
                }
            }
        }

        public double CropCanvas_Left
        {
            get
            {
                return cropCanvas_Left;
            }
            set
            {
                if (cropCanvas_Left != value)
                {
                    cropCanvas_Left = value;
                    OnPropertyChanged("CropCanvas_Left");
                }
            }
        }

        public double CropCanvas_Top
        {
            get
            {
                return cropCanvas_Top;
            }
            set
            {
                if (cropCanvas_Top != value)
                {
                    cropCanvas_Top = value;
                    OnPropertyChanged("CropCanvas_Top");
                }
            }
        }

        public Rect CropClip
        {
            get
            {
                return cropClip;
            }
            set
            {
                if (cropClip.Width != value.Width || cropClip.Height != value.Height)
                {
                    cropClip = value;
                    OnPropertyChanged("CropClip");
                }
            }
        }

        public double ResizeHandleTopLeft_Left 
        {
            get
            {
                return resizeHandleTopLeft_Left;
            }
            set
            {
                if (resizeHandleTopLeft_Left != value)
                {
                    resizeHandleTopLeft_Left = value;
                    OnPropertyChanged("ResizeHandleTopLeft_Left");
                    ResizeHandleLeft_Left = value;
                    Calc();
                }
            }
        }
        
        public double ResizeHandleTopLeft_Top
        {
            get
            {
                return resizeHandleTopLeft_Top;
            }
            set
            {
                if (resizeHandleTopLeft_Top != value)
                {
                    resizeHandleTopLeft_Top = value;
                    OnPropertyChanged("ResizeHandleTopLeft_Top");
                    ResizeHandleTop_Top = value;
                    Calc();
                }
            }
        }

        public double ResizeHandleTop_Left
        {
            get
            {
                return resizeHandleTop_Left;
            }
            set
            {
                if (resizeHandleTop_Left != value)
                {
                    resizeHandleTop_Left = value;
                    OnPropertyChanged("ResizeHandleTop_Left");
                }
            }
        }

        public double ResizeHandleTop_Top
        {
            get
            {
                return resizeHandleTop_Top;
            }
            set
            {
                if (resizeHandleTop_Top != value)
                {
                    resizeHandleTop_Top = value;
                    OnPropertyChanged("ResizeHandleTop_Top");
                    Calc();
                }
            }
        }

        public double ResizeHandleTopRight_Left
        {
            get
            {
                return resizeHandleTopRight_Left;
            }
            set
            {
                if (resizeHandleTopRight_Left != value)
                {
                    resizeHandleTopRight_Left = value;
                    OnPropertyChanged("ResizeHandleTopRight_Left");
                    ResizeHandleRight_Left = value;
                    Calc();
                }
            }
        }

        public double ResizeHandleTopRight_Top
        {
            get
            {
                return resizeHandleTopRight_Top;
            }
            set
            {
                if (resizeHandleTopRight_Top != value)
                {
                    resizeHandleTopRight_Top = value;
                    OnPropertyChanged("ResizeHandleTopRight_Top");
                    ResizeHandleTop_Top = value;
                    Calc();
                }
            }
        }

        public double ResizeHandleLeft_Left
        {
            get
            {
                return resizeHandleLeft_Left;
            }
            set
            {
                if (resizeHandleLeft_Left != value)
                {
                    resizeHandleLeft_Left = value;
                    OnPropertyChanged("ResizeHandleLeft_Left");
                    Calc();
                }
            }
        }

        public double ResizeHandleLeft_Top
        {
            get
            {
                return resizeHandleLeft_Top;
            }
            set
            {
                if (resizeHandleRight_Left != value)
                {
                    resizeHandleLeft_Top = value;
                    OnPropertyChanged("ResizeHandleLeft_Top");
                }
            }
        }

        public double ResizeHandleRight_Left
        {
            get
            {
                return resizeHandleRight_Left;
            }
            set
            {
                if (resizeHandleRight_Left != value)
                {
                    resizeHandleRight_Left = value;
                    OnPropertyChanged("ResizeHandleRight_Left");
                    Calc();
                }
            }
        }

        public double ResizeHandleRight_Top
        {
            get
            {
                return resizeHandleRight_Top;
            }
            set
            {
                if (resizeHandleRight_Top != value)
                {
                    resizeHandleRight_Top = value;
                    OnPropertyChanged("ResizeHandleRight_Top");
                }
            }
        }

        public double ResizeHandleBottomLeft_Left
        {
            get
            {
                return resizeHandleBottomLeft_Left;
            }
            set
            {
                if (resizeHandleBottomLeft_Left != value)
                {
                    resizeHandleBottomLeft_Left = value;
                    OnPropertyChanged("ResizeHandleBottomLeft_Left");
                    ResizeHandleLeft_Left = value;
                }
            }
        }

        public double ResizeHandleBottomLeft_Top
        {
            get
            {
                return resizeHandleBottomLeft_Top;
            }
            set
            {
                if (resizeHandleBottomLeft_Top != value)
                {
                    resizeHandleBottomLeft_Top = value;
                    OnPropertyChanged("ResizeHandleBottomLeft_Top");
                    ResizeHandleBottom_Top = value;
                    Calc();
                }
            }
        }

        public double ResizeHandleBottom_Left
        {
            get
            {
                return resizeHandleBottom_Left;
            }
            set
            {
                if (resizeHandleBottom_Left != value)
                {
                    resizeHandleBottom_Left = value;
                    OnPropertyChanged("ResizeHandleBottom_Left");
                }
            }
        }

        public double ResizeHandleBottom_Top
        {
            get
            {
                return resizeHandleBottom_Top;
            }
            set
            {
                if (resizeHandleBottom_Top != value)
                {
                    resizeHandleBottom_Top = value;
                    OnPropertyChanged("ResizeHandleBottom_Top");
                    Calc();
                }
            }
        }

        public double ResizeHandleBottomRight_Left
        {
            get
            {
                return resizeHandleBottomRight_Left;
            }
            set
            {
                if (resizeHandleBottomRight_Left != value)
                {
                    resizeHandleBottomRight_Left = value;
                    OnPropertyChanged("ResizeHandleBottomRight_Left");
                    ResizeHandleRight_Left = value;
                }
            }
        }

        public double ResizeHandleBottomRight_Top
        {
            get
            {
                return resizeHandleBottomRight_Top;
            }
            set
            {
                if (resizeHandleBottomRight_Top != value)
                {
                    resizeHandleBottomRight_Top = value;
                    OnPropertyChanged("ResizeHandleBottomRight_Top");
                    ResizeHandleBottom_Top = value;
                    Calc();
                }
            }
        }

        public double InnerWidth
        {
            get
            {
                return innerWidth;
            }
            set
            {
                if (value >= 0 && value <= imageWidth)
                {
                    if (innerWidth != value)
                    {
                        innerWidth = value;
                        if (innerWidth > 0)
                        {
                            OnPropertyChanged("InnerWidth");
                            if (resizeHandleLeft_Left + borderSize + value > imageWidth)
                            {
                                ResizeHandleLeft_Left = imageWidth - borderSize - value;
                            }
                            else
                            {
                                ResizeHandleRight_Left = ResizeHandleLeft_Left + borderSize + value;
                            }
                            Calc();
                        }
                    }
                }
            }
        }

        public double InnerHeight
        {
            get
            {
                return innerHeight;
            }
            set
            {
                if (innerHeight != value)
                {
                    innerHeight = value;
                    if (innerHeight > 0)
                    {
                        OnPropertyChanged("InnerHeight");
                        if (resizeHandleTop_Top + borderSize + value > imageHeight)
                        {
                            ResizeHandleTop_Top = imageHeight - borderSize - value;
                        }
                        else
                        {
                            ResizeHandleBottom_Top = resizeHandleTop_Top + borderSize + value;
                        }
                        Calc();
                    }

                }
            }
        }

        public double CropImage_Left
        {
            get
            {
                return cropImage_Left;
            }
            set
            {
                if (cropImage_Left != value)
                {
                    cropImage_Left = value;
                    OnPropertyChanged("CropImage_Left");
                }
            }
        }

        public double CropImage_Top
        {
            get
            {
                return cropImage_Top;
            }
            set
            {
                if (cropImage_Top != value)
                {
                    cropImage_Top = value;
                    OnPropertyChanged("CropImage_Top");
                }
            }
        }

        public void Move(double left, double top)
        {
            // こっち先にやらないとだめ
            ResizeHandleRight_Left = left + borderSize + innerWidth;
            ResizeHandleBottom_Top = top + borderSize + innerHeight;
            // こっちはあと
            ResizeHandleTop_Top = top;
            ResizeHandleLeft_Left = left;
            Calc();
        }

        private void Calc()
        {
            ResizeHandleTopLeft_Left = resizeHandleLeft_Left;
            ResizeHandleTopLeft_Top = resizeHandleTop_Top;
            ResizeHandleTopRight_Left = resizeHandleRight_Left;
            ResizeHandleTopRight_Top = resizeHandleTop_Top;
            ResizeHandleBottomLeft_Left = resizeHandleLeft_Left;
            ResizeHandleBottomLeft_Top = resizeHandleBottom_Top;
            ResizeHandleBottomRight_Left = resizeHandleRight_Left;
            ResizeHandleBottomRight_Top = resizeHandleBottom_Top;
            ResizeHandleTop_Left = resizeHandleLeft_Left + borderSize;
            ResizeHandleLeft_Top = resizeHandleTop_Top + borderSize;
            ResizeHandleRight_Top = resizeHandleLeft_Top;
            ResizeHandleBottom_Left = resizeHandleTop_Left;
            CropCanvas_Left = resizeHandleLeft_Left + borderSize;
            CropCanvas_Top = resizeHandleTop_Top + borderSize;
            CropImage_Left = -cropCanvas_Left;
            CropImage_Top = -cropCanvas_Top;
            InnerWidth = resizeHandleRight_Left - resizeHandleLeft_Left - borderSize;
            InnerHeight = resizeHandleBottom_Top - resizeHandleTop_Top - borderSize;
            if (innerWidth > 0 && innerHeight > 0)
            {
                CropClip = new Rect(0, 0, InnerWidth, innerHeight);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
