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
        private bool isResuming = true;
        private int TotalSegments = 0;

        private Stopwatch stopwatch = new Stopwatch();
        private TimeSpan offset = TimeSpan.FromMinutes(1.5) + TimeSpan.FromMilliseconds(120);

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


            if (TotalSegments == segmentCurrent + 1)
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
                Segments[segmentCurrent].BackgroundColor = "#FF3373F4";
                SegmentsTable.Items.Refresh();
                
                StartTimer();
                return;
            }

            // Mark Next Split to blue
            Segments[segmentCurrent + 1].BackgroundColor = "#FF3373F4";




            // Update By Index ??
            Segments[segmentCurrent].CurrentTime = SegmentTime;
            Segments[segmentCurrent].DeathCount = segmentDeathCount;
            // Reset Color
            Segments[segmentCurrent].BackgroundColor = "#FF463F3F";
            // TODO : Add delta time

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
            if (isResuming)
            {
                stopwatch.Start();
            }
            else
            {
                offset = TimeSpan.Zero; // Reset offset when starting fresh
                stopwatch.Start();
            }
            while (stopwatch.IsRunning)
            {
                TimeSpan elapsed = stopwatch.Elapsed + offset;
                Timers.Text = FormatTime(elapsed);
                SegmentTime = Timers.Text.Split(".").First();
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
            TotalDeathCount = 0;
            DeathTotals.Text = "0";
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


                Segments.Add(new SegmentData { SegmentNumber = "Hydra 1", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra 2", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Poseidon", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Hydra", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Aegean Sea", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Minotaurs", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Aphrodite", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Medusa", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Oracle", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Stay Away", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Wraiths C", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Wraiths", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Zeus Fury", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Temple", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Old Man", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Oracle", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Desert", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "3 Sirens", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Sirens", BackgroundColor = "#FF463F3F", DeathCount = 0 });
                Segments.Add(new SegmentData { SegmentNumber = "Cronos%", BackgroundColor = "#FF463F3F", DeathCount = 0 });



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

                if (id == 0)
                {
                    NewSplitButton_Click();
                }
                else if (id == 10)
                {
                    IncrementButton_Click();
                }
                else if (id == 9)
                {
                    ResetButton_Click();
                }
                handled = true;
            }
            return IntPtr.Zero;
        }

        private async Task<bool> ImportSplits(string[] splits)
        {
            if (splits.Length > 7)
            {
                Segments.Clear();
                string Title = splits[0];
                string Category = splits[1];

                
                // This is set timespan offset
                string TimeState = splits[4];
                if(TimeState.Replace(":","").Replace(".","") == "0000000000")
                {
                    isResuming = false;
                }
                else
                {
                    string Milisecond = TimeState.Split(".").Last().Split(" ").First();
                    string Second = TimeState.Split(":")[3].Split(".").First();
                    string Minute = TimeState.Split(":")[2];
                    string Hour = TimeState.Split(":")[1];
                    string Day = TimeState.Split(":")[0];
                    offset = new TimeSpan(int.Parse(Day), int.Parse(Hour), int.Parse(Minute), int.Parse(Second), int.Parse(Milisecond));
                    Timers.Text = FormatTime(offset);
                    // Also Set the timer to resume
                    isResuming = true;
                }
                segmentCurrent = int.Parse(splits[5].Split(" ").First());

                // Set Segments
                for (int i = 7; i < splits.Length; i++)
                {
                    string split = splits[i];
                    if (!string.IsNullOrEmpty(split))
                    {
                        // Add Segment
                        Segments.Add(new SegmentData { SegmentNumber = split.Split("SegmentNumber = \"").Last().Split("\"").First(), BackgroundColor = "#FF463F3F",CurrentTime = split.Split("CurrentTime = \"").Last().Split("\"").First().Split(".").First(), DeathCount = int.Parse(split.Split("DeathCount = ").Last().Split(",").First())});
                    }
                }
                Segments[segmentCurrent].BackgroundColor = "#FF3373F4";
            }
            else
            {
                MessageBox.Show("Invalid Split File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return true;
        }

        private async void ImportFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Split Files (*.ds)|*.ds",
                Title = "Import Splits"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string[] splits = System.IO.File.ReadAllLines(openFileDialog.FileName, Encoding.UTF8);
                await ImportSplits(splits);
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