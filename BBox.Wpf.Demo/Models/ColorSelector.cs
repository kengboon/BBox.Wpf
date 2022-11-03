using System.Linq;
using System.Reflection;

namespace BBox.Wpf.Demo.Models
{
    public class ColorSelector
    {
        public ColorSelector(string colorCode)
        {
            ColorCode = colorCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorSelector cs)
            {
                return cs.ColorCode == ColorCode;
            }
            else if (obj is System.Windows.Media.Color color)
            {
                return Color == color;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string ColorCode { get; }
        public System.Windows.Media.Color Color => ConvertColorNameToColor(ColorCode);

        public static System.Windows.Media.Color ConvertColorNameToColor(string colorName)
        {
            PropertyInfo props = typeof(System.Windows.Media.Colors).GetProperties().FirstOrDefault(f => f.Name == colorName);
            if (props != null)
            {
                return (System.Windows.Media.Color)props.GetValue(null);
            }
            return System.Windows.Media.Colors.LightGray;
        }
    }
}
