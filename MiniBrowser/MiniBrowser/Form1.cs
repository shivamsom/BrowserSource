using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; //for the Process
using Microsoft.Win32;  //for the Registry
namespace MiniBrowser
{
    public partial class Form1 : Form

    {
        public Form1()
        {
            var appName = Process.GetCurrentProcess().ProcessName + ".exe";
            SetIE8KeyforWebBrowserControl(appName);
            WebBrowserHelper.FixBrowserVersion();
            WebBrowserHelper.FixBrowserVersion("iexplore.exe");
            WebBrowserHelper.FixBrowserVersion("iexplore.exe", WebBrowserHelper.GetBrowserVersion());
           
            InitializeComponent();
            browser.ScriptErrorsSuppressed = true;
         
        }
       private void SetIE8KeyforWebBrowserControl(string appName)
        {
            RegistryKey Regkey = null;
            try
            {
                // For 64 bit machine
                if (Environment.Is64BitOperatingSystem)
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                else  //For 32 bit machine
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);

                // If the path is not correct or
                // if the user haven't priviledges to access the registry
                if (Regkey == null)
                {
                    MessageBox.Show("Application Settings Failed - Address Not found");
                    return;
                }

                string FindAppkey = Convert.ToString(Regkey.GetValue(appName));

                // Check if key is already present
                if (FindAppkey == "8000")
                {
                    MessageBox.Show("Required Application Settings Present");
                    Regkey.Close();
                    return;
                }

                // If a key is not present add the key, Key value 8000 (decimal)
                if (string.IsNullOrEmpty(FindAppkey))
                    Regkey.SetValue(appName, unchecked((int)0x1F40), RegistryValueKind.DWord);

                // Check for the key after adding
                FindAppkey = Convert.ToString(Regkey.GetValue(appName));

                if (FindAppkey == "8000")
                {    // MessageBox.Show("Application Settings Applied Successfully"); //for testing purpose only.

                }
                else
                {
                    //          MessageBox.Show("Application Settings Failed, Ref: " + FindAppkey); //for testing purpose only.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Application Settings Failed");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Close the Registry
                if (Regkey != null)
                    Regkey.Close();
            }
        }
        private void go_Click(object sender, EventArgs e)
        {
            click();    
        }

        private void back_Click(object sender, EventArgs e)
        {
            if(browser.CanGoBack)
            {
                browser.GoBack();
            }
            else { MessageBox.Show("Reached end"); }
        }

        private void forward_Click(object sender, EventArgs e)
        {
            if (browser.CanGoForward)
            {
                browser.GoForward();
            }
            else { MessageBox.Show("Reached end"); }
        }

        

        public void click()
        {
            if (addressBar.Text != "")//if not empty
            {
                tabControl1.SelectedTab.Controls.OfType<WebBrowser>().First().Navigate(addressBar.Text);
   
            }

            else
                MessageBox.Show("Invalid Url");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            TabPage tab = new TabPage("New Tab");
            tabControl1.Controls.Add(tab);
            WebBrowser br = new WebBrowser();
            br.Parent = tab;
            br.Dock = DockStyle.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }

        private void browser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            try
            {
                progressBar1.Value = Convert.ToInt32(e.CurrentProgress);
                progressBar1.Maximum = Convert.ToInt32(e.MaximumProgress);
            }
            catch
            { }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab.Controls.OfType<WebBrowser>().First().Refresh();
        }
    }
}
