using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIME_RECORDING
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static System.Threading.Mutex mutex;
        [STAThread]
        
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);           
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (mutex.WaitOne(0, false))
            {
                
            Application.Run(new Form1());
            
            }
            else
            {
                MessageBox.Show("程序已经运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
              
        }
    }
}
