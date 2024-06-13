using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for CPU_Usage_Gauge.xaml
    /// </summary>
    public partial class CPU_Usage_Gauge : UserControl
    {

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CPU_Usage_Gauge), new PropertyMetadata(0.0, OnValueChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CPU_Usage_Gauge gauge = d as CPU_Usage_Gauge;
            gauge.UpdateGauge();
        }
        public CPU_Usage_Gauge()
        {
            InitializeComponent();
        }

        private void UpdateGauge()
        {
            double angle = (Value / 100) * 180;
            double radians = angle * (Math.PI / 180);

            double centerX = ActualWidth / 2;
            double centerY = ActualHeight / 2;

            double radius = (ActualWidth - 40) / 2;
            double x = centerX + radius * Math.Cos(radians - Math.PI / 2);
            double y = centerY + radius * Math.Sin(radians - Math.PI / 2);

            LineGeometry needle = new LineGeometry(new Point(centerX, centerY), new Point(x, y));
            GaugeNeedle.Data = needle;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateGauge();
        }
    }
}
