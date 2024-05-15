using HeartSim.classes.DataAndTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace HeartSim
{


  public partial class Form1 : Form
  {
    private CancellationTokenSource _cancellationTokenSource;

    public Form1()
    {
      InitializeComponent();
      _cancellationTokenSource = new CancellationTokenSource();

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }
    public void SetTextBox1(string text)
    {
      textBox1.Text = text;
    }
    public TextBox GetTextBox1()
    {
      return textBox1;
    }


    // Store the objects and their colors in variables
    public List<Position> points_loc { get; set; } = new List<Position>(); // List of points coordinates
    public List<(int, int)> lines_loc { get; set; } = new List<(int, int)>(); // List of lines start and end points indices
    public List<int> points_color { get; set; } = new List<int>(Data.NodeNames.Count); // List of points colors
    public List<int> lines_color { get; set; } = new List<int>(Data.PathNames.Count); // List of lines colors

    // List of Colors 
    List<Color> point_colors_bruches { get; set; } = new List<Color> { Color.Lime, Color.Red, Color.Yellow };
    List<Color> line_colors_bruches { get; set; } = new List<Color> { Color.Blue, Color.Lime, Color.Yellow, Color.Black, Color.Red };


    // Create a method to redraw the PictureBox
    public void RedrawPictureBox()
    {
      // Create a copy of the original image
      Image image = new Bitmap(pictureBox1.Image);
      // Create a Graphics object from the image
      using (Graphics graphics = Graphics.FromImage(image))
      {

        for (int i = 0; i < lines_loc.Count; i++)
        {
          // Get the color for this line
          Color c = line_colors_bruches[lines_color[i]];
          //Color c = line_colors_bruches[0];

          // Create a pen with the color
          using (Pen linePen = new Pen(c, 4))
          {
            // Draw the line
            (int start, int end) = lines_loc[i];
            Position startPoint = points_loc[start];
            Position endPoint = points_loc[end];
            graphics.DrawLine(linePen, (float)startPoint.X + 2, (float)(image.Height - startPoint.Y + 2), (float)endPoint.X + 2, (float)(image.Height - endPoint.Y + 2));
          }
        }
        // Draw the points
        for (int i = 0; i < points_loc.Count; i++)
        {
          // Get the color for this point
          Color c = point_colors_bruches[points_color[i]];

          // Create a brush with the color
          using (SolidBrush pointBrush = new SolidBrush(c))
          {
            // Draw the point
            Position point = points_loc[i];
            graphics.FillRectangle(pointBrush, (float)point.X, (float)(image.Height - point.Y), 7, 7);
          }
        }
      }

      // Dispose of the old image and set the new one
      pictureBox1.Image.Dispose();
      pictureBox1.Image = image;
    }


    private void button1_Click_1(object sender, EventArgs e)
    {
      RedrawPictureBox();
    }
    public CancellationToken GetCancellationToken()
    {
      return _cancellationTokenSource.Token;
    }
    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      _cancellationTokenSource.Cancel();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
  }
}
