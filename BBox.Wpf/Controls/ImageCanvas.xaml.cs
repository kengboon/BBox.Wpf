using BBox.Wpf.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BBox.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ImageCanvas.xaml
    /// </summary>
    public partial class ImageCanvas : UserControl
    {
        public ImageCanvas()
        {
            InitializeComponent();
            BBoxes.CollectionChanged += BBoxes_CollectionChanged;
        }

        #region BBoxes
        /// <summary>
        /// Add a bounding box with default size & position.
        /// </summary>
        /// <param name="bboxType">Type of bounding box</param>
        public void AddDefaultBBox(BBoxType bboxType, string name = "")
        {
            if (ImageSource != null)
            {
                var width = ImageSource.Width;
                var height = ImageSource.Height;
                var newBox = new BBox(bboxType, name == null || name.Trim() == "" ? $"Box {BBoxes.Count + 1}" : name.Trim(),
                    width * .1, height * .1, width * .2, height * .2);
                BBoxes.Add(newBox);
                newBox.IsResizeEnabled = true;
            }
        }

        /// <summary>
        /// Remove selected bounding box.
        /// </summary>
        /// <param name="bbox"></param>
        public void RemoveSelectedBBox()
        {
            if (SelectedBBox != null)
            {
                BBoxes.Remove(SelectedBBox);
                SelectedBBox = null;
            }
        }

        /// <summary>
        /// Default bounding box color. Color of individual bounding box still can be changed.
        /// </summary>
        public Brush DefaultBBoxColor { get; set; } = new SolidColorBrush(Colors.LimeGreen);
        public double DefaultBBoxThickness { get; set; } = 2;

        /// <summary>
        /// The collection of bounding boxes for object references.
        /// Add or remove to this collection should also trigger change to canvas.
        /// </summary>
        public ObservableCollection<BBox> BBoxes { get; } = new ObservableCollection<BBox>();

        private BBox m_SelectedBBox;
        public BBox SelectedBBox
        {
            get => m_SelectedBBox;
            set
            {
                if (m_SelectedBBox != value)
                {
                    m_SelectedBBox = value;

                    if (m_SelectedBBox != null)
                    {
                        if (m_SelectedBBox.CanResize && !m_SelectedBBox.IsResizeEnabled)
                        {
                            m_SelectedBBox.IsResizeEnabled = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add bounding box to canvas.
        /// Add required event handlers.
        /// </summary>
        /// <param name="newBBox">Bounding box to be added</param>
        private void AddBBoxToView(BBox newBBox)
        {
            CTRL_Canvas.Children.Add(newBBox);
            newBBox.Selected += BBox_Selected;
            newBBox.UpdateDisplay(CTRL_Canvas.ActualWidth / ImageSource.Width);
        }

        /// <summary>
        /// Remove bounding box from canvas.
        /// Remove event handlers so object can be freed and GC-ed.
        /// </summary>
        /// <param name="oldBBox">Bounding box to be removed</param>
        private void RemoveBBoxFromView(BBox oldBBox)
        {
            if (SelectedBBox == oldBBox)
            {
                SelectedBBox = null;
            }
            oldBBox.Selected -= BBox_Selected;
            CTRL_Canvas.Children.Remove(oldBBox);
        }

        private void MoveBBoxToTopOfView(BBox bbox)
        {
            CTRL_Canvas.Children.Remove(bbox);
            CTRL_Canvas.Children.Add(bbox);
        }

        private void BBox_Selected(BBox selectedBBox)
        {
            MoveBBoxToTopOfView(selectedBBox);
            SelectedBBox = selectedBBox;
            foreach (var child in CTRL_Canvas.Children)
            {
                if (child is BBox bbox && bbox != selectedBBox)
                {
                    bbox.IsResizeEnabled = false;
                }
            }
        }

        /// <summary>
        /// Event handler for bounding box collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BBoxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        AddBBoxToView((BBox)item);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        RemoveBBoxFromView((BBox)item);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.OldItems)
                    {
                        RemoveBBoxFromView((BBox)item);
                    }
                    foreach (var item in e.NewItems)
                    {
                        AddBBoxToView((BBox)item);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        var temp = new List<BBox>();
                        foreach (var child in CTRL_Canvas.Children)
                        {
                            if (child is BBox bbox)
                            {
                                temp.Add(bbox);
                            }
                        }
                        foreach (var bbox in temp)
                        {
                            RemoveBBoxFromView(bbox);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Container
        /// <summary>
        /// Size changed of the Grid. Required update the display ratio of all bounding boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CTRL_Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var container = sender as FrameworkElement;
            if (container != null)
            {
                foreach (var child in CTRL_Canvas.Children)
                {
                    if (child is BBox bbox)
                    {
                        bbox.UpdateDisplay(container.ActualWidth / ImageSource.Width);
                    }
                }
            }
        }

        public new Brush Background
        {
            get => CTRL_Container.Background;
            set => CTRL_Container.Background = value;
        }
        #endregion

        #region Image
        public ImageSource ImageSource
        {
            get => CTRL_Image.Source;
            set
            {
                CTRL_Image.Source = value;
                if (value == null)
                {
                    BBoxes.Clear();
                }
            }
        }
        #endregion

        private void CTRL_Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectedBBox = null;
            foreach (var child in CTRL_Canvas.Children)
            {
                if (child is BBox bbox)
                {
                    bbox.IsResizeEnabled = false;
                }
            }
        }
    }
}
