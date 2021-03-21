using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using System.IO;
using MedBot_Test;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        private List<Comand> comandList;
        private List<CommadnTreeComponent> commandTreeComponentList = new List<CommadnTreeComponent>();
        private const String nullStr = "start";

        public Form1()
        {
            InitializeComponent();
        }

        private void loadTreeView(List<Comand> list)
        {
            CommandsTree.BeginUpdate();
            loadTreeComponentTree(commandTreeComponentList);
            CommandsTree.EndUpdate();
        }

        private void loadTree()
        {
            CommandsTree.BeginUpdate();
            loadTreeComponentTree(commandTreeComponentList);
        }

        private void loadTreeComponentTree(List<CommadnTreeComponent> list)
        {
            foreach(CommadnTreeComponent c in list)
            {
                addTreeComponent(c);
            }
        }

        private void addTreeComponent(CommadnTreeComponent c)
        {
            if (c.Level > 1)
            {
                List<int> path = new List<int>();
                //path.Insert(0, c.Position);
                CommadnTreeComponent tmp = FindById(c.Comand.ParentID, commandTreeComponentList);
                while (tmp != null)
                {
                    path.Insert(0, FindById(tmp.Comand.Id, commandTreeComponentList).Position);
                    tmp = FindById(tmp.Comand.ParentID, commandTreeComponentList);
                }
                path.Insert(0, 0);
                var nodes = CommandsTree.Nodes;
                for (int i = 0; i < path.Count; i++)
                {
                    nodes = nodes[path[i]].Nodes;
                }
                nodes.Add(c.Comand.Text).Name = c.Comand.Id;
            }
            else
            {
                CommandsTree.Nodes[0].Nodes.Add(c.Comand.Text).Name = c.Comand.Id;

            }
        }

        private CommadnTreeComponent FindById(String id, List<CommadnTreeComponent> list)
        {
            CommadnTreeComponent commandNode = null;
            foreach (CommadnTreeComponent c in list)
            {
                if (c.Comand.Id == id)
                {
                    commandNode = c;
                    break;
                }
            }
            return commandNode;
        }

        private void extractHeads(List<Comand> list)
        {
            int i = 0;
            foreach (Comand c in list)
            {
                if (c.ParentID == nullStr)
                {
                    commandTreeComponentList.Add(new CommadnTreeComponent(c, 1, i));
                    i++;
                }
            }
            List<CommadnTreeComponent> newList = new List<CommadnTreeComponent>(commandTreeComponentList);
            foreach (CommadnTreeComponent c in commandTreeComponentList)
            {
                fillTree(c, newList);
            }
            commandTreeComponentList = newList;
        }

        private void fillTree(CommadnTreeComponent c, List<CommadnTreeComponent> list)
        {
            if (c.Comand.ChildrID != null)
            {
                String[] ids = c.Comand.ChildrID.Split(';');
                for (int i = 0; i < ids.Length; i++)
                {
                    CommadnTreeComponent kid = new CommadnTreeComponent(DBClass.getComand(ids[i]), c.Level + 1, i);
                    if (kid.Comand != null)
                    {
                        list.Add(kid);
                        fillTree(kid, list);
                    }
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            DBClass.connect("D:/DB/Debug.db");
            comandList = DBClass.getComands(nullStr);
            CommandsTree.BeginUpdate();
            CommandsTree.Nodes.Add("Команды");
            CommandsTree.EndUpdate();
            extractHeads(comandList);
            loadTreeView(comandList);
            CommandsTree.Nodes[0].Toggle();


        }

        private void setAddComponets(bool flag)
        {
            typeComboBox.SelectedIndex = 0;
            typeComboBox.Enabled = flag;
            addButton.Enabled = flag;
            messageTextBox.Enabled = flag;
            messageTextBox.Text = null;
        }

        private void CommandsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            String id = CommandsTree.SelectedNode.Name;
            label1.Text = id;
            setAddComponets(true);
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String id = CommandsTree.SelectedNode.Name;
            Comand c = new Comand(FindSuitableId(), messageTextBox.Text, "", id, Convert.ToString(typeComboBox.SelectedIndex + 1));
            CommadnTreeComponent ct = new CommadnTreeComponent(c,2,0);
            commandTreeComponentList.Add(ct);
            DBClass.putComand(ct.Comand);
            CommandsTree.BeginUpdate();
            addTreeComponent(ct);
            CommandsTree.EndUpdate();
            setAddComponets(false);
        }

        private String FindSuitableId() 
        {
            String max = commandTreeComponentList[0].Comand.Id;
            foreach (CommadnTreeComponent ct in commandTreeComponentList)
            {
                if (Convert.ToInt32(ct.Comand.Id) > Convert.ToInt32(max))
                {
                    max = ct.Comand.Id;
                }
            }
            max = Convert.ToString(Convert.ToInt32(max) + 1);
            return max;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CommandsTree = new TreeView();
            comandList = DBClass.getComands(nullStr);
            CommandsTree.BeginUpdate();
            CommandsTree.Nodes.Add("Команды");
            CommandsTree.EndUpdate();
            extractHeads(comandList);
            loadTreeView(comandList);
            CommandsTree.Nodes[0].Toggle();
        }
    }
}
