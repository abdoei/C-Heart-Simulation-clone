using System;
using System.Windows.Forms;

namespace HeartSim
{
  public partial class Form1 : Form
  {
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
  }
}
