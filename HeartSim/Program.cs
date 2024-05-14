using HeartSim.classes.DataAndTypes;
using HeartSim.classes.HeartNS;
using System;
using System.Threading;
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


      // Start a new thread to update the colors
      Thread thread = new Thread(() =>
      {
        while (true)
        {
          // Update the colors...
          // Note: You need to use Invoke or BeginInvoke to update the UI from this thread
          //lock (form_lock)
          //{
            for (int j = 0; j < 1000; ++j)
            {
              myheart.HeartAutomaton();
              form.points_loc.Clear();
              form.points_color.Clear();
              for (int i = 0; i < Data.NodePositions.Count; ++i)
              {
                form.points_loc.Add(new Position { X = Data.NodePositions[i].X, Y = (Data.NodePositions[i].Y) });
                form.points_color.Add((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1);
              }
              string updateString = "";
              for (int i = 0; i < Data.NodeNames.Count; ++i)
              {
                //txtbx.Invoke(new Action(() => form.SetTextBox1(txtbx.Text + ((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1).ToString() + " ")));
                //txtbx.Text += ((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1).ToString() + " ";
                updateString += ((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1).ToString() + " ";
              }
              txtbx.Invoke((MethodInvoker)delegate
                        {
                          txtbx.Text = updateString;
                        });
              //txtbx.Text += "\n";
              form.RedrawPictureBox();
            }
          //}
          // Sleep for a while
          //Thread.Sleep(2);
        }
      });
      thread.Start();

      Application.Run(form);

    }
  }
}
