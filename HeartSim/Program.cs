using HeartSim.classes;
using System;
using System.Windows.Forms;
using HeartSim.classes.PathNS;

namespace HeartSim
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      //Class2 c2 = new Class2();
      //MyClass1 myClass1 = new MyClass1();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }
  }
}
