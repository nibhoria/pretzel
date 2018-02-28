using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace PreTzeL
{
    class Class_GlobalVars
    {
        //public static string Client_host;
        //public static string Client_port;
        //public static string Client_MySql_username;
        //public static string Client_MySql_password;
        //public static string Client_MySql_portNo;
        //public static string Client_MySql_DataSource;
        public static string database_type;
        public static string Client_Conn_string;
        public static string username;
        public static string password;
        public static string Conn_string = "datasource=localhost;port=3306;username=root; password=root;";
        public static bool SaveDetails;
        public static bool ReturnBackToMainwindow;
    }

    public class view_Customers
    {
        public string Tool_no { get; set; }
        public string Customer_name { get; set; }
        public string Site_address { get; set; }
        public string process_type { get; set; }
        public string total_zones { get; set; }
        public string total_PTL { get; set; }
        public string max_PTL { get; set; }
        public string HHT { get; set; }
        public string img_path { get; set; }
        public string License_from { get; set; }
        public string License_to { get; set; }
    }

    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
            {
                System.Windows.Forms.MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            SendKeys.SendWait("{Enter}");
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    public class client_TableAndColumn
    {
        public string client_col { get; set; }
        public string client_table { get; set; }
        public List<string> destination_col { get; set; }
        public List<string> destination_table { get; set; }
    }
}
