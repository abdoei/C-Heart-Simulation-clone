using HeartSim.classes.DataAndTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace HeartSim
{


  public partial class Form1 : Form
  {
    public static object form_lock { get; private set; } = new object();

    public Form1()
    {
      InitializeComponent();
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
    public List<Color> lines_color { get; set; } = new List<Color>(); // List of lines colors

    Point point = new Point(50, 50);
    Point lineStart = new Point(100, 100);
    Point lineEnd = new Point(200, 200);
    Color pointColor = Color.Yellow;
    Color lineColor = Color.Yellow;

    // List of Colors 
    //QString color_opt_node[] = { "lime", "red", "yellow" };
    //QString color_opt_path[] = { "blue", "lime", "yellow", "black", "red" };
    //List<Color> point_colors_bruches =  {Color.Lime, Color.Red, Color.Yellow};
    List<Color> point_colors_bruches { get; set; } = new List<Color> { Color.Lime, Color.Red, Color.Yellow };
    List<Color> line_colors_bruches { get; set; } = new List<Color> { Color.Blue, Color.Lime, Color.Yellow, Color.Black, Color.Red };


    // Create a method to redraw the PictureBox
    public void RedrawPictureBox()
    {
      lock (form_lock)
      {
        // Create a copy of the original image
        Image image = new Bitmap(pictureBox1.Image);
      }
      // Create a Graphics object from the image
      using (Graphics graphics = Graphics.FromImage(image))
      {
        //// Create brushes with the current colors
        //using (SolidBrush pointBrush = new SolidBrush(pointColor))
        //using (Pen linePen = new Pen(lineColor))
        //{
        //  // Draw the point and the line
        //  graphics.FillRectangle(pointBrush, point.X, point.Y, 5, 5);
        //  //graphics.DrawLine(linePen, lineStart, lineEnd);
        //  foreach (var p in points_loc)
        //  {
        //    graphics.FillRectangle(pointBrush, (float)p.X, (float)(image.Height - p.Y), 5, 5);

        //  }

        //}

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

    private void button1_Click(object sender, EventArgs e)
    {
      // Change the colors and redraw the PictureBox when the button is clicked
      pointColor = Color.Yellow;
      lineColor = Color.Yellow;
      RedrawPictureBox();
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      // Change the colors and redraw the PictureBox when the button is clicked
      pointColor = Color.Yellow;
      lineColor = Color.Yellow;
      RedrawPictureBox();
    }
  }
}
