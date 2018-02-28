using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PreTzeL
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }
        public static void Show(string text, string caption, int timeout)
        {
            Class_GlobalVars.ReturnBackToMainwindow = false;
            var msgWindow = new MessageWindow()
            {
                Title = caption,
                Owner = Application.Current.MainWindow
            };

            Task.Factory.StartNew(() =>
            {
                for (var n = 0; n < timeout; n++)
                {
                    msgWindow.Dispatcher.Invoke(() =>
                    {
                        msgWindow.text.Text = string.Format("{0}{1}{2}s", text, Environment.NewLine, timeout - n);
                    });

                    System.Threading.Thread.Sleep(1000);
                }

                msgWindow.Dispatcher.Invoke(msgWindow.Close);
                Class_GlobalVars.SaveDetails = true;
                Class_GlobalVars.ReturnBackToMainwindow = true;
            });

            msgWindow.ShowDialog();
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Class_GlobalVars.SaveDetails = true;
            Class_GlobalVars.ReturnBackToMainwindow = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Class_GlobalVars.SaveDetails = false;
            Class_GlobalVars.ReturnBackToMainwindow = true;
        }

        private bool closeCompleted = false;
        private void FormFadeOut_Completed(object sender, EventArgs e)
        {
            closeCompleted = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}
