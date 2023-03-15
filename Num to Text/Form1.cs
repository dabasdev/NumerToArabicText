using System;
using System.Windows.Forms;

namespace Num_to_Text
{
    public partial class Form1 : Form
    {
        private readonly NumberToText _n = new NumberToText();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = _n.DblToText(textBox1.Text, textBox5.Text.Trim(), textBox4.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("آخر رقم مدرج هو " + _n.show_max());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text.Trim() == "")
                {
                    MessageBox.Show("الرجاء كتابة الرقم بالحروف اولا");
                }
                else
                {
                    _n.Add_new(textBox3.Text);
                    textBox3.Clear();
                    button1.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
                Clipboard.SetText(textBox2.Text);
        }
    }
}