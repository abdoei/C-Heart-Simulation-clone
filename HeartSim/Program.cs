using HeartSim.classes.DataAndTypes;
using HeartSim.classes.HeartNS;
using System;
using System.Windows.Forms;
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
      //PathTable pathTable = new PathTable(Data.PathNames, Data.PathIntParameters, Data.PathDoubleParameters);
      //NodeTable nodeTable = new NodeTable(Data.NodeNames, Data.NodeIntParameters, Data.NodePositions, pathTable);

      Heart myheart = Heart.GetInstance();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      var form = new Form1();
      var txtbx = form.GetTextBox1();

      for (int j = 0; j < 1000; ++j)
      {
        myheart.HeartAutomaton();
        for (int i = 0; i < Data.NodeNames.Count; ++i)
        {
          txtbx.Text += ((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1).ToString() + " ";
        }
        txtbx.Text += "\n";
      }

      Application.Run(form);

    }
  }
}
