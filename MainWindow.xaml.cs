using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace CounterSplit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int segmentDeathCount = 0; // Deaths in the current segment
        private int TotalDeathCount = 0; // Total deaths
        private int segmentNumber = 1; // Segment counter
        private int segmentCurrent = 0;
        private string SegmentTime = string.Empty;

        private int TotalSegments = 0;

        private Stopwatch stopwatch = new Stopwatch();

        public ObservableCollection<SegmentData> Segments { get; set; }
        public MainWindow()
        {
            DragMove();
            InitializeComponent();
            Segments = new ObservableCollection<SegmentData>();
            SegmentsTable.ItemsSource = Segments; // Bind data to the DataGrid
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            segmentDeathCount++;
            TotalDeathCount++;
            DeathTotals.Text = TotalDeathCount.ToString();
            DeathCountLabel.Text = segmentDeathCount.ToString();
        }

        private void NewSplitButton_Click(object sender, RoutedEventArgs e)
        {
            

            if(TotalSegments == segmentCurrent+1)
            {
                // End of Splits
                Segments[segmentCurrent].CurrentTime = SegmentTime;
                Segments[segmentCurrent].DeathCount = segmentDeathCount;
                // Reset Color
                Segments[segmentCurrent].BackgroundColor = "#FF463F3F";

                Timers.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF16A6FF");
                // Refresh DataGrid
                SegmentsTable.Items.Refresh();
                stopwatch.Stop();
                DeathCountLabel.Text = "0";
                MessageBox.Show("GG!", "End of Splits", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (!stopwatch.IsRunning)
            {
                // Get Segment Length
                foreach (SegmentData data in Segments)
                {
                    TotalSegments++;
                }
                Segments[0].BackgroundColor = "#FF3373F4";
                SegmentsTable.Items.Refresh();

                StartTimer();
                return;
            }

            // Mark Next Split to blue
            Segments[segmentCurrent+1].BackgroundColor = "#FF3373F4";


            // Update By Index ??
            Segments[segmentCurrent].CurrentTime = SegmentTime;
            Segments[segmentCurrent].DeathCount = segmentDeathCount;
            // Reset Color
            Segments[segmentCurrent].BackgroundColor = "#FF463F3F";

            // Refresh DataGrid
            SegmentsTable.Items.Refresh();

            //goto next segment
            segmentCurrent++;

            segmentDeathCount = 0;
            DeathCountLabel.Text = "0";

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove(); // Allows dragging the window
            }
        }
        private async Task StartTimer()
        {
            Timers.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF00CC36");
            stopwatch.Start();
            while (stopwatch.IsRunning)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                Timers.Text = FormatTime(elapsed);
                SegmentTime = Timers.Text;
                await Task.Delay(10); // Update every millisecond
            }
            return;
        }
        private string FormatTime(TimeSpan elapsed)
        {
            int milliseconds = elapsed.Milliseconds / 10; // Convert 3-digit to 2-digit
            if (elapsed.Hours > 0)
                return $"{elapsed.Hours}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{milliseconds:D2}";
            if (elapsed.Minutes > 0)
                return $"{elapsed.Minutes}:{elapsed.Seconds:D2}.{milliseconds:D2}";
            return $"{elapsed.Seconds}.{milliseconds:D2}"; // Default: "S:MS"
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            segmentDeathCount = 0;
            segmentNumber = 1;
            DeathCountLabel.Text = "0";
            Segments.Clear();
        }

        private void UpdateSegmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SegmentNumberInput.Text, out int targetSegment) && targetSegment > 0)
            {

                #region Add Segment Manually
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1",BackgroundColor = "#FF463F3F", DeathCount = 0 });
                
                #endregion

                // Update By Index ??
                Segments[0].DeathCount = segmentDeathCount;

                // Refresh DataGrid
                SegmentsTable.Items.Refresh();

                // Reset segment death count
                segmentDeathCount = 0;
                DeathCountLabel.Text = "0";
            }
            else
            {
                MessageBox.Show("Enter a valid segment number!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }

    public class SegmentData
    {
        public string SegmentNumber { get; set; }
        public string CurrentTime { get; set; }
        public string BackgroundColor { get; set; }
        public int DeathCount { get; set; }
    }
}