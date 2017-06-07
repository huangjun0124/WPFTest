using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
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

namespace WpfApplication
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

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            RefreshDBData();
        }

        private void btnFuckTheWorld_Click(object sender, RoutedEventArgs e)
        {
            SaveData1();
            RefreshDBData();
        }

        private void SaveData1()
        {
            // Open the connection using the connection string.
            using (SqlCeConnection con = new SqlCeConnection(DB_CONNECTION_STR))
            {
                con.Open();

                // Insert into the SqlCe table. ExecuteNonQuery is best for inserts.
                using (SqlCeCommand com = new SqlCeCommand("INSERT INTO Love(Name, Lover, RelationFlag, WordsToSay) VALUES(@0,@1,@2,@3)", con))
                {
                    com.Parameters.AddWithValue("@0", "I");
                    com.Parameters.AddWithValue("@1", "SomeOne");
                    com.Parameters.AddWithValue("@2", false);
                    com.Parameters.AddWithValue("@3", "SomeDay");
                    com.ExecuteNonQuery();
                }
            }
        }


        private void SaveData2()
        {
            WpfApplication.Database1DataSet database1DataSet = ((WpfApplication.Database1DataSet)(this.FindResource("database1DataSet")));
            // Load data into the table Love. You can modify this code as needed.
            WpfApplication.Database1DataSetTableAdapters.LoveTableAdapter database1DataSetLoveTableAdapter = new WpfApplication.Database1DataSetTableAdapters.LoveTableAdapter();
            database1DataSetLoveTableAdapter.Connection.ConnectionString = DB_CONNECTION_STR;
            database1DataSetLoveTableAdapter.Update(database1DataSet.Love);
        }

        private const string DB_CONNECTION_STR = @"Data Source=|DataDirectory|\Database1.sdf;Password=TestPwd;Persist Security Info=True";
        private void RefreshDBData()
        {
            WpfApplication.Database1DataSet database1DataSet = ((WpfApplication.Database1DataSet)(this.FindResource("database1DataSet")));
            // Load data into the table Love. You can modify this code as needed.
            WpfApplication.Database1DataSetTableAdapters.LoveTableAdapter database1DataSetLoveTableAdapter = new WpfApplication.Database1DataSetTableAdapters.LoveTableAdapter();
            database1DataSetLoveTableAdapter.Connection.ConnectionString = DB_CONNECTION_STR;
            database1DataSetLoveTableAdapter.Fill(database1DataSet.Love);
            System.Windows.Data.CollectionViewSource loveViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("loveViewSource")));
            loveViewSource.View.MoveCurrentToFirst();
        }

        private void btnAsyncDownload_Click(object sender, RoutedEventArgs e)
        {
            AsyncDownload asd = new AsyncDownload();
            asd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            asd.ShowDialog();
        }
    }
}
