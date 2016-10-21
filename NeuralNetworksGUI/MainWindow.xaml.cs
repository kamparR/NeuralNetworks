using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNetworksGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<KeyValuePair<int, int>> source;
        List<KeyValuePair<DateTime, double>> valueList = new List<KeyValuePair<DateTime, double>>();
        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            LoadLineChartData();
        }

        private void LoadLineChartData()
        {

            //Example.DataContext = valueList;

            source = new ObservableCollection<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>(1, 100),
                new KeyValuePair<int, int>(2, 90),
                new KeyValuePair<int, int>(3, 70),
                new KeyValuePair<int, int>(4, 65),
            };
            ChartSeries.DataContext = source;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            source.Add(new KeyValuePair<int, int>(source.Count, rand.Next(0, 100)));
        }
    }
}
