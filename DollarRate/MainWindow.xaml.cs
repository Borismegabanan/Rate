using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using MyLib;

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
        public SeriesCollection DollarRateChart
        {
            get => (SeriesCollection) GetValue(DollarRateChartProperty);
            set => SetValue(DollarRateChartProperty, value);
        }

        public static readonly DependencyProperty DollarRateChartProperty = DependencyProperty.Register(
            nameof(DollarRateChart), typeof(SeriesCollection), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Событие на изменение даты в календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var firstDate = Calendar.SelectedDates.First();
            var lastDate = Calendar.SelectedDates.Last();

            DollarRateChart = BildChart(firstDate,lastDate);
        }


        /// <summary>
        /// Построение графика
        /// </summary>
        /// <param name="fDateTime">Первая дата</param>
        /// <param name="lDateTime">Последняя дата</param>
        public static SeriesCollection BildChart(DateTime fDateTime, DateTime lDateTime)
        {
            //Код доллара
            var currency = new MyLib.Currency(ECurrencyType.USD);

            return new SeriesCollection
            {
                new LineSeries {Values = new ChartValues<decimal>(currency.ValuesOfRange(fDateTime, lDateTime))}
            };
        }

    }
}
