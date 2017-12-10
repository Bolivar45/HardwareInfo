using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace HardwareInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 161, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            HInfo hInfo = new HInfo();
            
            string serialized = JsonConvert.SerializeObject(hInfo);
          
            string result = jsonSend(serialized);
           


            if (result == "{\"success\":true}")
            {
                MessageBox.Show("Данные собраны!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result != "")
            {
                MessageBox.Show(result, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public string jsonSend(string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.overpro.ru/api/activator/put");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string result = "";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return result;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
