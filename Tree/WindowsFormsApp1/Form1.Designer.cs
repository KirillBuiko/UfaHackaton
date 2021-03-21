namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.CommandsTree = new System.Windows.Forms.TreeView();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loadButtton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CommandsTree
            // 
            this.CommandsTree.Location = new System.Drawing.Point(12, 12);
            this.CommandsTree.Name = "CommandsTree";
            this.CommandsTree.Size = new System.Drawing.Size(767, 277);
            this.CommandsTree.TabIndex = 0;
            this.CommandsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CommandsTree_AfterSelect);
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.Enabled = false;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "Категория",
            "Сообщение",
            "Выход на специалиста"});
            this.typeComboBox.Location = new System.Drawing.Point(26, 334);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(121, 24);
            this.typeComboBox.TabIndex = 1;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Enabled = false;
            this.messageTextBox.Location = new System.Drawing.Point(305, 330);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(474, 84);
            this.messageTextBox.TabIndex = 2;
            this.messageTextBox.Text = "";
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(27, 373);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(121, 29);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(882, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Тип узла";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(302, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Текст";
            // 
            // loadButtton
            // 
            this.loadButtton.Location = new System.Drawing.Point(174, 373);
            this.loadButtton.Name = "loadButtton";
            this.loadButtton.Size = new System.Drawing.Size(107, 29);
            this.loadButtton.TabIndex = 7;
            this.loadButtton.Text = "Загрузить";
            this.loadButtton.UseVisualStyleBackColor = true;
            this.loadButtton.Click += new System.EventHandler(this.loadButtton_Click);
            // 
            // Form1
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(792, 426);
            this.Controls.Add(this.loadButtton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.CommandsTree);
            this.Name = "Form1";
            this.Text = "Редактор команд";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TreeView CommandsTree;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadButtton;

        #endregion

        //private System.Windows.Forms.MenuStrip menuStrip3;
    }
}

