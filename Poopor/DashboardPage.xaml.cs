using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Poopor
{
    public partial class DashboardPage : PhoneApplicationPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            pickers[0] = durationPicker1;
            pickers[1] = durationPicker2;
            pickers[2] = durationPicker3;
            pickers[3] = durationPicker4;
            pickers[4] = durationPicker5;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            colorChart.DataSource = _data;
            shapeChart.DataSource = _data;
            bloodAmountChart.DataSource = _data;
            painLevelChart.DataSource = _data;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!isInit)
            {
                durationPicker1.SelectedItem = "1 week ago";
                isInit = true;
            }

            durationPicker1.SelectionChanged += durationPicker_SelectionChanged;
            durationPicker2.SelectionChanged += durationPicker2_SelectionChanged;
            durationPicker3.SelectionChanged += durationPicker3_SelectionChanged;
            durationPicker4.SelectionChanged += durationPicker4_SelectionChanged;
            durationPicker5.SelectionChanged += durationPicker5_SelectionChanged;
        }

        async void durationPicker5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS; ;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                var data = await RetrieveUserPoopData(inputDate);
                PlotGraph(data);
                ChangeValueOtherDurationPickers(durationPicker5, picker.SelectedItem.ToString());
            }
        }

        async void durationPicker4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS; ;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                var data = await RetrieveUserPoopData(inputDate);
                PlotGraph(data);
                ChangeValueOtherDurationPickers(durationPicker4, picker.SelectedItem.ToString());
            }
        }

        async void durationPicker3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS; ;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                var data = await RetrieveUserPoopData(inputDate);
                PlotGraph(data);
                ChangeValueOtherDurationPickers(durationPicker3, picker.SelectedItem.ToString());
            }
        }

        async void durationPicker2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS; ;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                var data = await RetrieveUserPoopData(inputDate);
                PlotGraph(data);
                ChangeValueOtherDurationPickers(durationPicker2, picker.SelectedItem.ToString());
            }
        }

        async void durationPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS; ;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                var data = await RetrieveUserPoopData(inputDate);
                PlotGraph(data);
                ChangeValueOtherDurationPickers(durationPicker1, picker.SelectedItem.ToString());
            }
        }

        private void ChangeValueOtherDurationPickers(ListPicker picker, string value)
        {
            int skipNum = 0;
            if (picker.Name.Equals(durationPicker1.Name))
            {
                skipNum = 1;
            }
            else if (picker.Name.Equals(durationPicker2.Name))
            {
                skipNum = 2;
            }
            else if (picker.Name.Equals(durationPicker3.Name))
            {
                skipNum = 3;
            }
            else if (picker.Name.Equals(durationPicker4.Name))
            {
                skipNum = 4;
            }
            else if (picker.Name.Equals(durationPicker5.Name))
            {
                skipNum = 5;
            }
            for (int i = 1; i <= 5; i++)
            {
                if (i != skipNum)
                {
                    pickers[i - 1].SelectedItem = value;
                }
            }
        }

        private async Task<ObservableCollection<UserPoopData>> RetrieveUserPoopData(DateTime inputDate)
        {
            ObservableCollection<UserPoopData> _data = new ObservableCollection<UserPoopData>();
            List<Poop_Table_SQLite> userPoopData = new List<Poop_Table_SQLite>();
            userLastestPoopDataInSQLite = sqliteFunctions.GetUserPoopData(SessionManagement.GetEmail());
            //userLastestPoopDataInSQLite.Reverse();
            foreach (var item in userLastestPoopDataInSQLite)
            {
                //Debug.WriteLine("inputDate: {0} itemDate: {1}", inputDate, item.Date_Time);
                if (item.Date_Time > inputDate)
                    userPoopData.Add(item);
                else
                    break;
            }
            //userPoopData.Reverse();
            //Debug.WriteLine("User poop data " + userPoopData.Count());
            if (userPoopData.Count >= 2)
            {
                foreach (var item in userPoopData)
                {
                    _data.Add(new UserPoopData()
                    {
                        color = poopColor_dictionary[item.Color],
                        shape = poopShape_dictionary[item.Shape],
                        bloodAmount = bloodAmount_dictionary[item.Blood_Amount],
                        painLevel = painLevel_dictionary[item.Pain_Level],
                        date = item.Date_Time.ToString()
                    });
                }
            }
            else
                _data = null;

            return _data;
        }

        private void PlotGraph(ObservableCollection<UserPoopData> userPoopData)
        {
            if (userPoopData != null)
            {
                _data.Clear();
                _data = userPoopData;
                colorChart.DataSource = _data;
                shapeChart.DataSource = _data;
                bloodAmountChart.DataSource = _data;
                painLevelChart.DataSource = _data;
            }
            else
            {
                colorChart.Opacity = 30;
                shapeChart.Opacity = 30;
                bloodAmountChart.Opacity = 30;
                painLevelChart.Opacity = 30;
                MessageBox.Show("Data is insufficient in this duration. Please change to other durations", "Sorry", MessageBoxButton.OK);
            }
        }

        private static DateTime previousInputDate;
        private DateTime inputDate;
        private static readonly TimeSpan START_TIME_OF_DAYS = new TimeSpan(0, 0, 0);
        private bool isInit;
        private ListPicker[] pickers = new ListPicker[5];
        private ObservableCollection<UserPoopData> _data = new ObservableCollection<UserPoopData>();
        private List<Poop_Table_SQLite> userLastestPoopDataInSQLite = null;
        private SQLiteFunctions sqliteFunctions = new SQLiteFunctions();
        private Dictionary<string, double> poopColor_dictionary = new Dictionary<string, double>(){
            {"Very light brown", 1},
            {"Medium brown", 2},
            {"Black", 3},
            {"Maroon", 4},
            {"Bright red", 5},
            {"Orange", 6},
            {"Dark green", 7},
            {"Yellow", 8},
            {"Gray", 9}
        };
        private Dictionary<string, double> poopShape_dictionary = new Dictionary<string, double>(){
            {"Separated hard lumps", 1},
            {"Lumpy sausage", 2},
            {"Cracked surface sausage", 3},
            {"Smooth soft snake", 4},
            {"Soft blobs with clear cut", 5},
            {"Mushy and fluffy pieces", 6},
            {"Entirely liquid", 7}
        };
        private Dictionary<string, double> painLevel_dictionary = new Dictionary<string, double>(){
            {"None", 1},
            {"Mild", 2},
            {"Moderate", 3},
            {"Severe", 4},
            {"Worst", 5}
        };
        private Dictionary<string, double> bloodAmount_dictionary = new Dictionary<string, double>(){
            {"None", 1},
            {"Little blood", 2},
            {"Medium blood", 3},
            {"Much blood", 4},
            {"A lot of blood", 5}
        };
    }

    public class UserPoopData
    {
        public double color { get; set; }
        public double shape { get; set; }
        public double bloodAmount { get; set; }
        public double painLevel { get; set; }
        public string date { get; set; }
    }
}