using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using ImageTools.IO;
using ImageTools.IO.Png;
using ImageTools.IO.Jpeg;
using ImageTools.IO.Bmp;

namespace PakuriEasyCropper
{
    public partial class MainPage : UserControl
    {
        double imageWidth = 0;
        double imageHeight = 0;
        double borderSize = 4;
        Color borderColor = Colors.Red;
        double cropMinSize = 16;
        CropManager cropManager;

        public MainPage()
        {
            InitializeComponent();
            HtmlPage.Window.AttachEvent("onresize", new EventHandler<HtmlEventArgs>(BrowserWindow_Resized));
            
        }

        void BrowserWindow_Resized(object sender, HtmlEventArgs e)
        {
            this.Width = BrowserScreenInformation.ClientWidth;
            this.Height = BrowserScreenInformation.ClientHeight;
            if (double.IsNaN(stageCanvas.Width) || double.IsNaN(stageCanvas.Height))
            {
                txtInfo.SetValue(Canvas.LeftProperty, (this.Width - txtInfo.ActualWidth) / 2);
                txtInfo.SetValue(Canvas.TopProperty, (this.Height - txtInfo.ActualHeight) / 2);
            }
            else
            {
                originalSizeBorder.SetValue(Canvas.LeftProperty, (this.Width - stageCanvas.Width) / 2 + 10 - borderSize);
                originalSizeBorder.SetValue(Canvas.TopProperty, (this.Height - stageCanvas.Height) / 2 + 50 - borderSize);
            }
        } 

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BrowserWindow_Resized(null, null);
            txtInfo.SetValue(Canvas.LeftProperty, (this.Width - txtInfo.ActualWidth) / 2);
            txtInfo.SetValue(Canvas.TopProperty, (this.Height - txtInfo.ActualHeight) / 2);
        }

