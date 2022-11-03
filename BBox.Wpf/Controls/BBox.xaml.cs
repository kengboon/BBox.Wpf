using BBox.Wpf.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BBox.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BBox.xaml
    /// </summary>
    public partial class BBox : UserControl
    {
        public BBox()
        {
            InitializeComponent();
            Color = Colors.LimeGreen;
            Thickness = 2;
        }

        public BBox(BBoxType roiType, string displayName)
            : this()
        {
            BBoxType = roiType;
            DisplayName = displayName;
        }

        public BBox(BBoxType roiType, string displayName, double top, double left, double width, double height)
            : this(roiType, displayName)
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        public BBox(BBoxType roiType, string displayName, double top, double left, double width, double height, Color color, double thickness)
            : this(roiType, displayName, top, left, width, height)
        {
            Color = color;
            Thickness = thickness;
        }

        #region Local properties
        protected BBoxType BBoxType;

        public string DisplayName
        {
            get => CTRL_Name.Text;
            set => CTRL_Name.Text = value;
        }

        private double m_Top;
        public double Top
        {
            get => m_Top;
            set
            {
                if (m_Top != value)
                {
                    m_Top = value;
                    Canvas.SetTop(this, m_Top * m_DisplayRatio);
                }
            }
        }

        private double m_Left;
        public double Left
        {
            get => m_Left;
            set
            {
                if (m_Left != value)
                {
                    m_Left = value;
                    Canvas.SetLeft(this, m_Left * m_DisplayRatio);
                }
            }
        }

        private double m_Width;
        protected new double Width
        {
            get => m_Width;
            set
            {
                if (m_Width != value)
                {
                    m_Width = value;
                    UpdateDisplay(m_DisplayRatio);
                }
            }
        }

        private double m_Height;
        protected new double Height
        {
            get => m_Height;
            set
            {
                if (m_Height != value)
                {
                    m_Height = value;
                    UpdateDisplay(m_DisplayRatio);
                }
            }
        }

        private Color m_Color;
        protected Color Color
        {
            get => m_Color;
            set
            {
                if (m_Color != value)
                {
                    m_Color = value;
                    CTRL_Shape.BorderBrush = new SolidColorBrush(m_Color);
                }
            }
        }

        private double m_Thickness;
        public double Thickness
        {
            get => m_Thickness;
            set
            {
                if (m_Thickness != value)
                {
                    m_Thickness = value;
                    CTRL_Shape.BorderThickness = new Thickness(m_Thickness);
                }
            }
        }

        private double m_DisplayRatio = 1;
        #endregion

        #region Helper methods
        public void UpdateDisplay(double ratio = 1)
        {
            m_DisplayRatio = ratio;

            // Canvas position
            Canvas.SetTop(this, Top * ratio);
            Canvas.SetLeft(this, Left * ratio);

            CTRL_Name.Text = DisplayName;

            var shapeWidth = Width * ratio;
            var shapeHeight = Height * ratio;
            var adornerWidth = shapeWidth + CTRL_Adorner.BorderThickness.Left + CTRL_Adorner.BorderThickness.Right;
            var adornerHeight = shapeHeight + CTRL_Adorner.BorderThickness.Top + CTRL_Adorner.BorderThickness.Bottom;
            // Adorner size
            CTRL_Adorner.Width = adornerWidth;
            CTRL_Adorner.Height = adornerHeight;
            // Shape size
            CTRL_Shape.Width = shapeWidth;
            CTRL_Shape.Height = shapeHeight;
            // Shape border
            CTRL_Shape.BorderBrush = new SolidColorBrush(Color);
            CTRL_Shape.BorderThickness = new Thickness(Thickness);
            if (BBoxType == BBoxType.Ellipse)
            {
                CTRL_Shape.CornerRadius = new CornerRadius(Math.Max(shapeWidth, shapeHeight) / 2);
            }
            // Resize thumbs positions
            Canvas.SetTop(CTRL_ResizeThumb_C, adornerHeight / 2 - CTRL_ResizeThumb_C.Height / 2);
            Canvas.SetLeft(CTRL_ResizeThumb_C, adornerWidth / 2 - CTRL_ResizeThumb_C.Width / 2);
            Canvas.SetTop(CTRL_ResizeThumb_N, -CTRL_ResizeThumb_N.Width / 2);
            Canvas.SetLeft(CTRL_ResizeThumb_N, adornerWidth / 2 - CTRL_ResizeThumb_N.Width / 2);
            Canvas.SetTop(CTRL_ResizeThumb_S, adornerHeight - CTRL_ResizeThumb_S.Width / 2);
            Canvas.SetLeft(CTRL_ResizeThumb_S, adornerWidth / 2 - CTRL_ResizeThumb_S.Width / 2);
            Canvas.SetTop(CTRL_ResizeThumb_W, adornerHeight / 2 - CTRL_ResizeThumb_W.Height / 2);
            Canvas.SetLeft(CTRL_ResizeThumb_W, -CTRL_ResizeThumb_W.Width / 2);
            Canvas.SetTop(CTRL_ResizeThumb_E, adornerHeight / 2 - CTRL_ResizeThumb_E.Height / 2);
            Canvas.SetLeft(CTRL_ResizeThumb_E, adornerWidth - CTRL_ResizeThumb_W.Width / 2);

            TRANS_Rotate.CenterX = adornerWidth / 2;
            TRANS_Rotate.CenterY = adornerHeight / 2;
        }

        internal void UpdateFromDisplay(double displayTop, double displayLeft, double displayWidth, double displayHeight)
        {
            if (displayWidth < 0 ||
                displayHeight < 0 ||
                displayTop < -(displayHeight * .5) ||
                displayLeft < -(displayWidth * .5) ||
                displayTop > (Parent as Canvas).ActualHeight - (displayHeight * .5) ||
                displayLeft > (Parent as Canvas).ActualWidth - (displayWidth * .5))
            {
                return;
            }

            if (m_DisplayRatio != 0 && !double.IsNaN(m_DisplayRatio))
            {
                var ratio = 1 / m_DisplayRatio;
                Top = Math.Round(displayTop * ratio, 3);
                Left = Math.Round(displayLeft * ratio, 3);
                Width = Math.Round(displayWidth * ratio, 3);
                Height = Math.Round(displayHeight * ratio, 3);
            }
        }
        #endregion

        #region Handle Resize
        private bool m_CanResize;
        public bool CanResize
        {
            get => m_CanResize;
            set
            {
                m_CanResize = value;
                if (!m_CanResize)
                {
                    IsResizeEnabled = false;
                }
            }
        }

        public bool IsResizeEnabled
        {
            get => CTRL_ResizeThumb_C.Visibility == Visibility.Visible;
            set
            {
                var visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (CTRL_ResizeThumb_C.Visibility != visibility)
                {
                    CTRL_ResizeThumb_C.Visibility = visibility;
                    CTRL_ResizeThumb_E.Visibility = visibility;
                    CTRL_ResizeThumb_W.Visibility = visibility;
                    CTRL_ResizeThumb_S.Visibility = visibility;
                    CTRL_ResizeThumb_N.Visibility = visibility;

                    if (value)
                    {
                        m_SelectedAction?.Invoke(this);
                    }
                }
            }
        }

        private Action<BBox> m_SelectedAction;
        public event Action<BBox> Selected
        {
            add => m_SelectedAction += value;
            remove => m_SelectedAction -= value;
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (!IsResizeEnabled)
            {
                IsResizeEnabled = true;
            }
        }

        private void ResizeThumb_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (IsResizeEnabled)
            {
                var control = sender as Ellipse;
                control.CaptureMouse();
                control.Tag = true;
            }
        }

        private void ResizeThumb_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            var control = sender as Ellipse;
            control.ReleaseMouseCapture();
            control.Tag = null;
        }

        private void ResizeThumb_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as Ellipse;
            if (IsResizeEnabled && control.Tag != null && (bool)control.Tag)
            {
                var relativePos = e.GetPosition(control);
                var moveRight = relativePos.X;
                var moveDown = relativePos.Y;

                if (control == CTRL_ResizeThumb_C)
                {
                    var top = Canvas.GetTop(this) + moveDown;
                    var left = Canvas.GetLeft(this) + moveRight;
                    var width = CTRL_Shape.Width;
                    var height = CTRL_Shape.Height;
                    UpdateFromDisplay(top, left, width, height);
                }
                else if (control == CTRL_ResizeThumb_N)
                {
                    var top = Canvas.GetTop(this) + moveDown;
                    var left = Canvas.GetLeft(this);
                    var width = CTRL_Shape.Width;
                    var height = CTRL_Shape.Height - moveDown;
                    UpdateFromDisplay(top, left, width, height);
                }
                else if (control == CTRL_ResizeThumb_S)
                {
                    var top = Canvas.GetTop(this);
                    var left = Canvas.GetLeft(this);
                    var width = CTRL_Shape.Width;
                    var height = CTRL_Shape.Height + moveDown;
                    UpdateFromDisplay(top, left, width, height);
                }
                else if (control == CTRL_ResizeThumb_W)
                {
                    var top = Canvas.GetTop(this);
                    var left = Canvas.GetLeft(this) + moveRight;
                    var width = CTRL_Shape.Width - moveRight;
                    var height = CTRL_Shape.Height;
                    UpdateFromDisplay(top, left, width, height);
                }
                else if (control == CTRL_ResizeThumb_E)
                {
                    var top = Canvas.GetTop(this);
                    var left = Canvas.GetLeft(this);
                    var width = CTRL_Shape.Width + moveRight;
                    var height = CTRL_Shape.Height;
                    UpdateFromDisplay(top, left, width, height);
                }
            }
        }
        #endregion
    }
}
