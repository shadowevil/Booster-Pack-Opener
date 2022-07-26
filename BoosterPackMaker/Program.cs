using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BoosterPackMaker
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            using (Form1 cardWindow = new Form1())
            {
                cardWindow.ShowDialog();
            }
        }
    }
}
