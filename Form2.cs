using System.Collections.Generic;
using System.Windows.Forms;

namespace guigubahuang
{
    public partial class Form2 : Form
    {
        private List<Hero> _list;

        public Form2()
        {
            InitializeComponent();
        }

        public string Dialog(Form form, List<Hero> list)
        {
            SetListBox(list);
            this.ShowDialog(form);
            string name = string.Empty;
            if (listBox1.SelectedItem != null)
                name = listBox1.SelectedItem.ToString();
            return name;
        }

        private void SetListBox(List<Hero> list)
        {
            _list = list;
            listBox1.Items.Clear();
            foreach (Hero item in _list)
            {
                listBox1.Items.Add(item.GetName());
            }
        }
        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (Hero item in _list)
            {
                if (item.GetName().Contains(textBox1.Text))
                    listBox1.Items.Add(item.GetName());
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem != null)
                this.Close();
        }
    }
}
