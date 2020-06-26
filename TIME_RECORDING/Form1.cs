using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace TIME_RECORDING
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        int time_duration_minutes = 0;
        int time_duration_minutes_slice = 0;
        int back_seq = 1;
        int forward_seq = 0;
        DateTime endtime_of_last_record;
        TimeSpan ts;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = System.DateTime.Now.ToString();
            if (label5.Text!="")
            {
                label4.Text = (System.DateTime.Now - Convert.ToDateTime(label5.Text)).ToString(@"dd\.hh\:mm\:ss");
                time_duration_minutes = (int)((System.DateTime.Now - Convert.ToDateTime(label5.Text)).TotalSeconds);
                int days = int.Parse(label4.Text.Split('.')[0]);
                int hours = int.Parse(label4.Text.Split('.')[1].Split(':')[0]);
                int minutes = int.Parse(label4.Text.Split('.')[1].Split(':')[1]);
                int seconds = int.Parse(label4.Text.Split('.')[1].Split(':')[2]);
                ts = new TimeSpan(days, hours, minutes, seconds);
            }
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            timer1.Start();
            timer2.Start();
            String TasksListCfg_L1 = ConfigurationManager.AppSettings["TasksList_Lv1"];
            String TasksListCfg_L2 = ConfigurationManager.AppSettings["TasksList_Lv2"];
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Items.AddRange(TasksListCfg_L1.Split(','));
            comboBox2.Items.AddRange(TasksListCfg_L2.Split(','));
            label11.Text = System.DateTime.Now.ToString();
            String csvpath = ConfigurationManager.AppSettings["CsvPath"];
            trackBar1.Value = 50;
            label11.Location = new Point(500, 128);
            if (File.Exists(csvpath))
            {
                int lines = 0;
                //string last_time = "";
                using (var sr = new StreamReader(csvpath))
                {
                    var ls = "";
                    while ((ls = sr.ReadLine()) != null)
                        {
                            lines++;
                        }
                    label5.Text = File.ReadAllLines(csvpath, Encoding.Default)[lines-1].Split(',').Last();
                    comboBox1.Text = File.ReadAllLines(csvpath, Encoding.Default)[lines - 1].Split(',')[0];
                    comboBox2.Text = File.ReadAllLines(csvpath, Encoding.Default)[lines - 1].Split(',')[1];
                    richTextBox1.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - 1].Split(',')[2];
                    endtime_of_last_record = Convert.ToDateTime(label5.Text.Split(' ')[0].Replace("/","-")  + " 23:59:59");
                }
                

            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Update UI
            if (comboBox1.Text==""|| comboBox2.Text == "")
            {
                MessageBox.Show("Task can't be empty!");
                return;
            } 
            //richTextBox1的逗号自动替换
            string richTextBox1_str = richTextBox1.Text.Replace(',', '_');
            String csvpath = ConfigurationManager.AppSettings["CsvPath"];
            //如果不存在就创建file文件夹
            if (label5.Text != ""&& label4.Text != "")
            {
                if (File.Exists(csvpath))
                {
                    using (StreamWriter writer = new StreamWriter(csvpath, true))
                    {
                        //判断是否跨天
                        if (endtime_of_last_record< Convert.ToDateTime(label11.Text))
                        {
                            //跨天
                            writer.WriteLine(comboBox1.SelectedItem.ToString() + ',' + comboBox2.SelectedItem.ToString() + ',' + richTextBox1_str + ',' + time_duration_minutes_slice + ',' + label5.Text + ',' + endtime_of_last_record.ToString());
                            time_duration_minutes = (int)(( Convert.ToDateTime(label11.Text) - endtime_of_last_record).TotalSeconds);
                            writer.WriteLine(comboBox1.SelectedItem.ToString() + ',' + comboBox2.SelectedItem.ToString() + ',' + richTextBox1_str + ',' + time_duration_minutes + ',' + endtime_of_last_record.ToString() + ',' + label11.Text);
                        }
                        else if (endtime_of_last_record > Convert.ToDateTime(label11.Text))
                        {
                            //不跨天
                            time_duration_minutes_slice = (int)((Convert.ToDateTime(label11.Text) - Convert.ToDateTime(label5.Text)).TotalSeconds);
                            writer.WriteLine(comboBox1.SelectedItem.ToString() + ',' + comboBox2.SelectedItem.ToString() + ',' + richTextBox1_str + ',' + time_duration_minutes_slice + ',' + label5.Text + ',' + label11.Text);
                        }
                        else if (endtime_of_last_record == Convert.ToDateTime(label11.Text))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(csvpath))
                    {
                        writer.WriteLine("L1, L2, L3, Duration, Start_Time, End_Time");
                        writer.WriteLine(comboBox1.SelectedItem.ToString() + ',' + comboBox2.SelectedItem.ToString() + ',' + richTextBox1_str + ',' + time_duration_minutes + ','+ label5.Text + ',' + label11.Text);
                    }
                }
            }
            this.OnLoad(null);

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            String csvpath = ConfigurationManager.AppSettings["CsvPath"];
            if (File.Exists(csvpath))
            {
                int lines = 0;
                //string last_time = "";
                using (var sr = new StreamReader(csvpath))
                {
                    var ls = "";
                    while ((ls = sr.ReadLine()) != null)
                    {
                        lines++;
                    }
                    
                    
                    if ((lines - back_seq)>=0)
                    {
                        back_seq++;
                        comboBox1.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq].Split(',')[0];
                        comboBox2.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq].Split(',')[1];
                        richTextBox1.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq].Split(',')[2];
                    }
                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            String csvpath = ConfigurationManager.AppSettings["CsvPath"];
            if (File.Exists(csvpath))
            {
                int lines = 0;
                //string last_time = "";
                using (var sr = new StreamReader(csvpath))
                {
                    var ls = "";
                    while ((ls = sr.ReadLine()) != null)
                    {
                        lines++;
                    }


                    if ((lines - back_seq + forward_seq)<(lines-1))
                    {
                        forward_seq++;
                        comboBox1.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq + forward_seq].Split(',')[0];
                        comboBox2.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq + forward_seq].Split(',')[1];
                        richTextBox1.Text = File.ReadAllLines(csvpath, Encoding.UTF8)[lines - back_seq + forward_seq].Split(',')[2];
                    }
                }
            }
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox1.Text);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            
            int x = 100 + 8 * trackBar1.Value;
            label11.Location=new Point(x, label11.Location.Y);           
            DateTime slice_time = Convert.ToDateTime(label5.Text).AddMinutes(ts.TotalMinutes/50* trackBar1.Value);
            time_duration_minutes_slice = (int)(ts.TotalSeconds / 50 * trackBar1.Value);
            label11.Text = slice_time.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.OnLoad(null);
        }
    }
}
