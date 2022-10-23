using BBox.Wpf.Enums;
using System.Windows;
using System.Windows.Media;

namespace BBox.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (BBoxType bboxType in System.Enum.GetValues(typeof(BBoxType)))
            {
                CTRL_BoxType.Items.Add(bboxType);
            }
            CTRL_BoxType.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // For demo:
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Car", 97.4, 626.5, 288.5, 132.4, Colors.Cyan, 2.5));
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Bike", 176.3, 163.2, 594.9, 390.2, Colors.Red, 2.5));
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Dog", 293.6, 172, 243.4, 427.6, Colors.LimeGreen, 2.5));        }

        private void AddDefaultBBoxButton_Click(object sender, RoutedEventArgs e)
        {
            CTRL_ImageCanvas.AddDefaultBBox((BBoxType)CTRL_BoxType.SelectedItem, CTRL_BoxName.Text);
        }

        private void RemoveSelectedBBoxButton_Click(object sender, RoutedEventArgs e)
        {
            CTRL_ImageCanvas.RemoveSelectedBBox();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            CTRL_ImageCanvas.BBoxes.Clear();
            Window_Loaded(null, null);
        }
    }
}
