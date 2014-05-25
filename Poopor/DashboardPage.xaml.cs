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

        public ObservableCollection<PData> PieData = new ObservableCollection<PData>()
        {
            new PData() { title = "slice #1", value = 10 },
            new PData() { title = "slice #2", value = 10 },
            new PData() { title = "slice #3", value = 10 },
            new PData() { title = "slice #4", value = 10 },
            new PData() { title = "slice #5", value = 10 },
            new PData() { title = "slice #6", value = 10 },
            new PData() { title = "slice #7", value = 10 },
            new PData() { title = "slice #8", value = 10 },
            new PData() { title = "slice #9", value = 10 },
            new PData() { title = "slice #10", value = 10 },
            new PData() { title = "slice #11", value = 10 },
            new PData() { title = "slice #12", value = 10 },
        };

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            pie1.DataSource = PieData;
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

        void durationPicker5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            int totalDays = 0;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 7;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 14;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 21;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 28;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                PlotGraph(RetrieveUserPoopData(inputDate));
                ChangeValueOtherDurationPickers(durationPicker5, picker.SelectedItem.ToString());
                setFreqText(totalDays); // Done the upper task in Overview (You num, You type)
                createTransitRepresentation(); // Done lower task in Overview (You transit, pie)
            }
        }

        void durationPicker4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            int totalDays = 0;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 7;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 14;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 21;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 28;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                PlotGraph(RetrieveUserPoopData(inputDate));
                ChangeValueOtherDurationPickers(durationPicker4, picker.SelectedItem.ToString());
                setFreqText(totalDays); // Done the upper task in Overview (You num, You type)
                createTransitRepresentation(); // Done lower task in Overview (You transit, pie)
            }
        }

        void durationPicker3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            int totalDays = 0;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 7;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 14;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 21;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 28;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                PlotGraph(RetrieveUserPoopData(inputDate));
                ChangeValueOtherDurationPickers(durationPicker3, picker.SelectedItem.ToString());
                setFreqText(totalDays); // Done the upper task in Overview (You num, You type)
                createTransitRepresentation(); // Done lower task in Overview (You transit, pie)
            }
        }

        void durationPicker2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            int totalDays = 0;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 7;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 14;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 21;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 28;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                PlotGraph(RetrieveUserPoopData(inputDate));
                ChangeValueOtherDurationPickers(durationPicker2, picker.SelectedItem.ToString());
                setFreqText(totalDays); // Done the upper task in Overview (You num, You type)
                createTransitRepresentation(); // Done lower task in Overview (You transit, pie)
            }
        }

        void durationPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as ListPicker;
            int totalDays = 0;
            if (picker.SelectedItem.ToString().Equals("1 week ago"))
            {
                inputDate = DateTime.Now.AddDays(-7);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 7;
            }
            else if (picker.SelectedItem.ToString().Equals("2 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-14);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 14;
            }
            else if (picker.SelectedItem.ToString().Equals("3 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-21);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 21;
            }
            else if (picker.SelectedItem.ToString().Equals("4 weeks ago"))
            {
                inputDate = DateTime.Now.AddDays(-28);
                inputDate = inputDate.Date + START_TIME_OF_DAYS;
                totalDays = 28;
            }
            if (inputDate != previousInputDate)
            {
                previousInputDate = inputDate;
                PlotGraph(RetrieveUserPoopData(inputDate));
                ChangeValueOtherDurationPickers(durationPicker1, picker.SelectedItem.ToString());
                setFreqText(totalDays); // Done the upper task in Overview (You num, You type)
                createTransitRepresentation(); // Done lower task in Overview (You transit, pie)
            }
        }

        private void setFreqText(double totalDays) // Done overview upper task
        {
            int userRecordCount = (_data != null) ? _data.Count : 0;

            double dividedNum = 0;
            if (userRecordCount != 0)
            {
                dividedNum = userRecordCount / totalDays;

                if (dividedNum >= 1.00)
                {
                    you_number.Text = "" + (int)dividedNum;
                    you_type.Text = ((int)dividedNum == 1) ? "time/day" : "times/day";
                }
                else
                {
                    //find how many time that multiply by dividedNum >= 1
                    int i = 1;
                    while (i * dividedNum < 1)
                    {
                        i++;
                    }

                    you_number.Text = "" + i;
                    you_type.Visibility = System.Windows.Visibility.Visible;
                    you_type.Text = "days/time";
                }
            }
            else
            {
                //Data is not sufficient -> not represent or what ?
                you_number.Text = "- ";
                you_type.Visibility = System.Windows.Visibility.Collapsed;
                Debug.WriteLine("Data is not sufficient for freq (Overview upper task)");
            }
        }

        private void reCreatePie(int pattern)
        {
            PieData.Clear();
            for (int i = 1; i <= 12; i++) PieData.Add(new PData() { title = "slice #" + i, value = 10 });

            switch (pattern)
            {
                case 1: pie1.setBrushesPattern(1); break;
                case 2: pie1.setBrushesPattern(2); break;
                case 3: pie1.setBrushesPattern(3); break;
                case 4: pie1.setBrushesPattern(4); break;
                case 5: pie1.setBrushesPattern(5); break;
                case 6: pie1.setBrushesPattern(6); break;
                case 7: pie1.setBrushesPattern(7); break;
                case 8: pie1.setBrushesPattern(8); break;
                case 9: pie1.setBrushesPattern(9); break;
                case 10: pie1.setBrushesPattern(10); break;
                case 11: pie1.setBrushesPattern(11); break;
                case 12: pie1.setBrushesPattern(12); break;
                default: pie1.setBrushesPattern(0); break;
            }
        }

        private void setDefaultForTransitDic()
        {
            foreach (var key in you_transit_dictionary.Keys.ToList())
            {
                you_transit_dictionary[key] = 0;
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

        private ObservableCollection<UserPoopData> RetrieveUserPoopData(DateTime inputDate)
        {
            ObservableCollection<UserPoopData> _data = new ObservableCollection<UserPoopData>();
            userLastestPoopDataInSQLite = sqliteFunctions.GetUserPoopData(SessionManagement.GetEmail());
            if (userLastestPoopDataInSQLite.Count < 2)
            {
                MessageBox.Show("Your data is not enough to represent in dashboard", "Sorry", MessageBoxButton.OK);
                _data = null;
                setDefaultForTransitDic(); // clear all value to 0 in transit dic
            }
            else
            {
                List<Poop_Table_SQLite> userPoopData = new List<Poop_Table_SQLite>();
                userLastestPoopDataInSQLite.Reverse();
                foreach (var item in userLastestPoopDataInSQLite)
                {
                    if (item.Date_Time > inputDate)
                        userPoopData.Add(item);
                    else
                        break;
                }
                userPoopData.Reverse();
                Debug.WriteLine("User poop data " + userPoopData.Count());

                foreach (var item in userPoopData)
                {
                    _data.Add(new UserPoopData()
                    {
                        color = poopColor_dictionary[item.Color],
                        shape = poopShape_dictionary[item.Shape],
                        bloodAmount = bloodAmount_dictionary[item.Blood_Amount],
                        painLevel = painLevel_dictionary[item.Pain_Level]
                    });
                    Debug.WriteLine(item.Date_Time);

                    // add data into the transit dicitonary
                    DateTime dt = item.Date_Time;
                    string you_transit_dic_key = dt.ToString("hh") + "_" + dt.ToString("tt");
                    you_transit_dictionary[you_transit_dic_key] += 1;
                }
                Debug.WriteLine("Initialize Finished");
            }
            return _data;
        }

        private void createTransitRepresentation() // Done overview lower task
        {
            int userRecordCount = (_data != null) ? _data.Count : 0;

            if (userRecordCount < 2)
            {
                setDefaultForTransitDic();
                reCreatePie(0); // pattern 0
                you_transit_time.Text = "-  : insufficient data"; // set text
            }
            else
            {
                // get first selected transit time.
                string selectedTransitItem = "";

                // Order by values.
                // ... Use LINQ to specify sorting by value.
                var items = from pair in you_transit_dictionary
                            orderby pair.Value descending
                            select pair;

                foreach (KeyValuePair<string, int> pair in items)
                {
                    //Debug.WriteLine("{0}: {1}", pair.Key, pair.Value);
                    selectedTransitItem = pair.Key; // get only first item
                    break;
                }

                string textHourNum = selectedTransitItem.ElementAt(0) + "" + selectedTransitItem.ElementAt(1);
                string textAMPM = selectedTransitItem.ElementAt(3) + "." + selectedTransitItem.ElementAt(4) + ".";

                int intHourNum = Int32.Parse(textHourNum);

                string fullTextForTransit = intHourNum + ".00 " + textAMPM + " - " + intHourNum + ".59 " + textAMPM;

                int pattern = (intHourNum % 12 == 0) ? 1 : intHourNum + 1;
                reCreatePie(pattern);
                you_transit_time.Text = fullTextForTransit; // set text
            }
        }

        private void PlotGraph(ObservableCollection<UserPoopData> userPoopData)
        {
            if (_data != null)
            {
                _data.Clear();
                _data = userPoopData;
            }

        }

        private static DateTime previousInputDate;
        private DateTime inputDate;
        private static readonly TimeSpan START_TIME_OF_DAYS = new TimeSpan(0, 0, 0);
        private bool isInit;
        private ListPicker[] pickers = new ListPicker[5];
        public ObservableCollection<UserPoopData> Data { get { return _data; } }
        private ObservableCollection<UserPoopData> _data = new ObservableCollection<UserPoopData>();
        private List<Poop_Table_SQLite> userLastestPoopDataInSQLite = null;
        private SQLiteFunctions sqliteFunctions = new SQLiteFunctions();
        private Dictionary<string, int> poopColor_dictionary = new Dictionary<string, int>(){
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
        private Dictionary<string, int> poopShape_dictionary = new Dictionary<string, int>(){
            {"Separated hard lumps", 1},
            {"Lumpy sausage", 2},
            {"Cracked surface sausage", 3},
            {"Smooth soft snake", 4},
            {"Soft blobs with clear cut", 5},
            {"Mushy and fluffy pieces", 6},
            {"Entirely liquid", 7}
        };
        private Dictionary<string, int> painLevel_dictionary = new Dictionary<string, int>(){
            {"None", 1},
            {"Mild", 2},
            {"Moderate", 3},
            {"Severe", 4},
            {"Worst", 5}
        };
        private Dictionary<string, int> bloodAmount_dictionary = new Dictionary<string, int>(){
            {"None", 1},
            {"Little blood", 2},
            {"Medium blood", 3},
            {"Much blood", 4},
            {"A lot of blood", 5}
        };

        private Dictionary<string, int> you_transit_dictionary = new Dictionary<string, int>{
            {"12_AM", 0}, // midnight = 0:00 - 0:59  pattern 1
            {"01_AM", 0}, // 1:00 - 1:59 pattern 2
            {"02_AM", 0}, // 2:00 - 2:59 pattern 3
            {"03_AM", 0}, // 3:00 - 3:59 pattern 4
            {"04_AM", 0}, // 4:00 - 4:59 pattern 5
            {"05_AM", 0}, // 5:00 - 5:59 pattern 6
            {"06_AM", 0}, // 6:00 - 6:59 pattern 7
            {"07_AM", 0}, // 7:00 - 7:59 pattern 8
            {"08_AM", 0}, // 8:00 - 8:59 pattern 9
            {"09_AM", 0}, // 9:00 - 9:59 pattern 10
            {"10_AM", 0}, // 10:00 - 10:59 pattern 11
            {"11_AM", 0}, // 11:00 - 11:59 pattern 12
            {"12_PM", 0}, // noon = 12:00 - 12:59 pattern 1
            {"01_PM", 0}, // 13:00 - 13:59 pattern 2
            {"02_PM", 0}, // 14:00 - 14:59 pattern 3
            {"03_PM", 0}, // 15:00 - 15:59 pattern 4
            {"04_PM", 0}, // 16:00 - 16:59 pattern 5
            {"05_PM", 0}, // 17:00 - 17:59 pattern 6
            {"06_PM", 0}, // 18:00 - 18:59 pattern 7
            {"07_PM", 0}, // 19:00 - 19:59 pattern 8
            {"08_PM", 0}, // 20:00 - 20:59 pattern 9
            {"09_PM", 0}, // 21:00 - 21:59 pattern 10
            {"10_PM", 0}, // 22:00 - 22:59 pattern 11
            {"11_PM", 0}, // 23:00 - 23:59 pattern 12
        };
    }

    public class UserPoopData
    {
        public int color { get; set; }
        public int shape { get; set; }
        public int bloodAmount { get; set; }
        public int painLevel { get; set; }
    }

    public class PData
    {
        public string title { get; set; }
        public double value { get; set; }
    }
}