        Point? lp = null;
        Point? rp = null;
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lp = e.GetPosition(cropCanvas);
            cropCanvas.CaptureMouse();
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            rp = e.GetPosition((FrameworkElement)sender);
            ((FrameworkElement)sender).CaptureMouse();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (lp != null)
            {
                if (((FrameworkElement)sender).Name.StartsWith("resizeHandle"))
                {
                    Point p = e.GetPosition((FrameworkElement)sender);
                    GeneralTransform gt = LayoutRoot.TransformToVisual(cropImage);
                    Point tp = gt.Transform(new Point(0, 0));
                    Canvas rh = (Canvas)sender;
                    //Point p = e.GetPosition(rh);
                    double newX = (double)rh.GetValue(Canvas.LeftProperty) + p.X - lp.Value.X;
                    double newY = (double)rh.GetValue(Canvas.TopProperty) + p.Y - lp.Value.Y;
                    double leftMaxX = cropManager.ResizeHandleRight_Left - borderSize - cropMinSize;
                    double rightMinX = cropManager.ResizeHandleLeft_Left + borderSize + cropMinSize;
                    double topMaxY = cropManager.ResizeHandleBottom_Top - borderSize - cropMinSize;
                    double bottomMinY = cropManager.ResizeHandleTop_Top + borderSize + cropMinSize;
                    switch (rh.Name)
                    {
                        case "resizeHandleTopLeft":
                            newX = Math.Max(-borderSize, Math.Min(leftMaxX, newX));
                            newY = Math.Max(-borderSize, Math.Min(topMaxY, newY));
                            break;
                        case "resizeHandleLeft":
                            newX = Math.Max(-borderSize, Math.Min(leftMaxX, newX));
                            break;
                        case "resizeHandleTop":
                            newY = Math.Max(-borderSize, Math.Min(topMaxY, newY));
                            break;
                        case "resizeHandleTopRight":
                            newX = Math.Min(imageWidth, Math.Max(rightMinX, newX));
                            newY = Math.Max(-borderSize, Math.Min(topMaxY, newY));
                            break;
                        case "resizeHandleRight":
                            newX = Math.Min(imageWidth, Math.Max(rightMinX, newX));
                            break;
                        case "resizeHandleBottomLeft":
                            newX = Math.Max(-borderSize, Math.Min(leftMaxX, newX));
                            newY = Math.Min(imageHeight, Math.Max(bottomMinY, newY));
                            break;
                        case "resizeHandleBottom":
                            newY = Math.Min(imageHeight, Math.Max(bottomMinY, newY));
                            break;
                        case "resizeHandleBottomRight":
                            newX = Math.Min(imageWidth, Math.Max(rightMinX, newX));
                            newY = Math.Min(imageHeight, Math.Max(bottomMinY, newY));
                            break;
                    }

                    if (rh.Name != "resizeHandleTop" && rh.Name != "resizeHandleBottom")
                    {
                        rh.SetValue(Canvas.LeftProperty, newX);
                    }
                    if (rh.Name != "resizeHandleLeft" && rh.Name != "resizeHandleRight")
                    {
                        rh.SetValue(Canvas.TopProperty, newY);
                    }
                }
                else
                {
                    Point p = e.GetPosition(cropCanvas);
                    double cbx = (double)cropCanvas.GetValue(Canvas.LeftProperty) + p.X - lp.Value.X;
                    double cby = (double)cropCanvas.GetValue(Canvas.TopProperty) + p.Y - lp.Value.Y;
                    cbx = Math.Max(cbx, -borderSize);
                    cby = Math.Max(cby, -borderSize);
                    cbx = Math.Min(cbx, stageCanvas.Width - cropCanvas.Width - borderSize);
                    cby = Math.Min(cby, stageCanvas.Height - cropCanvas.Height - borderSize);
                    cropManager.Move(cbx, cby);
                }
            }
            else if (rp != null)
            {
                Point p = e.GetPosition((FrameworkElement)sender);
                originalSizeBorder.SetValue(Canvas.LeftProperty, (double)originalSizeBorder.GetValue(Canvas.LeftProperty) + p.X - rp.Value.X);
                originalSizeBorder.SetValue(Canvas.TopProperty, (double)originalSizeBorder.GetValue(Canvas.TopProperty) + p.Y - rp.Value.Y);
            }
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lp = null;
            cropCanvas.ReleaseMouseCapture();
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            rp = null;
            ((UIElement)sender).ReleaseMouseCapture();
        }

        //Point? mp = null;
        //private void resizeHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    mp = e.GetPosition((UIElement)sender);
        //    ((UIElement)sender).CaptureMouse();
        //}

        //private void resizeHandle_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (mp != null)
        //    {
                
        //    }
        //}

        //private void resizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    mp = null;
        //    ((Canvas)sender).ReleaseMouseCapture();
        //}

        private void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            var file = ((FileInfo[])e.Data.GetData(DataFormats.FileDrop))[0];
            string fn = file.Name.ToLower();
            WriteableBitmap wb = null;
            if (fn.EndsWith(".jpg") || fn.EndsWith(".png"))
            {
                BitmapImage bi = new BitmapImage();
                try
                {
                    bi.SetSource(file.OpenRead());
                    if (bi.PixelWidth < 50 || bi.PixelHeight < 50)
                    {
                        MessageBox.Show("画像サイズが小さすぎます");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("エラー");
                    return;
                }
                wb = new WriteableBitmap(bi);
                
            }
            else if (fn.EndsWith(".bmp"))
            {
                IImageDecoder dec = new BmpDecoder();
                FileStream fs = file.OpenRead();
                ImageTools.Image img = new ImageTools.Image();
                try
                {
                    dec.Decode(img, fs);
                }
                catch
                {
                    MessageBox.Show("エラー");
                    return;
                }
                if (img.Width < 50 || img.Height < 50)
                {
                    MessageBox.Show("画像サイズが小さすぎます");
                    return;
                }
                wb = new WriteableBitmap(img.Width, img.Height);
                byte[] data = img.GetPixels();
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        int idx = (x + y * img.Width) * 4;
                        wb.SetPixel(x, y, Color.FromArgb(data[idx + 3], data[idx], data[idx + 1], data[idx + 2]));
                    }
                }
                fs.Close();
            }
            else
            {
                MessageBox.Show("エラー");
                return;
            }
            try
            {
                SetImage(wb);
            }
            catch
            {
                MessageBox.Show("エラー");
            }
        }

        private void SetImage(WriteableBitmap wb)
        {
            try
            {
                BitmapImage img = new BitmapImage();
                stageImage.Source = wb;
                cropImage.Source = wb;
                Dispatcher.BeginInvoke(() =>
                {
                    imageWidth = stageImage.ActualWidth;
                    imageHeight = stageImage.ActualHeight;
                    double ccWidth = imageWidth - 40;
                    double ccHeight = imageHeight - 40;
                    double cbLeft = (imageWidth - ccWidth) / 2 - borderSize;
                    double cbTop = (imageHeight - ccHeight) / 2 - borderSize;
                    if (this.DataContext != null && cropKeep.IsChecked == true)
                    {
                        cbLeft = Math.Min(imageWidth - cropMinSize - borderSize, cropManager.ResizeHandleLeft_Left);
                        cbTop = Math.Min(imageHeight - cropMinSize - borderSize, cropManager.ResizeHandleTop_Top);
                        ccWidth = Math.Min(imageWidth - cbLeft - borderSize, Math.Min(imageWidth, cropManager.InnerWidth));
                        ccHeight = Math.Min(imageHeight - cbTop - borderSize, Math.Min(imageHeight, cropManager.InnerHeight));
                        this.DataContext = null;
                    }
                    cropManager = new CropManager();
                    this.DataContext = cropManager;
                    cropManager.BorderSize = borderSize;
                    cropManager.BorderColor = borderColor;
                    cropManager.CropMinSize = cropMinSize;
                    cropWidth.Maximum = imageWidth;
                    cropHeight.Maximum = imageHeight;
                    cropManager.ImageWidth = imageWidth;
                    cropManager.ImageHeight = imageHeight;
                    double htmlWidth = Convert.ToDouble(BrowserScreenInformation.ClientWidth);
                    double htmlHeight = Convert.ToDouble(BrowserScreenInformation.ClientHeight);
                    if (originalSizeBorder.Visibility != System.Windows.Visibility.Collapsed)
                    {
                        if (cbLeft > imageWidth - 1)
                        {
                            cbLeft = imageWidth - ccWidth - 1;
                        }
                        if (cbTop > imageHeight - 1)
                        {
                            cbTop = imageHeight - ccHeight - 1;
                        }
                    }
                    double osbLeft = (htmlWidth - imageWidth) / 2 + 10 - borderSize;
                    double osbTop = (htmlHeight - imageHeight) / 2 + 50 - borderSize;
                    originalSizeBorder.SetValue(Canvas.LeftProperty, osbLeft);
                    originalSizeBorder.SetValue(Canvas.TopProperty, osbTop);
                    originalSizeCanvas.Width = imageWidth;
                    originalSizeCanvas.Height = imageHeight;
                    cropManager.ResizeHandleLeft_Left = cbLeft;
                    cropManager.ResizeHandleTop_Top = cbTop;
                    cropManager.ResizeHandleRight_Left = cbLeft + borderSize + ccWidth;
                    cropManager.ResizeHandleBottom_Top = cbTop + borderSize + ccHeight;
                    stageCanvas.Width = maskCanvas.Width = cropImage.Width = imageWidth;
                    stageCanvas.Height = maskCanvas.Height = cropImage.Height = imageHeight;
                    txtInfo.Visibility = System.Windows.Visibility.Collapsed;
                    btnSave.IsEnabled = true;
                    originalSizeBorder.Visibility = System.Windows.Visibility.Visible;
                    if (scaleKeep.IsChecked == true)
                    {
                        scaleSlider.Value = zoom * 100;
                        scaleLabel.Content = ((int)(zoom * 100)).ToString() + "%";

                    }
                    else
                    {
                        scaleSlider.Value = 100;
                        scaleLabel.Content = "100%";
                    }
                });
            }
            catch
            {
            }
        }

        private double zoom = 1;
        private double centerX;
        private double centerY;
        private int zoomSteps;
        private void stageCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var location = e.GetPosition(sender as UIElement);

            var deltaZoom = e.Delta;
            if (deltaZoom > 0)
                zoom += .01;
            else
                zoom -= .01;
            zoom = Math.Min(1, zoom);
            zoom = Math.Max(0.01, zoom);
            scaleSlider.Value = zoom * 100;
            scaleLabel.Content = ((int)(zoom * 100)).ToString() + "%";

            if (deltaZoom >= 0)
            {
                if (zoomSteps == -1)
                {
                    centerX = 0;
                    centerY = 0;
                    zoomSteps = 0;
                }
                else
                {
                    centerX = (centerX * Math.Abs(zoomSteps) + location.X) / (Math.Abs(zoomSteps + 1));
                    centerY = (centerY * Math.Abs(zoomSteps) + location.Y) / (Math.Abs(zoomSteps + 1));
                    zoomSteps++;
                }
            }
            else
            {
                if (zoomSteps == 1)
                {
                    centerX = 0;
                    centerY = 0;
                    zoomSteps = 0;
                }
                else
                {
                    centerX = (centerX * Math.Abs(zoomSteps) - location.X) / (Math.Abs(zoomSteps - 1));
                    centerY = (centerY * Math.Abs(zoomSteps) - location.Y) / (Math.Abs(zoomSteps - 1));
                    zoomSteps--;
                }
            }

            stageZoomAnimationX.To = zoom;
            stageZoomAnimationY.To = zoom;
            cropImageZoomAnimationX.To = zoom;
            cropImageZoomAnimationY.To = zoom;

            var group = stageCanvas.RenderTransform as TransformGroup;
            var scale = group.Children[0] as ScaleTransform;
            stageZoomStoryboard.Begin();
            cropImageZoomStoryboard.Begin();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPEGファイル (*.jpg)|*.jpg|PNGファイル (*.png)|*.png|ビットマップファイル (*.bmp)|*.bmp";
            if (sfd.ShowDialog() == true)
            {
                IImageEncoder enc;
                string fn = sfd.SafeFileName.ToLower();
                if(fn.EndsWith(".jpg"))
                {
                    enc = new JpegEncoder();
                }
                else if (fn.EndsWith(".png"))
                {
                    enc = new PngEncoder();
                }
                else if (fn.EndsWith(".bmp"))
                {
                    enc = new BmpEncoder();
                }
                else
                {
                    MessageBox.Show("エラー");
                    return;
                }

                double scaledWidth = (cropImage.Width * zoom);
                double scaledHeight = (cropImage.Height * zoom);
                double scaledLeft = (imageWidth - scaledWidth) / 2;
                double scaledTop = (imageHeight - scaledHeight) / 2;
                Rect r = new Rect((double)cropCanvas.GetValue(Canvas.LeftProperty), (double)cropCanvas.GetValue(Canvas.TopProperty), cropCanvas.Width, cropCanvas.Height);
                Rect r2 = new Rect(scaledLeft, scaledTop, scaledWidth , scaledHeight);
                r.Intersect(r2);
                double clipedLeft = r.Left - scaledLeft;
                double clipedTop = r.Top - scaledTop;
                r.X = clipedLeft;
                r.Y = clipedTop;
                ScaleTransform st = new ScaleTransform();
                st.ScaleX = zoom;
                st.ScaleY = zoom;
                WriteableBitmap wb = new WriteableBitmap(cropImage, st);
                WriteableBitmap outputWb = new WriteableBitmap((int)r.Width, (int)r.Height);
                outputWb.Blit(new Rect(0, 0, r.Width, r.Height), wb, new Rect(r.Left, r.Top, r.Width, r.Height));
                ImageTools.Image img = ToImage(outputWb);
                using (Stream stream = sfd.OpenFile())
                {
                    enc.Encode(img, stream);
                }
            }
        }

        public static ImageTools.Image ToImage(WriteableBitmap bitmap)
        {
            bitmap.Invalidate();

            ImageTools.Image image = new ImageTools.Image(bitmap.PixelWidth, bitmap.PixelHeight);
            try
            {
                for (int y = 0; y < bitmap.PixelHeight; ++y)
                {
                    for (int x = 0; x < bitmap.PixelWidth; ++x)
                    {
                        int pixel = bitmap.Pixels[bitmap.PixelWidth * y + x];

                        byte a = (byte)((pixel >> 24) & 0xFF);

                        float aFactor = a / 255f;

                        byte r = (byte)(((pixel >> 16) & 0xFF) / aFactor);
                        byte g = (byte)(((pixel >> 8) & 0xFF) / aFactor);
                        byte b = (byte)((pixel & 0xFF) / aFactor);

                        image.SetPixel(x, y, r, g, b, a);
                    }
                }
            }
            catch (System.Security.SecurityException e)
            {
                throw new ArgumentException("Bitmap cannot accessed", e);
            }

            return image;
        }

        private void scaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (scaleSlider == null) return;
            zoom = scaleSlider.Value / 100;
            scaleLabel.Content = ((int)scaleSlider.Value).ToString() + "%";
            stageZoomAnimationX.To = zoom;
            stageZoomAnimationY.To = zoom;
            cropImageZoomAnimationX.To = zoom;
            cropImageZoomAnimationY.To = zoom;

            var group = stageCanvas.RenderTransform as TransformGroup;
            var scale = group.Children[0] as ScaleTransform;
            stageZoomStoryboard.Begin();
            cropImageZoomStoryboard.Begin();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPEG,PNG,BMPファイル (*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != true) return;
            string fn = ofd.File.Name.ToLower();
            IImageDecoder dec;
            if (fn.EndsWith(".jpg"))
            {
                dec = new JpegDecoder();
            }
            else if (fn.EndsWith(".png"))
            {
                dec = new PngDecoder();
            }
            else if (fn.EndsWith(".bmp"))
            {
                dec = new BmpDecoder();
            }
            else
            {
                MessageBox.Show("エラー");
                return;
            }
            try
            {
                FileStream fs = ofd.File.OpenRead();
                ImageTools.Image img = new ImageTools.Image();
                dec.Decode(img, fs);
                if (img.Width < 50 || img.Height < 50)
                {
                    MessageBox.Show("画像サイズが小さすぎます");
                    return;
                }
                WriteableBitmap wb = new WriteableBitmap(img.Width, img.Height);
                byte[] data = img.GetPixels();
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        int idx = (x + y * img.Width) * 4;
                        wb.SetPixel(x, y, Color.FromArgb(data[idx + 3], data[idx], data[idx + 1], data[idx + 2]));
                    }
                }
                SetImage(wb);
                fs.Close();
            }
            catch
            {
                MessageBox.Show("エラー");
            }
        }

        private void cropWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cropManager == null) return;
            if (cropWidth == null) return;
            if (cropManager.ResizeHandleLeft_Left + borderSize + cropWidth.Value > imageWidth)
            {
                cropManager.ResizeHandleLeft_Left = imageWidth - borderSize - cropWidth.Value;
            }
            else
            {
                cropManager.ResizeHandleRight_Left = cropManager.ResizeHandleLeft_Left + borderSize + cropWidth.Value;
            }
        }
    }
}
