using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace SWApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SWObject sWObject = new SWObject();
        SldWorks swApp = new SldWorks();
        ModelDoc2 swModel;
        DataTable dt = new DataTable();
        DataColumn dc1 = new DataColumn("Nazwa", typeof(string));
        DataColumn dc2 = new DataColumn("X", typeof(double));
        DataColumn dc3 = new DataColumn("Y", typeof(double));
        DataColumn dc4 = new DataColumn("Grubość", typeof(double));
        DataColumn dc5 = new DataColumn("Długość", typeof(double));
        DataColumn dc6 = new DataColumn("Ilość zacięć", typeof(int));

        public MainWindow()
        {
            InitializeComponent();
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);

            DataRow dr = dt.NewRow();

            dataGridProfile.DataContext = dt.DefaultView;
        }

        private void Btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> filepathWithName = sWObject.GetDirectory();
                List<ProfileSW> profiles = new List<ProfileSW>();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    ProfileSW profil = new ProfileSW(Convert.ToString(dt.Rows[i][0]), Convert.ToInt32(dt.Rows[i][1]),
                    Convert.ToInt32(dt.Rows[i][2]), Convert.ToDouble(dt.Rows[i][3]),
                    Convert.ToDouble(dt.Rows[i][4]), Convert.ToInt32(dt.Rows[i][5]));
                    profiles.Add(profil);
                }
                string assemblyName = filepathWithName[1];

                string filepathAsm = filepathWithName[0];
                string filepathDir = filepathWithName[2];

                sWObject.CreateAssembly(filepathAsm);
                foreach (ProfileSW profile in profiles)
                {
                    sWObject.CreateRectangleProfile(profile, $"{filepathDir}\\");
                    sWObject.AddToAssembly($"{filepathDir}\\{profile.Name}.SLDPRT", assemblyName);
                    sWObject.CloseSWDoc(profile.Name);

                }
                MessageBox.Show($"Wykonane");

            }
            catch (System.InvalidCastException)
            {
                MessageBox.Show("Wprowadź poprawne wartości liczbowe");
            }
            
        }

        private void BtnGenDXF_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ConfigurationManager swConfigurationManager;
                Configuration swConf;
                Component2 swComp;
                int options=0;
                var totalParts = sWObject.CountParts();
                //foreach(CheckBox checkbox in spCheckboxes.Children)
                //{
                //    for(int i =0;i< spCheckboxes.Children.Count; i++)
                //    {
                //        if(checkbox.IsEnabled == true)
                //        {
                //            options += 2 ^ i;
                //        }
                //    }
                //}
                if(cbPBSheet.IsChecked == true)
                {
                    options += 1;
                }
                if(cbPTSheet.IsChecked == true)
                {
                    options += 2;
                }
                if (cbAllDXF.IsChecked == true)
                {
                    options += 4;
                }

                if (options == 0 && cbDXFFromDrawing.IsChecked == false)
                {
                    MessageBox.Show("Wskaż wymagane opcje");
                    recOptions.Visibility = Visibility.Visible;
                    goto End;    
                }
                if (cbDXFFromDrawing.IsChecked == true)
                {
                    sWObject.saveDXFFromDrawing(txtFolderDir.Text, (bool)cbDXFDir.IsChecked);
                }

                swModel = swApp.ActiveDoc;
                swConfigurationManager = swModel.ConfigurationManager;
                swConf = swConfigurationManager.ActiveConfiguration;
                swComp = swConf.GetRootComponent3(false);
                sWObject.GenerateDXF(swComp, txtFolderDir.Text, (bool)cbDXFDir.IsChecked, options,totalParts);
                MessageBox.Show("Zakończono");
                End:;

            }
            catch (System.ObjectDisposedException)
            {

            }
            //catch (System.NullReferenceException)
            //{
            //    MessageBox.Show("Włącz złożenie, z którego chcesz wygenerować pliki");
            //}
            //catch (System.InvalidCastException)
            //{
            //    MessageBox.Show("Wybierz plik typu złożenie");
            //}

        }

        private void BtnFolderDir_Click(object sender, RoutedEventArgs e)
        {
            txtFolderDir.Text = sWObject.GetDirDXF();
            cbDXFDir.IsChecked = false;
            //txtFolderDir.IsEnabled = true;
        }

        private void CbDXFDir_Checked(object sender, RoutedEventArgs e)
        {
            //txtFolderDir.IsEnabled = false;
        }

        private void CbDXFDir_Unchecked(object sender, RoutedEventArgs e)
        {
            //txtFolderDir.IsEnabled = true;
        }

        private void CbAllDXF_Checked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = false;
            cbPTSheet.IsEnabled = false;
            cbPBSheet.IsChecked = true;
            cbPTSheet.IsChecked = true;
        }

        private void CbAllDXF_Unchecked(object sender, RoutedEventArgs e)
        {
            cbPBSheet.IsEnabled = true;
            cbPTSheet.IsEnabled = true;
            cbPBSheet.IsChecked = false;
            cbPTSheet.IsChecked = false;
        }
    }
}
