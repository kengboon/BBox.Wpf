using BBox.Wpf.Enums;
using System.Windows;
using System.Windows.Media;
using BBox.Wpf.Demo.Models;
using System.Windows.Controls;

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
            CTRL_BoxType.SelectionChanged += CTRL_BoxType_SelectionChanged;
            CTRL_BoxType.SelectedItem = BBoxType.None;

            foreach (var prop in typeof(Colors).GetProperties())
            {
                var selector = new ColorSelector(prop.Name);
                CTRL_Color.Items.Add(selector);

                if (prop.Name == "LimeGreen")
                {
                    CTRL_Color.SelectionChanged += CTRL_Color_SelectionChanged;
                    CTRL_Color.SelectedItem = selector;
                }
            }

            CTRL_BoxName.TextChanged += CTRL_BoxName_TextChanged;
            CTRL_BoxName.Text = "BBox #1";
        }

        private void CTRL_BoxName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            CTRL_ImageCanvas.BBoxNameToAdd = txtBox.Text;
        }

        private void CTRL_Color_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ColorSelector)
                CTRL_ImageCanvas.BBoxColorToAdd = ((ColorSelector)e.AddedItems[0]).Color;
        }

        private void CTRL_BoxType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BBoxType)
            {
                CTRL_ImageCanvas.BBoxTypeToAdd = (BBoxType)e.AddedItems[0];
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // For demo:
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Car", 97.4, 626.5, 288.5, 132.4, Colors.Cyan, 2.5));
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Bike", 176.3, 163.2, 594.9, 390.2, Colors.Red, 2.5));
            CTRL_ImageCanvas.BBoxes.Add(new Controls.BBox(BBoxType.Rectangle, "Dog", 293.6, 172, 243.4, 427.6, Colors.LimeGreen, 2.5));
        }

        private void RemoveSelectedBBoxButton_Click(object sender, RoutedEventArgs e)
        {
            if (CTRL_ImageCanvas.SelectedBBox != null)
            {
                if (MessageBox.Show("Delete selected bounding box?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CTRL_ImageCanvas.RemoveSelectedBBox();
                }
            }
            else
            {
                MessageBox.Show("No selected bounding box.", "Delete", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Reset demo?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CTRL_ImageCanvas.BBoxes.Clear();
                Window_Loaded(null, null);
            }
        }
    }
}
