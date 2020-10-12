using Diplom.Properties;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Diplom
{
    public partial class NewVideoStream : Form
    {
        private VideoStream vs;

        public NewVideoStream(VideoStream vs)
        {
            InitializeComponent();

            this.vs = vs;

            if (vs.url == null || vs.url.Length == 0)
            {
                this.Text = "Додавання трансляції";
            }
            else
            {
                this.Text = "Налаштування трансляції";
            }

            this.textBox1.Text = vs.url;
            this.textBox2.Text = vs.name;
            this.textBox3.Text = vs.road_size.ToString();
            this.textBox4.Text = vs.max_speed.ToString();

            this.ShowDialog();

        }

        #region btn
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                vs.name = this.textBox2.Text;
                vs.url = this.textBox1.Text;
                try { vs.road_size = float.Parse(this.textBox3.Text); }
                catch (FormatException) { vs.road_size = float.Parse(this.textBox3.Text, CultureInfo.InvariantCulture.NumberFormat); }
                catch { vs.road_size = null; }
                try { vs.max_speed = int.Parse(this.textBox4.Text); } catch { vs.max_speed = null; }
                vs.AddToList();
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Данні введені не вірно", Resources.ErrorMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        #endregion
    }
}
