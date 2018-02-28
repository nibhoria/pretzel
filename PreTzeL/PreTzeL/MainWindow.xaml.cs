using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PreTzeL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    public static class BlurElementExtension
    {
        /// <summary>
        /// Turning blur on
        /// </summary>
        /// <param name="element">bluring element</param>
        /// <param name="blurRadius">blur radius</param>
        /// <param name="duration">blur animation duration</param>
        /// <param name="beginTime">blur animation delay</param>
        public static void BlurApply(this UIElement element,
            double blurRadius, TimeSpan duration, TimeSpan beginTime)
        {
            BlurEffect blur = new BlurEffect() { Radius = 0 };
            DoubleAnimation blurEnable = new DoubleAnimation(0, blurRadius, duration)
            { BeginTime = beginTime };
            element.Effect = blur;
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurEnable);
        }
        /// <summary>
        /// Turning blur off
        /// </summary>
        /// <param name="element">bluring element</param>
        /// <param name="duration">blur animation duration</param>
        /// <param name="beginTime">blur animation delay</param>
        public static void BlurDisable(this UIElement element, TimeSpan duration, TimeSpan beginTime)
        {
            BlurEffect blur = element.Effect as BlurEffect;
            if (blur == null || blur.Radius == 0)
            {
                return;
            }
            DoubleAnimation blurDisable = new DoubleAnimation(blur.Radius, 0, duration) { BeginTime = beginTime };
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurDisable);
        }
    }

    public class client_column
    {
        public string client_col { get; set; }
        public List<string> cb10 { get; set; }
    }
    public class grid1
    {
        public string value { get; set; }
    }

    public partial class MainWindow : Window
    {
        bool processType_put_state = false;
        bool processType_pick_state = false;
        bool ptlType_WithPlusMinus = false;
        bool ptlType_WithoutPlusMinus = false;
        bool ptlType_SteadyGlow = false;
        bool ptlType_BlinkingGlow = false;
        bool ptlType_SingleRGB = false;
        bool ptlType_numeric = false;
        bool ptlType_alphaNumeric = false;
        bool ptlConfirmation_No_Scan_and_Button_Press = false;
        bool ptlConfirmation_Some_Scan_No_Button_Press = false;
        bool ptlConfirmation_Button_Press = false;
        bool ptlConfirmation_Next_Scan = false;
        bool ptlConfirmation_Single_Scan = false;
        bool ptlMapping_1ToMany = false;
        bool ptlMapping_OneToOne = false;
        bool barcodeScanner_YesScanner = false;
        bool barcodeScanner_NoScanner = false;
        bool OperationMode_packIcon_loginEnable = false;
        bool OperationMode_packIicon_loginDisable = false;
        
        ObservableCollection<view_Customers> customer = new ObservableCollection<view_Customers>
        {
            
        };

        ObservableCollection<client_TableAndColumn> clientTableAndColumn = new ObservableCollection<client_TableAndColumn>
        {

        };

        public MainWindow()
        {
            InitializeComponent();
        }

        static System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBX_AppType.Items.Clear();
            comboBX_AppType.Items.Insert(0, "--Select--");
            comboBX_AppType.Items.Insert(1, "Destination Sorting (1 to 1)");
            comboBX_AppType.Items.Insert(2, "Order Sorting (1 to many)");
            comboBX_AppType.Items.Insert(3, "Order Picking – Zone Wise");
            myTimer.Interval = new TimeSpan(0, 1, 0); // 1 Minute
                                                      // Set Timer Event 
                                                      // timer to show message window every x seconds
            myTimer.Tick += new EventHandler(TimerEventProcessor);

            // Start timer
            myTimer.Start();

            ComboBox_DBtype.Items.Clear();
            ComboBox_DBtype.Items.Insert(0,"--select--");
            ComboBox_DBtype.Items.Insert(1, "My SQL");
            ComboBox_DBtype.Items.Insert(2, "SQL");
            ComboBox_DBtype.Items.Insert(3, "Oracle");
            ComboBox_DBtype.SelectedIndex = 0;
        }

        private static void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            ShowMessage("Please check on the status");
        }

        protected static void ShowMessage(string Message)
        {
            MessageWindow.Show("details saved, Will move to next page in: ", "Message", 10);
            while (Class_GlobalVars.ReturnBackToMainwindow == false) { }
            if(Class_GlobalVars.SaveDetails == true)
            {
                MessageBox.Show("function to save details will be executed!");
            }
        }

        private void menuItem_procType_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 0;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void closeButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(Tab_Control.SelectedIndex.ToString());

            Details_TxtBox1.Text = "For detailed information on PUT/PICK systems,";
            Run run3 = new Run("Click Here");
            Hyperlink hyperlink = new Hyperlink(run3)
            {
                NavigateUri = new Uri("https://www.falconautoonline.com/put-to-light-systems/")
            };
            hyperlink.RequestNavigate += new RequestNavigateEventHandler(hyperlink_RequestNavigate);
            Details_TxtBox2.Inlines.Clear();
            Details_TxtBox2.Inlines.Add(hyperlink);
            Details_TxtBox3.Text = "";
            Details_TxtBox4.Text = "";

            var currentTabitem = (TabItem)Tab_Control.SelectedItem;
            if (Convert.ToString(currentTabitem.Name) == "tab_CreateNewCustomer")
            {
                btn_Next.Content = "SAVE  ›";
                btn_Next.ToolTip = "SAVE and proceed on editing configuration";
            }
            else
            {
                btn_Next.Content = "NEXT  ›";
                btn_Next.ToolTip = null;
            }
        }

        private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void comboBX_AppType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBX_AppType.SelectedItem.ToString() == "Destination Sorting (1 to 1)" || comboBX_AppType.SelectedItem.ToString() == "Order Sorting (1 to many)")
            {
                if(comboBX_AppType.SelectedItem.ToString() == "Destination Sorting (1 to 1")
                {
                    toggle_YesScanner.IsChecked = true;
                    toggle_NoScanner.IsEnabled = false;
                }
                toggle_PutToLight.IsChecked = true;
                tab_Pick_ConfirmationTyp.IsEnabled = false;
                tab_Put_ConfirmationTyp.Visibility = Visibility.Visible;
            }
            
            if (comboBX_AppType.SelectedItem.ToString() == "Order Picking – Zone Wise")
            {
                toggle_PickToLight.IsChecked = true;
                tab_Pick_ConfirmationTyp.Visibility = Visibility.Visible;
            }

            if (comboBX_AppType.SelectedItem.ToString() != "--Select--")
            {
                if (comboBX_AppType.SelectedItem.ToString() == "Destination Sorting (1 to 1)")
                {
                    toggle_OneTime.IsChecked = true;
                    toggle_EveryZoneScan.IsEnabled = false;
                    TabControl_Config.SelectedItem = tab_Ptl_type;
                }
                else
                {
                    TabControl_Config.SelectedIndex += 1;
                }
            }   
        }

        private void toggle_PickToLight_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_PickToLight.IsChecked == true)
            {
                toggle_PutToLight.IsChecked = false;
            }
            if (!processType_pick_state)
            {
                pick_PackIcon.Foreground = Brushes.White;
                processType_pick_state = true;
            }
        }

        private void toggle_PutToLight_Checked(object sender, RoutedEventArgs e)
        {

            if (toggle_PutToLight.IsChecked == true)
            {
                toggle_PickToLight.IsChecked = false;
            }

            if (!processType_put_state)
            {
                put_PackIcon.Foreground = Brushes.White;
                processType_put_state = true;
            }
        }

        private void toggle_WithPlusMinus_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_WithPlusMinus.IsChecked == true)
            {
                toggle_WithoutPlusMinus.IsChecked = false;
            }
            if (!ptlType_WithPlusMinus)
            {
                packIcon_WithPlusMinus.Foreground = Brushes.White;
                ptlType_WithPlusMinus = true;
            }

        }

        private void toggle_WithoutPlusMinus_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_WithoutPlusMinus.IsChecked == true)
            {
                toggle_WithPlusMinus.IsChecked = false;
            }
            if (!ptlType_WithoutPlusMinus)
            {
                packIcon_WithoutPlusMinus.Foreground = Brushes.White;
                ptlType_WithoutPlusMinus = true;
            }
        }

        private void toggle_Button_Press_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_Button_Press.IsChecked == true)
            {
                toggle_Some_Scan_No_Button_Press.IsChecked = false;
                toggle_No_Scan_and_Button_Press.IsChecked = false;
            }
            if (!ptlConfirmation_Button_Press)
            {
                packIcon_Button_Press.Foreground = Brushes.White;
                ptlConfirmation_Button_Press = true;
            }
        }

        private void toggle_Some_Scan_No_Button_Press_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_Some_Scan_No_Button_Press.IsChecked == true)
            {
                toggle_Button_Press.IsChecked = false;
                toggle_No_Scan_and_Button_Press.IsChecked = false;
            }
            if (!ptlConfirmation_Some_Scan_No_Button_Press)
            {
                packIcon_Some_Scan_No_Button_Press.Foreground = Brushes.White;
                ptlConfirmation_Some_Scan_No_Button_Press = true;
            }
        }

        private void toggle_No_Scan_and_Button_Press_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_No_Scan_and_Button_Press.IsChecked == true)
            {
                toggle_Button_Press.IsChecked = false;
                toggle_Some_Scan_No_Button_Press.IsChecked = false;
            }
            if (!ptlConfirmation_No_Scan_and_Button_Press)
            {
                packIcon_No_Scan_and_Button_Press.Foreground = Brushes.White;
                ptlConfirmation_No_Scan_and_Button_Press = true;
            }
        }

        private void tab_Pick_ConfirmationTyp_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void toggle_OneToOne_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_OneToOne.IsChecked == true)
            {
                toggle_OneToMany.IsChecked = false;
            }
            if (!ptlMapping_OneToOne)
            {
                packIcon_OneToOne.Foreground = Brushes.White;
                ptlMapping_OneToOne = true;
            }
        }

        private void toggle_OneToMany_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_OneToMany.IsChecked == true)
            {
                toggle_OneToOne.IsChecked = false;
            }
            if (!ptlMapping_1ToMany)
            {
                packIcon_1ToMany.Foreground = Brushes.White;
                ptlMapping_1ToMany = true;
            }
        }

        private void toggle_YesScanner_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_YesScanner.IsChecked == true)
            {
                toggle_NoScanner.IsChecked = false;
            }
            if (!barcodeScanner_YesScanner)
            {
                packIcon_YesScanner.Foreground = Brushes.White;
                barcodeScanner_YesScanner = true;
            }
        }

        private void toggle_NoScanner_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_NoScanner.IsChecked == true)
            {
                toggle_YesScanner.IsChecked = false;
            }
            if (!barcodeScanner_NoScanner)
            {
                packIcon_NoScanner.Foreground = Brushes.White;
                barcodeScanner_NoScanner = true;
            }
        }

        private void toggle_SteadyGlow_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_SteadyGlow.IsChecked == true)
            {
                toggle_BlinkingMode.IsChecked = false;
            }
            if (!ptlType_SteadyGlow)
            {
                packIcon_SteadyGlow.Foreground = Brushes.White;
                ptlType_SteadyGlow = true;
            }
        }

        private void toggle_BlinkingMode_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_BlinkingMode.IsChecked == true)
            {
                toggle_SteadyGlow.IsChecked = false;
            }
            if (!ptlType_BlinkingGlow)
            {
                packIcon_BlinkingMode.Foreground = Brushes.White;
                ptlType_BlinkingGlow = true;
            }
        }

        private void toggle_NumericDisplay_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_NumericDisplay.IsChecked == true)
            {
                toggle_AlphaNuericDisplay.IsChecked = false;
            }
            if (!ptlType_numeric)
            {
                packIcon_Numeric.Foreground = Brushes.White;
                ptlType_numeric = true;
            }
        }

        private void toggle_AlphaNumericDisplay_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_AlphaNuericDisplay.IsChecked == true)
            {
                toggle_NumericDisplay.IsChecked = false;
            }
            if (!ptlType_alphaNumeric)
            {
                packIcon_AlphaNumeric.Foreground = Brushes.White;
                ptlType_alphaNumeric = true;
            }
        }

        private void menuItem_ptlType_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 1;
        }

        private void menuItem_ptlConfirm_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 2;
        }

        private void menuItem_ptlMapping_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 3;
        }

        private void menuItem_BarcdScanner_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 4;
        }

        private void menuItem_PtlOperationMode_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedIndex = 5;
        }

        private void toggle_loginEnable_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_loginEnable.IsChecked == true)
            {
                toggle_loginDisable.IsChecked = false;
            }
            if (!OperationMode_packIcon_loginEnable)
            {
                packIcon_loginEnable.Foreground = Brushes.White;
                OperationMode_packIcon_loginEnable = true;
            }
        }

        private void toggle_loginDisable_Checked(object sender, RoutedEventArgs e)
        {
            if (toggle_loginDisable.IsChecked == true)
            {
                toggle_loginEnable.IsChecked = false;
            }
            if (!OperationMode_packIicon_loginDisable)
            {
                packIicon_loginDisable.Foreground = Brushes.White;
                OperationMode_packIicon_loginDisable = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Tab_Control.IsEnabled = false;
            btn_Next.IsEnabled = false;
            var currentTabitem = (TabItem)Tab_Control.SelectedItem;
            if (currentTabitem.Name == "tab_CreateNewCustomer")
            {
                string processtype = "";
                if (toggle_CreateCustomerPickToLight.IsChecked == true)
                {
                    processtype = "Pick To Light";
                }
                else if (toggle_CreateCustomerPutToLight.IsChecked == true)
                {
                    processtype = "Put To Light";
                }
                bool hht = false;
                if (toggle_HHT.IsChecked == true)
                {
                    hht = true;
                }
                MySqlConnection connection = new MySqlConnection(Class_GlobalVars.Conn_string);
                string query_InsertIntoCustomerMaster = "insert into customer_info.customer_master (toolno,customer_name,site_address,process_type,total_zones,maxPTL,totalPTL,HHT,license_from,license_to,logo_path) values(@toolno,@customername,@siteAddress,@processType,@totalzones,@maxPTLinzone,@totalPTL,@HHT,@from,@to,@imageDestination)";
                using (MySqlCommand cmd = new MySqlCommand(query_InsertIntoCustomerMaster, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@toolno", Convert.ToString(textBox_toolno.Text));
                    cmd.Parameters.AddWithValue("@customername", Convert.ToString(textBox_customerName.Text));
                    cmd.Parameters.AddWithValue("@siteAddress", Convert.ToString(textBox_SiteAddress.Text));
                    cmd.Parameters.AddWithValue("@processType", processtype);
                    cmd.Parameters.AddWithValue("@totalzones", Convert.ToString(textBox_TotalZones.Text));
                    cmd.Parameters.AddWithValue("@maxPTLinzone", Convert.ToString(textBox_MaxPtl.Text));
                    cmd.Parameters.AddWithValue("@totalPTL", Convert.ToString(textBox_TotalPtl.Text));
                    cmd.Parameters.AddWithValue("@HHT", hht);
                    cmd.Parameters.AddWithValue("@from", datePicker_LicenseKey_from.DisplayDate);
                    cmd.Parameters.AddWithValue("@to", datePicker_LicenseKey_to.DisplayDate);
                    cmd.Parameters.AddWithValue("@imageDestination", Convert.ToString(img_CustomerImage.ToolTip));
                    MySqlDataReader reader = cmd.ExecuteReader();
                    connection.Close();

                    //string from = (datePicker_LicenseKey_from.DisplayDate).ToString("yyyy-MM-dd");
                    //string to = (datePicker_LicenseKey_to.DisplayDate).ToString("yyyy-MM-dd");
                }
            }
            Tab_Control.IsEnabled = true;
            btn_Next.IsEnabled = true;
            this.Cursor = Cursors.Arrow;
            Tab_Control.SelectedIndex += 1;
            //this.Cursor = Cursors.Arrow;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if (Tab_Control.SelectedIndex >= 1)
                Tab_Control.SelectedIndex -= 1;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void toggle_AlphaNuericDisplay_Click(object sender, RoutedEventArgs e)
        {



        }

        private void minimizeButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }



        private void btn_additional_details_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_ClientDetails.Visibility == Visibility.Hidden)
            { if (Tab_Control.SelectedIndex == 1)
                {
                    Tab_Control.BlurApply(100, new TimeSpan(0, 0, 0, 0, 200), TimeSpan.Zero);
                    Grid_ClientDetails.Visibility = Visibility.Visible;
                }
                else
                {
                    Tab_Control.BlurApply(30, new TimeSpan(0, 0, 0, 0, 200), TimeSpan.Zero);
                    Grid_ClientDetails.Visibility = Visibility.Visible;
                }
                
                
                
            }

            else
            {
                Tab_Control.BlurDisable(new TimeSpan(0, 0, 0, 0, 30), TimeSpan.Zero);
                Grid_ClientDetails.Visibility = Visibility.Hidden;
            }

            MySqlConnection connection = new MySqlConnection(Class_GlobalVars.Conn_string);
            string query_SelectFromColumnMaster = "select * from customer_info.customer_master";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query_SelectFromColumnMaster, connection);
                command.ExecuteNonQuery();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("fapl_customers.customer_master");
                adapter.Fill(dt);
                List<view_Customers> items = new List<view_Customers>();
                foreach (DataRowView dr in dt.DefaultView)
                {
                    items.Add(new view_Customers()
                    {
                        Tool_no = Convert.ToString(dr[1]),
                        Customer_name = Convert.ToString(dr[2]),
                        Site_address = Convert.ToString(dr[3]),
                        process_type = Convert.ToString(dr[4]),
                        total_zones = Convert.ToString(dr[5]),
                        max_PTL = Convert.ToString(dr[6]),
                        total_PTL = Convert.ToString(dr[7]),
                        HHT = Convert.ToString(dr[8]),
                        License_from = Convert.ToDateTime(Convert.ToString(dr[9])).ToString("dd/MM/yyyy"),
                        License_to = Convert.ToDateTime(Convert.ToString(dr[10])).ToString("dd/MM/yyyy"),
                        img_path = Convert.ToString(dr[11]),

                    });
                }
                itemsControl_ViewCustomers.ItemsSource = items;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(itemsControl_ViewCustomers.ItemsSource);
                view.Filter = UserFilter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void frame_Add_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Grid_ClientDetails_Loaded(object sender, RoutedEventArgs e)
        {
            
            //add textblock to grid
            //DynamicGrid.Children.Add(txtBlock1);

        }

        private void btn_close_AddGrid_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.BlurDisable(new TimeSpan(0, 0, 0, 0, 30), TimeSpan.Zero);
            Grid_ClientDetails.Visibility = Visibility.Hidden;
        }

        private void btn_loginStatement_Click(object sender, RoutedEventArgs e)
        {
            btn_login.Visibility = Visibility.Visible;
            btn_signup.Visibility = Visibility.Hidden;
            btn_loginStatement.Visibility = Visibility.Hidden;
        }

        private void btn_signup_Click(object sender, RoutedEventArgs e)
        {
            string query_createDatabase = "create database pretzel_T102";
            MySqlConnection connection = new MySqlConnection(Class_GlobalVars.Conn_string);
            //MessageBox.Show(Class_GlobalVars.Conn_string);
            using (MySqlCommand cmd = new MySqlCommand(query_createDatabase, connection))
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                connection.Close();
            }
            string query_createTableClientMaster = "create table pretzel_T102.user_master (username varchar(20), password varchar(30));";
            using (MySqlCommand cmd = new MySqlCommand(query_createTableClientMaster, connection))
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                connection.Close();
            }
            string query_insertUsernamePass = "insert into pretzel_T102.user_master (username, password) values('"+Class_GlobalVars.username+"','"+Class_GlobalVars.password+"')";
            using (MySqlCommand cmd = new MySqlCommand(query_insertUsernamePass, connection))
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                connection.Close();
            }
        }

        private void textBox_ServerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void textBox_PortNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void textBox_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            Class_GlobalVars.username = textBox_username.Text;
        }

        private void textBox_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Class_GlobalVars.password = textBox_password.Password;
        }

        private void TabPanel_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openfile = new Microsoft.Win32.OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";
            //openfile.ShowDialog();

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                txtFilePath.Text = openfile.FileName;

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //Static File From Base Path...........
                //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(txtFilePath.Text.ToString(), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;
                BitmapImage tick_img = new BitmapImage();
                tick_img = new BitmapImage(new Uri(@"pack://application:,,,/tick.bmp"));
                BitmapImage cross_img = new BitmapImage();
                cross_img = new BitmapImage(new Uri(@"pack://application:,,,/cross.bmp"));
                DataTable dt = new DataTable();
                
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                    //MessageBox.Show(strColumn);
                    dt.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += strCellData + "|";
                            //MessageBox.Show(strData);
                        }
                        catch (Exception ex)
                        {
                            douCellData = (double)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    string [] strdata = strData.Split('|');
                    string name = strdata[0];
                    string ip1 = strdata[1];
                    string ip2 = strdata[2];
                    dt.Rows.Add(name,ip1,ip2);
                }
                dt.Columns.Add("Img", typeof(BitmapImage)).SetOrdinal(0);
                dtGrid.ItemsSource = dt.DefaultView;

                excelBook.Close(true, null, null);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 1 || j == 2)
                        {
                            string ip1 = Convert.ToString(row[j]);
                            string[] str = null;
                            str = ip1.Split('.');
                            //MessageBox.Show(Convert.ToString(str.Length));
                            if (str.Length == 4)
                            {
                                row[0]=tick_img;
                            }
                            else
                            {
                                row[0] = cross_img;
                            }
                        }
                    }
                }
                dtGrid.ItemsSource = dt.DefaultView;
                excelApp.Quit();
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add("C:\\Users\\Nikhil_Nibhoria\\Desktop\\wpf_test.xlsx");
            ExcelApp.Columns.ColumnWidth = 30;
            DataTable dtdata = ((DataView)dtGrid.ItemsSource).ToTable().DefaultView.Table;
            //DataGridView dgv = new DataGridView();
            //dgv.DataSource = ((DataView)dtGrid.ItemsSource).ToTable();
            
            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                DataRow row = dtdata.Rows[i];
                for (int j = 0; j < dtdata.Columns.Count; j++)
                {
                    ExcelApp.Cells[i + 2, j + 1] = row[j].ToString();
                }
            }
            ExcelApp.ActiveWorkbook.SaveCopyAs("C:\\Users\\Nikhil_Nibhoria\\Desktop\\this.xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        }

        private void dtGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Img")
            {
                // replace text column with image column
                e.Column = new DataGridTemplateColumn
                {
                    // searching for predefined tenplate in Resources
                    CellTemplate = (sender as DataGrid).Resources["ImgCell"] as DataTemplate,
                    HeaderTemplate = e.Column.HeaderTemplate,
                    Header = e.Column.Header
                };
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            //Class_GlobalVars.Conn_string = "datasource=localhost;port=3306;username=root; password=root;";
            MySqlConnection connection = new MySqlConnection(Class_GlobalVars.Conn_string);
            //MessageBox.Show(Class_GlobalVars.Conn_string);
            try
            {
                string query_CheckUsername = "select count(*) from user_info.user_master where username= @username";
                int count = 0;
                using (MySqlCommand cmd = new MySqlCommand(query_CheckUsername, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@username", textBox_username.Text.ToString());
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader[0]);
                    }
                    connection.Close();
                }
                if (count == 0)
                {
                    throw new Exception("Invalid User Name");
                }
                string query_CheckPass = "select pass from user_info.user_master where username = @username;";
                string password = "";
                using (MySqlCommand cmd = new MySqlCommand(query_CheckPass, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@username", textBox_username.Text.ToString());
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        password = Convert.ToString(reader[0]);
                    }
                    connection.Close();
                }
                if(password == textBox_password.Password.ToString())
                {
                    MessageBox.Show("Successfully logged in");
                    btn_Next.IsEnabled = true;
                    btn_Back.IsEnabled = true;
                    tab_ProcessType.IsEnabled = true;
                    tab_Ptl_type.IsEnabled = true;
                    tab_Pick_ConfirmationTyp.IsEnabled = true;
                    tab_PTL_Mapping_Type.IsEnabled = true;
                    tab_Barcode.IsEnabled = true;
                    tab_PTLOperationMode.IsEnabled = true;
                    tab_ExcelUplaod.IsEnabled = true;
                    Tab_Control.SelectedIndex += 1;
                }
                else
                {
                    throw new Exception("Incorrect password");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Main_grid_Loaded(object sender, RoutedEventArgs e)
        {
            tab_Login.IsEnabled = true;
            btn_Next.IsEnabled = false;
            btn_Back.IsEnabled = false;
            tab_ProcessType.IsEnabled = false;
            tab_Ptl_type.IsEnabled = false;
            tab_Pick_ConfirmationTyp.IsEnabled = false;
            tab_PTL_Mapping_Type.IsEnabled = false;
            tab_Barcode.IsEnabled = false;
            tab_PTLOperationMode.IsEnabled = false;
            tab_ExcelUplaod.IsEnabled = false;
        }

        private void btn_CreateNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            Tab_Control.SelectedItem = tab_CreateNewCustomer;
        }

        private void btn_BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".png";
            openfile.Filter = "(.png, .jpg)|*.png;*.jpg";
            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                img_CustomerImage.ToolTip = openfile.FileName;
                img_CustomerImage.Source = new BitmapImage(new Uri(openfile.FileName));
                File.Copy(openfile.FileName, @"C: \Users\Administrator\Downloads\this.png", true);
            }
        }
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        public static bool IsTextNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void textBox_searchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(itemsControl_ViewCustomers.ItemsSource).Refresh();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBox_searchCustomer.Text))
                return true;
            else
                return ((item as view_Customers).Customer_name.IndexOf(textBox_searchCustomer.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void toggle_MasterSlave_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void textBlock_notAllowed_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void textBlock_Allowed_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (textBlock_Allowed.Visibility == Visibility.Visible)
            {
                textBlock_notAllowed.Visibility = Visibility.Hidden;
            }
            if (textBlock_Allowed.Visibility == Visibility.Collapsed)
            {
                textBlock_notAllowed.Visibility = Visibility.Visible;
            }
        }

        private void ComboBox_DBtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_DBtype.SelectedItem.ToString() == "My SQL")
            {
                Class_GlobalVars.database_type = "My SQL";
                ShowConfigWindow("Create Connection");
                List<string> list = new List<string>();
                string query_showtables = "SHOW tables;";
                //MessageBox.Show(db_listbox1.SelectedItem.ToString());
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Class_GlobalVars.Client_Conn_string))
                    {
                        MySqlCommand cmd = new MySqlCommand(query_showtables, conn);
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(reader[0].ToString());
                        }
                        conn.Close();
                    }
                    ComboBox_clientTable.ItemsSource = list;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (ComboBox_DBtype.SelectedItem.ToString() == "SQL")
            {
                Class_GlobalVars.database_type = "SQL";
                ShowConfigWindow("Create Connection");
            }
            else if (ComboBox_DBtype.SelectedItem.ToString() == "Oracle")
            {
                Class_GlobalVars.database_type = "Oracle";
                ShowConfigWindow("Create Connection");
            }
            
        }
        protected static void ShowConfigWindow(string Message)
        {
            DbConfigDetails.Show1(Message);
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            string query_showDestinationTables = "show tables from f102;";
            using (MySqlConnection conn = new MySqlConnection(Class_GlobalVars.Conn_string))
            {
                MySqlCommand cmd = new MySqlCommand(query_showDestinationTables, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                conn.Close();
            }
            ComboBox_destinationTable.ItemsSource = list;
        }

        private void ComboBox_destinationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox_clientTable.SelectedItem == null)
            {
                MessageBox.Show("Please Select Client's Table First");
                return;
            }

            else
            {
                List<string> list1 = new List<string>();
                string query_showClientCols = "SELECT column_name FROM information_schema.columns WHERE table_name = @tablename ; ";
                using (MySqlConnection conn = new MySqlConnection(Class_GlobalVars.Conn_string))
                {
                    MySqlCommand cmd = new MySqlCommand(query_showClientCols, conn);
                    cmd.Parameters.AddWithValue("@tablename", ComboBox_clientTable.SelectedItem.ToString());
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list1.Add(reader[0].ToString());
                    }
                    conn.Close();
                }
                List<string> list2 = new List<string>();
                string query_showDestinationCols = "SELECT column_name FROM information_schema.columns WHERE table_schema = 'f102' AND table_name = @tablename; ";
                using (MySqlConnection conn = new MySqlConnection(Class_GlobalVars.Conn_string))
                {
                    MySqlCommand cmd = new MySqlCommand(query_showDestinationCols, conn);
                    cmd.Parameters.AddWithValue("@tablename", ComboBox_destinationTable.SelectedItem.ToString());
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list2.Add(reader[0].ToString());
                    }
                    conn.Close();
                }
                foreach (string i in list1)
                {
                    clientTableAndColumn.Add(new client_TableAndColumn { client_col = i, destination_col = list2 });
                }
                datagrid_Mapping.DataContext = clientTableAndColumn;
            }
        }

       

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Class_GlobalVars.Conn_string);
            //string CountTables = "SELECT count(*) FROM information_schema.TABLES WHERE(TABLE_SCHEMA = 'map_test' AND(TABLE_NAME = 'column_master');";
            //int count = 0;
            //using (MySqlCommand cmd = new MySqlCommand(CountTables, conn))
            //{
            //    conn.Open();
            //    MySqlDataReader reader = cmd.ExecuteReader();
            //    while(reader.Read())
            //    {
            //        count = Convert.ToInt32(reader[0]);
            //    }
            //    conn.Close();
            //}
            //if(count==0)
            //{
            //    string query_createColumnMaster = "create table map_test.column_master (column_id int PRIMARY KEY AUTO_INCREMENT, client_column varchar(20), destination_column varchar(100))";
            //    using (MySqlCommand cmd = new MySqlCommand(query_createColumnMaster, conn))
            //    {
            //        conn.Open();
            //        MySqlDataReader reader = cmd.ExecuteReader();
            //        conn.Close();
            //    }
            //}
            var cb = sender as ComboBox;
            string item = cb.SelectedItem.ToString();
            string grid_row = "";
            foreach (client_TableAndColumn row in datagrid_Mapping.SelectedItems)
            {
                grid_row = row.client_col;
            }

            string count_ClientColumn = "select count(*) from map_test.column_master where client_column='" + grid_row + "';";
            int count = 0;
            using (MySqlCommand cmd = new MySqlCommand(count_ClientColumn, conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
                conn.Close();
            }
            if (count == 0)
            {
                string insertIntoColumnMaster = "insert into map_test.column_master (client_column) values('" + grid_row + "')";
                using (MySqlCommand cmd = new MySqlCommand(insertIntoColumnMaster, conn))
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                }
            }
            
                string query_UpdateColumnMaster = "UPDATE map_test.column_master SET destination_column = '" + item + "' WHERE client_column = '" + grid_row + "';";
                using (MySqlCommand cmd = new MySqlCommand(query_UpdateColumnMaster, conn))
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                }
            
        }
    }
}
