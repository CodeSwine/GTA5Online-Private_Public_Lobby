using System.Windows.Media;

namespace CodeSwine_Solo_Public_Lobby.Helpers
{
    public class ColorBrush
    {
        public static SolidColorBrush Red {
            get { return new SolidColorBrush(Color.FromArgb(255, (byte)220, (byte)53, (byte)69)); }
        }

        public static SolidColorBrush Blue {
            get { return new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)123, (byte)255)); }
        }

        public static SolidColorBrush Green {
            get { return new SolidColorBrush(Color.FromArgb(255, (byte)40, (byte)167, (byte)69)); }
        }

        public static SolidColorBrush Yellow {
            get { return new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)193, (byte)7)); }
        }
    }
}
