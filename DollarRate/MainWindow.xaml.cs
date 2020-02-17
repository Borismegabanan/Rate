using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace DollarRate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        /// <summary>
        /// Экземпляр графика
        /// </summary>
        public SeriesCollection DollarRateChart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие на изменение даты в календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime FirstDate = Calendar.SelectedDates.First();
            DateTime LastDate = Calendar.SelectedDates.Last();
            ChartBuilder(FirstDate,LastDate);
        }


        /// <summary>
        /// Построение графика
        /// </summary>
        /// <param name="FDateTime">Первая дата</param>
        /// <param name="LDateTime">Последняя дата</param>
        public void ChartBuilder(DateTime FDateTime, DateTime LDateTime)
        {
            DataContext = null;
            //Код доллара
            MyLib.Currency currency = new MyLib.Currency("R01235");
            DollarRateChart = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<decimal>(currency.ValuesOfRange(FDateTime, LDateTime))
                },
            };
            DataContext = this;
        }

    }
}
