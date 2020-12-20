using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kurirska_Služba.Forms
{
    /// <summary>
    /// Interaction logic for WindowPrice.xaml
    /// </summary>
    public partial class WindowPrice : Window
    {
        public WindowPrice()
        {
            InitializeComponent();
        }
        public WindowPrice(String windowType)
        {
            InitializeComponent();
            setType(windowType);
        }

        private void setType(String type)
        {
            switch (type)
            {
                case "edit":
                    this.Title = "Izmena cene";
                    btnApply.Content = "Sačuvaj";
                    break;
                case "add":
                    this.Title = "Dodavanje nove cene";
                    btnApply.Content = "Dodaj cenu";
                    break;
            }

        }
    }
}
