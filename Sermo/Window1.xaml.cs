using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace Sermo
{

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();


           var ser = new XmlSerializer(typeof(Seat[]));
            using(var file = File.OpenRead(@"test.xml"))
            {
                var seats = (Seat[])ser.Deserialize(file);
                Canvas.ItemsSource = seats;
            }
           
        }

        private void rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var seat = (((Rectangle)sender).Tag as Seat);
            seat.Selected =! seat.Selected;
        }
    }
}
