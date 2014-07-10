using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace SteamIdToGuid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertSteamId(object sender, RoutedEventArgs e)
        {
            Int64 id = 0;
            try
            {
                id = Convert.ToInt64(SteamIdTB.Text);
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return;
            }
            Task t = Task.Factory.StartNew(delegate
            {
                byte[] parts = { 0x42, 0x45, 0, 0, 0, 0, 0, 0, 0, 0 };
                byte counter = 2;
                do
                {
                    parts[counter++] = (byte)(id & 0xFF);
                } while ((id >>= 8) > 0);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] beHash = md5.ComputeHash(parts);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < beHash.Length; i++)
                {
                    sb.Append(beHash[i].ToString("x2"));
                }
                return sb.ToString();
            }).ContinueWith(ret => GuidTB.Text = ret.Result, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
