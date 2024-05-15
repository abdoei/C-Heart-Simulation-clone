using HeartSim.classes.DataAndTypes;
using HeartSim.classes.HeartNS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeartSim
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      Heart myheart = Heart.GetInstance();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      var form = new Form1();
      var txtbx = form.GetTextBox1();
      var cancellationToken = form.GetCancellationToken();
      var newPointsLoc = new List<Position>();
      for (int i = 0; i < Data.NodePositions.Count; ++i) {
        newPointsLoc.Add(new Position { X = Data.NodePositions[i].X, Y = Data.NodePositions[i].Y });
      }
      form.points_loc.AddRange(newPointsLoc);
      var newLinesLoc = new List<(int, int)>();
      for (int i = 0; i < Data.PathNames.Count; ++i) {
        int sni = myheart.GetPathTable().path_table[i].GetParameters().EntryNodeIndex; // start node index
        int eni = myheart.GetPathTable().path_table[i].GetParameters().ExitNodeIndex; // end node index
        newLinesLoc.Add((sni, eni));
      }
      form.lines_loc.AddRange(newLinesLoc);

      // Start a new task to update the colors
      Task.Run(() => {
        for (int j = 0; j < 1000; ++j) {
          if (cancellationToken.IsCancellationRequested)
            break;

          myheart.HeartAutomaton();
          var newPointsColor = new List<int>();

          for (int i = 0; i < Data.NodePositions.Count; ++i) {
            newPointsColor.Add((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1);
          }

          var newLinesColor = new List<int>();
          for (int i = 0; i < Data.PathNames.Count; ++i) {
            newLinesColor.Add((int)myheart.GetPathTable().path_table[i].GetParameters().PathStateIndex - 1);
          }

          string updateString = "";
          for (int i = 0; i < Data.NodeNames.Count; ++i){
            updateString += ((int)myheart.GetNodeTable().node_table[i].GetParameters().NodeStateIndex - 1).ToString() + " ";
          }
          // Safely update the UI on the UI thread
          form.Invoke((MethodInvoker)delegate {
            if (form.IsDisposed || !form.IsHandleCreated)
              return;

            form.points_color.Clear();
            form.points_color.AddRange(newPointsColor);
            form.lines_color.Clear();
            form.lines_color.AddRange(newLinesColor);
            txtbx.Text = updateString;
            form.RedrawPictureBox();
          });

          Thread.Sleep(50); // Adjust sleep time as needed
        }
      }, cancellationToken);

      Application.Run(form);
    }
  }
}