namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            clientTabs = new TabControl();
            tabPage1 = new TabPage();
            totalSum = new Label();
            suppliers = new ComboBox();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            button3 = new Button();
            numProducts = new NumericUpDown();
            button2 = new Button();
            button1 = new Button();
            selectedProducts = new ListBox();
            products = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            payment = new TabPage();
            button7 = new Button();
            dateTimePicker7 = new DateTimePicker();
            paymantTable = new DataGridView();
            deliveryList = new TabPage();
            invTable = new DataGridView();
            tabPage2 = new TabPage();
            checkBox6 = new CheckBox();
            checkBox5 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox3 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            button6 = new Button();
            button4 = new Button();
            label5 = new Label();
            label4 = new Label();
            dateTimePicker4 = new DateTimePicker();
            dateTimePicker3 = new DateTimePicker();
            reportTable = new DataGridView();
            orders = new TabPage();
            button8 = new Button();
            label1 = new Label();
            label2 = new Label();
            dateTimePicker2 = new DateTimePicker();
            dateTimePicker5 = new DateTimePicker();
            reportTable2 = new DataGridView();
            info = new TabPage();
            pictureBox2 = new PictureBox();
            name_client = new Label();
            clientTabs.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numProducts).BeginInit();
            payment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)paymantTable).BeginInit();
            deliveryList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)invTable).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)reportTable).BeginInit();
            orders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)reportTable2).BeginInit();
            info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // clientTabs
            // 
            clientTabs.Controls.Add(tabPage1);
            clientTabs.Controls.Add(payment);
            clientTabs.Controls.Add(deliveryList);
            clientTabs.Controls.Add(tabPage2);
            clientTabs.Controls.Add(orders);
            clientTabs.Controls.Add(info);
            clientTabs.Dock = DockStyle.Fill;
            clientTabs.Location = new Point(0, 0);
            clientTabs.Name = "clientTabs";
            clientTabs.SelectedIndex = 0;
            clientTabs.Size = new Size(994, 590);
            clientTabs.TabIndex = 2;
            clientTabs.Click += clientTabs_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(totalSum);
            tabPage1.Controls.Add(suppliers);
            tabPage1.Controls.Add(radioButton2);
            tabPage1.Controls.Add(radioButton1);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(numProducts);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(selectedProducts);
            tabPage1.Controls.Add(products);
            tabPage1.Controls.Add(dateTimePicker1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(986, 562);
            tabPage1.TabIndex = 5;
            tabPage1.Text = "Приемка";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // totalSum
            // 
            totalSum.AutoSize = true;
            totalSum.Location = new Point(161, 349);
            totalSum.Name = "totalSum";
            totalSum.Size = new Size(0, 15);
            totalSum.TabIndex = 30;
            // 
            // suppliers
            // 
            suppliers.FormattingEnabled = true;
            suppliers.Items.AddRange(new object[] { "КубаньМясоПром", "Сочинский фермер", "АПК \"Южный край\"", "Фермерское подворье Кавказ", "ООО \"Краснодархолод\"", "ЗАО \"Ейскмясопродукт\"" });
            suppliers.Location = new Point(158, 103);
            suppliers.Name = "suppliers";
            suppliers.Size = new Size(458, 23);
            suppliers.TabIndex = 29;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(258, 396);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(97, 19);
            radioButton2.TabIndex = 28;
            radioButton2.TabStop = true;
            radioButton2.Text = "Не оплачено";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(158, 396);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(81, 19);
            radioButton1.TabIndex = 27;
            radioButton1.TabStop = true;
            radioButton1.Text = "Оплачено";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(155, 435);
            button3.Name = "button3";
            button3.Size = new Size(461, 23);
            button3.TabIndex = 26;
            button3.Text = "Оформить заказ";
            button3.UseVisualStyleBackColor = true;
            button3.Click += buttonCreateInvoices_Click;
            // 
            // numProducts
            // 
            numProducts.Location = new Point(158, 261);
            numProducts.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numProducts.Name = "numProducts";
            numProducts.Size = new Size(458, 23);
            numProducts.TabIndex = 25;
            numProducts.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // button2
            // 
            button2.Location = new Point(541, 299);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 24;
            button2.Text = "Удалить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += removeFromSelectedProduct;
            // 
            // button1
            // 
            button1.Location = new Point(158, 299);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 23;
            button1.Text = "Добавить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += addToSelectedProduct_Click;
            // 
            // selectedProducts
            // 
            selectedProducts.FormattingEnabled = true;
            selectedProducts.ItemHeight = 15;
            selectedProducts.Location = new Point(158, 161);
            selectedProducts.Name = "selectedProducts";
            selectedProducts.Size = new Size(458, 94);
            selectedProducts.TabIndex = 22;
            // 
            // products
            // 
            products.FormattingEnabled = true;
            products.Items.AddRange(new object[] { "Сосиски куринные \"Сашка\"", "Сосиски докторские", "Сосиски детские", "Колбаса сырокопченая", "Ветчина нарезная", "Шашлык охлажденный (курица)", "Шашлык охлажденный (свинина)", "Сало с чесноком", "Паштет домашний", "Гречневая каша готовая" });
            products.Location = new Point(158, 132);
            products.Name = "products";
            products.Size = new Size(458, 23);
            products.TabIndex = 21;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(158, 69);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(458, 23);
            dateTimePicker1.TabIndex = 20;
            // 
            // payment
            // 
            payment.Controls.Add(button7);
            payment.Controls.Add(dateTimePicker7);
            payment.Controls.Add(paymantTable);
            payment.Location = new Point(4, 24);
            payment.Name = "payment";
            payment.Padding = new Padding(3);
            payment.Size = new Size(986, 562);
            payment.TabIndex = 0;
            payment.Text = "Оплата";
            payment.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(762, 52);
            button7.Name = "button7";
            button7.Size = new Size(159, 23);
            button7.TabIndex = 4;
            button7.Text = "Оплатить";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // dateTimePicker7
            // 
            dateTimePicker7.Location = new Point(762, 6);
            dateTimePicker7.Name = "dateTimePicker7";
            dateTimePicker7.Size = new Size(159, 23);
            dateTimePicker7.TabIndex = 3;
            // 
            // paymantTable
            // 
            paymantTable.AllowUserToAddRows = false;
            paymantTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            paymantTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            paymantTable.BackgroundColor = SystemColors.Window;
            paymantTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            paymantTable.Location = new Point(0, 3);
            paymantTable.Name = "paymantTable";
            paymantTable.RowHeadersWidth = 62;
            paymantTable.Size = new Size(756, 470);
            paymantTable.TabIndex = 1;
            // 
            // deliveryList
            // 
            deliveryList.Controls.Add(invTable);
            deliveryList.Location = new Point(4, 24);
            deliveryList.Name = "deliveryList";
            deliveryList.Size = new Size(986, 562);
            deliveryList.TabIndex = 2;
            deliveryList.Text = "Заказы";
            deliveryList.UseVisualStyleBackColor = true;
            // 
            // invTable
            // 
            invTable.AllowUserToAddRows = false;
            invTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            invTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            invTable.BackgroundColor = SystemColors.Window;
            invTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            invTable.Location = new Point(7, 3);
            invTable.Name = "invTable";
            invTable.RowHeadersWidth = 62;
            invTable.Size = new Size(822, 470);
            invTable.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(checkBox6);
            tabPage2.Controls.Add(checkBox5);
            tabPage2.Controls.Add(checkBox4);
            tabPage2.Controls.Add(checkBox3);
            tabPage2.Controls.Add(checkBox2);
            tabPage2.Controls.Add(checkBox1);
            tabPage2.Controls.Add(button6);
            tabPage2.Controls.Add(button4);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(dateTimePicker4);
            tabPage2.Controls.Add(dateTimePicker3);
            tabPage2.Controls.Add(reportTable);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(986, 562);
            tabPage2.TabIndex = 6;
            tabPage2.Text = "Отчет1";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(775, 274);
            checkBox6.Margin = new Padding(2);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(160, 19);
            checkBox6.TabIndex = 28;
            checkBox6.Text = "ЗАО \"Ейскмясопродукт\"";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(775, 251);
            checkBox5.Margin = new Padding(2);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(158, 19);
            checkBox5.TabIndex = 27;
            checkBox5.Text = "ООО \"Краснодархолод\"";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(775, 228);
            checkBox4.Margin = new Padding(2);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(189, 19);
            checkBox4.TabIndex = 26;
            checkBox4.Text = "Фермерское подворье Кавказ";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(775, 205);
            checkBox3.Margin = new Padding(2);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(136, 19);
            checkBox3.TabIndex = 25;
            checkBox3.Text = "АПК \"Южный край\"";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(775, 182);
            checkBox2.Margin = new Padding(2);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(135, 19);
            checkBox2.TabIndex = 24;
            checkBox2.Text = "Сочинский фермер";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(775, 159);
            checkBox1.Margin = new Padding(2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(127, 19);
            checkBox1.TabIndex = 23;
            checkBox1.Text = "КубаньМясоПром";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(777, 407);
            button6.Name = "button6";
            button6.Size = new Size(194, 23);
            button6.TabIndex = 22;
            button6.Text = "Сохранить отчет";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button4
            // 
            button4.Location = new Point(777, 364);
            button4.Name = "button4";
            button4.Size = new Size(194, 23);
            button4.TabIndex = 21;
            button4.Text = "Сформировать";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(777, 99);
            label5.Name = "label5";
            label5.Size = new Size(21, 15);
            label5.TabIndex = 20;
            label5.Text = "по";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(776, 13);
            label4.Name = "label4";
            label4.Size = new Size(127, 45);
            label4.TabIndex = 19;
            label4.Text = "Сформировать отчет \r\n\r\nС\r\n";
            // 
            // dateTimePicker4
            // 
            dateTimePicker4.Location = new Point(776, 117);
            dateTimePicker4.Name = "dateTimePicker4";
            dateTimePicker4.Size = new Size(194, 23);
            dateTimePicker4.TabIndex = 18;
            // 
            // dateTimePicker3
            // 
            dateTimePicker3.Location = new Point(777, 61);
            dateTimePicker3.Name = "dateTimePicker3";
            dateTimePicker3.Size = new Size(194, 23);
            dateTimePicker3.TabIndex = 17;
            // 
            // reportTable
            // 
            reportTable.AllowUserToAddRows = false;
            reportTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reportTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            reportTable.BackgroundColor = SystemColors.Window;
            reportTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            reportTable.Location = new Point(0, 0);
            reportTable.Name = "reportTable";
            reportTable.RowHeadersWidth = 62;
            reportTable.Size = new Size(753, 497);
            reportTable.TabIndex = 16;
            // 
            // orders
            // 
            orders.Controls.Add(button8);
            orders.Controls.Add(label1);
            orders.Controls.Add(label2);
            orders.Controls.Add(dateTimePicker2);
            orders.Controls.Add(dateTimePicker5);
            orders.Controls.Add(reportTable2);
            orders.Location = new Point(4, 24);
            orders.Name = "orders";
            orders.Size = new Size(986, 562);
            orders.TabIndex = 3;
            orders.Text = "Отчет2";
            orders.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Location = new Point(774, 180);
            button8.Name = "button8";
            button8.Size = new Size(194, 23);
            button8.TabIndex = 33;
            button8.Text = "Сформировать";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(774, 89);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 32;
            label1.Text = "по";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(773, 3);
            label2.Name = "label2";
            label2.Size = new Size(127, 45);
            label2.TabIndex = 31;
            label2.Text = "Сформировать отчет \r\n\r\nС\r\n";
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(773, 107);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(194, 23);
            dateTimePicker2.TabIndex = 30;
            // 
            // dateTimePicker5
            // 
            dateTimePicker5.Location = new Point(774, 51);
            dateTimePicker5.Name = "dateTimePicker5";
            dateTimePicker5.Size = new Size(194, 23);
            dateTimePicker5.TabIndex = 29;
            // 
            // reportTable2
            // 
            reportTable2.AllowUserToAddRows = false;
            reportTable2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reportTable2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            reportTable2.BackgroundColor = SystemColors.Window;
            reportTable2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            reportTable2.Location = new Point(3, 3);
            reportTable2.Name = "reportTable2";
            reportTable2.RowHeadersWidth = 62;
            reportTable2.Size = new Size(764, 470);
            reportTable2.TabIndex = 1;
            // 
            // info
            // 
            info.Controls.Add(pictureBox2);
            info.Controls.Add(name_client);
            info.Location = new Point(4, 24);
            info.Name = "info";
            info.Size = new Size(986, 562);
            info.TabIndex = 4;
            info.Text = "Инфо";
            info.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.фмф1;
            pictureBox2.Location = new Point(3, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(179, 162);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // name_client
            // 
            name_client.AutoSize = true;
            name_client.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            name_client.Location = new Point(198, 62);
            name_client.Name = "name_client";
            name_client.Size = new Size(415, 50);
            name_client.TabIndex = 1;
            name_client.Text = "Приложение разработано  Петрено Давидом \r\n38 группа 2025 год";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(994, 590);
            Controls.Add(clientTabs);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            clientTabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numProducts).EndInit();
            payment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)paymantTable).EndInit();
            deliveryList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)invTable).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)reportTable).EndInit();
            orders.ResumeLayout(false);
            orders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)reportTable2).EndInit();
            info.ResumeLayout(false);
            info.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl clientTabs;
        private TabPage info;
        private PictureBox pictureBox2;
        private Label name_client;
        private TabPage payment;
        private Button button7;
        private DateTimePicker dateTimePicker7;
        private DataGridView paymantTable;
        private TabPage deliveryList;
        private DataGridView invTable;
        private TabPage orders;
        private DataGridView reportTable2;
        private TabPage tabPage1;
        private ComboBox suppliers;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Button button3;
        private NumericUpDown numProducts;
        private Button button2;
        private Button button1;
        private ListBox selectedProducts;
        private ComboBox products;
        private DateTimePicker dateTimePicker1;
        private TabPage tabPage2;
        private CheckBox checkBox6;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Button button6;
        private Button button4;
        private Label label5;
        private Label label4;
        private DateTimePicker dateTimePicker4;
        private DateTimePicker dateTimePicker3;
        private DataGridView reportTable;
        private Label totalSum;
        private Button button8;
        private Label label1;
        private Label label2;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker5;
    }
}
