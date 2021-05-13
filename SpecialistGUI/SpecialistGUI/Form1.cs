using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpecialistGUI
{
    public partial class Form1 : Form
    {
        static List<String> ChatData;
        static bool isWork = false;
        static int index = -1;
        static ulong LastID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DBClass.connect("D:/DB/Debug.db");
        }
        private void GetMes()
        {
            List<ChatMess> ChatMessages;
            ChatMessages = DBClass.getMessages(ChatData[index], LastID.ToString());
            if (ChatMessages.Count != 0)
            {
                richTextBox1.Text += "Клиент: \n";
                foreach (ChatMess Mes in ChatMessages)
                    richTextBox1.Text += Mes.Mesage + '\n';
                richTextBox1.Text += '\n';
                LastID = (ulong)Convert.ToInt64(ChatMessages[ChatMessages.Count - 1].Id);
            }              
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "/end")
            {
                string All = "Вы:\n" + richTextBox2.Text + '\n';
                richTextBox1.Text += All;               
                LastID = DBClass.putMessage(richTextBox2.Text, ChatData[index]);
                richTextBox2.Clear();                
            }
            else
            {
                stop();
            }            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null) return;
            index = comboBox2.SelectedIndex;
            messTimer.Start();
            button3.Enabled = true;
            queryTimer.Stop();
            isWork = true;
            richTextBox2.Enabled = true;
            comboBox2.Enabled = false;
            comboBox1.Enabled = false;
            LastID = DBClass.getLastID(ChatData[index]);
            DBClass.acceptQuery(ChatData[index]);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DBClass.checkQuery(ChatData[index]) == -1)
                stop();
            if (isWork)
                GetMes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void queryTimer_Tick(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            ChatData = DBClass.getChatIDs();
            
            int i = 0;
            for (i = 0; (ChatData.Count == comboBox2.Items.Count) && i < ChatData.Count; i++)
            {
                if (ChatData[i] != comboBox2.Items[i].ToString()) break;
            }

            if(ChatData.Count == 0 || i != ChatData.Count)
            {
                comboBox2.Items.Clear();
                for (int j = 0; j < ChatData.Count; j++)
                    if (!comboBox1.Items.Contains(ChatData[j]))
                        comboBox2.Items.Add(ChatData[j]);
                label3.Text = "Доступных записей: " + ChatData.Count;
            }
        }

        private void stop()
        {
            queryTimer.Start();
            messTimer.Stop();
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            button3.Enabled = false;
            richTextBox2.Enabled = false;
            richTextBox1.Clear();
            richTextBox2.Clear();
            if (DBClass.checkQuery(ChatData[index]) != -1)
                DBClass.clearQuery(ChatData[index]);
        }
    }
}
