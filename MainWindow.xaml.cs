using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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


        // Hotkey shit
        private const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public MainWindow()
        {
            DragMove();
            InitializeComponent();
            Segments = new ObservableCollection<SegmentData>();
            SegmentsTable.ItemsSource = Segments; // Bind data to the DataGrid
            Loaded += (s, e) => RegisterHotkeys();
            Closed += (s, e) => UnregisterHotkeys();
            UpdateSegmentButton_Click();
        }

        private void IncrementButton_Click(object sender = null, RoutedEventArgs e = null)
        {
            segmentDeathCount++;
            TotalDeathCount++;
            DeathTotals.Text = TotalDeathCount.ToString();
            DeathCountLabel.Text = segmentDeathCount.ToString();
        }

        private void NewSplitButton_Click(object sender = null, RoutedEventArgs e = null)
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

                Task.Delay(10000);
                MessageBox.Show("GG!\nYou Have Beaten God of War PAIN+\nSplit Created by Putra3340", "End of Splits", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (!stopwatch.IsRunning)
            {
                TotalSegments = 0;
                // Get Segment Length
                foreach (SegmentData data in Segments)
                {
                    TotalSegments++;
                }

                if (TotalSegments == 0)
                {
                    UpdateSegmentButton_Click();
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

        private void ResetButton_Click(object sender = null, RoutedEventArgs e = null)
        {
            segmentDeathCount = 0;
            segmentNumber = 1;
            segmentCurrent = 0;
            DeathCountLabel.Text = "0";
            Segments.Clear();
            stopwatch.Stop();
            stopwatch.Reset();
            Timers.Text = "0.00";
            Timers.Foreground = Brushes.White;
            UpdateSegmentButton_Click();
        }

        private void UpdateSegmentButton_Click(object sender = null, RoutedEventArgs e = null)
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



        // Hotkey
        private void RegisterHotkeys()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(WndProc);

            for (int i = 0; i <= 9; i++)
            {
                RegisterHotKey(hwnd, i, 0, (uint)(KeyInterop.VirtualKeyFromKey(Key.NumPad0) + i));
            }
            RegisterHotKey(hwnd, 10, 0, (uint)KeyInterop.VirtualKeyFromKey(Key.Add));
        }

        private void UnregisterHotkeys()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            for (int i = 0; i <= 10; i++)
            {
                UnregisterHotKey(hwnd, i);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();

                if(id == 0)
                {
                    NewSplitButton_Click();
                }else if(id == 10)
                {
                    IncrementButton_Click();
                }else if(id == 9) {
                    ResetButton_Click();
                }
                handled = true;
            }
            return IntPtr.Zero;
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