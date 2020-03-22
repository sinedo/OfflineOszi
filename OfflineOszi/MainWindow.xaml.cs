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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;        //file dialog
using Microsoft.Win32;  //file dialog
using System.Globalization; //standartisation

namespace OfflineOszi
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int[] sample;
        double[] time;
        double[] ch1;
        double[] ch2;

        public MainWindow()
        {
            InitializeComponent();
            wpfPlot1.plt.Title("Oszi");
            wpfPlot1.plt.YLabel("Volts [V]");
            wpfPlot1.plt.XLabel("time [ms]");
        }

        private void ButtonLoadCSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string file = ofd.FileName;
            try
            {
                Load(file);
            }
            catch (Exception)
            {
                
            }
            Load(file);

            wpfPlot1.plt.PlotScatter(time, ch1, label: "Channel 1");
            wpfPlot1.plt.PlotScatter(time, ch2, label: "Channel 2");
            wpfPlot1.plt.Legend();

            //wpfPlot1.plt.PlotScatter(time, ch1);
            wpfPlot1.Render();

            ButtonLoadCSV.IsEnabled = false;
        }

        public void Load(string filename)
        {
            List<string> lines = File.ReadAllLines(filename).ToList();

            sample = new int[lines.Count-8];
            time = new double[lines.Count - 8];
            ch1 = new double[lines.Count - 8];
            ch2 = new double[lines.Count - 8];

            string[] infos = new string[10];
            /* infos:                   zeile
             * 0...version              0
             * 1...Date                 1
             * 2...Device               2
             * 3...Nr of Samples        3
             * 4...Samplerate           4
             * 5...Tool                 5
             */
            
            int zeile = 0;

            foreach (string line in lines)
            {
                string[] daten = line.Split(',');

                switch (zeile)
                {
                    case 0:             //0...Version   
                        infos[0] = "Version: " + daten[1];
                        break;

                    case 1:             //1...Date    
                        infos[1] = daten[1];
                        break;

                    case 2:             //2...Device    
                        infos[2] = "Device: " + daten[1];
                        break;

                    case 3:             //3...Nr.Samples    
                        infos[3] = "Nr. of Samples: " + daten[1];
                        break;

                    case 4:             //4...Samplerate    
                        infos[4] = "Samplerate: " + daten[1];
                        break;

                    case 5:             //5...Tool:    
                        infos[5] = "Tool: " + daten[1];
                        break;

                    case 6:             //useless info
                        break;

                    case 7:             //useless info
                        break;

                    default:            //i>=8
                        sample[zeile-8]=Convert.ToInt32(daten[0]);
                        time[zeile - 8] = Convert.ToDouble(daten[1], CultureInfo.InvariantCulture);
                        ch1[zeile - 8] = Convert.ToDouble(daten[2], CultureInfo.InvariantCulture);
                        ch2[zeile - 8] = Convert.ToDouble(daten[3], CultureInfo.InvariantCulture);
                        break;
                }
                zeile++;
            }

            label.Content =
                infos[0] + "\n" +
                infos[1] + "\n" +
                infos[2] + "\n" +
                infos[3] + "\n" +
                infos[4] + "\n" +
                infos[5] + "\n" +
                infos[6] + "\n";
        }
    }
}