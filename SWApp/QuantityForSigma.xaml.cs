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

namespace SWApp
{
    /// <summary>
    /// Interaction logic for QuantityForSIgma.xaml
    /// </summary>
    public partial class QuantityForSigma : Window
    {
        public int Quantity { get; set; }

        public QuantityForSigma()
        {
            InitializeComponent();
        }

        private void BtnAcceptQty_Click(object sender, RoutedEventArgs e)
        {
            string quantity = tbSigmaQty.Text;
            if(Int32.TryParse(quantity, out int result) == false)
            {
                MessageBox.Show("Wprowadź poprawny nakład");
            }
            else
            {
                Quantity = result;
                this.Close();
            }
            
        }
    }
}
