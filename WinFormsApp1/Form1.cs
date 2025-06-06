using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.ComponentModel.Com2Interop;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Data.Data data;
        public Form1()
        {
            InitializeComponent();
            data = new Data.Data();
        }



        private void buttonCreateInvoices_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToShortDateString();

            // Валидация данных

            if (string.IsNullOrEmpty(products.Text))
            {
                MessageBox.Show("Товар не выбран!");
                return;
            }
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Выберите оплату ");
                return;
            }

            string[] selected_products = new string[selectedProducts.Items.Count];
            int[] nums = new int[selectedProducts.Items.Count];

            for (int i = 0; i < selected_products.Length; i++)
            {
                string item = selectedProducts.Items[i]!.ToString()!;
                var (product, num) = ParseProductInfo(item);
                selected_products[i] = product;
                nums[i] = num;
            }

            string supplier = this.suppliers.Text;
            bool payment = radioButton1.Checked ? true : false;
            int total_sum = Convert.ToInt32(totalSum.Text);

            int new_innvo = data.CreateInvoice(date, supplier, total_sum, selected_products, nums, payment);
            Data.ExcelHelper.SaveDataTableToExcel(data.createExelInvoces(new_innvo), "Накладная");
        }

        private void addToSelectedProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(products.Text))
            {
                MessageBox.Show("Товар не выбран!");
                return;
            }

            string product = products.Text;
            string num = numProducts.Value.ToString();
            input_Product(product, num);
            totalSum.Text = TotalSum().ToString();
        }

        int TotalSum()
        {
            int sum = 0;

            for (int i = 0; i < selectedProducts.Items.Count; i++)
            {
                string item = selectedProducts.Items[i]!.ToString()!;

                if (int.TryParse(item.Split(new[] { " - ", " штук" }, StringSplitOptions.RemoveEmptyEntries)[1], out int num))
                {
                    int lastSeparatorIndex = item.LastIndexOf(" - ");
                    string product = item.Substring(0, lastSeparatorIndex).Trim();
                    sum += num * data.GetProductPrice(product);
                }
            }
            return sum;
        }

        (string ProductName, int Quantity) ParseProductInfo(string input)
        {
            int lastSeparatorIndex = input.LastIndexOf(" - ");
            string productName = input.Substring(0, lastSeparatorIndex).Trim();
            string quantityPart = input.Substring(lastSeparatorIndex + 3).Trim();
            Match match = Regex.Match(quantityPart, @"\d+");

            if (!match.Success || !int.TryParse(match.Value, out int quantity) || quantity <= 0)
            {
                throw new FormatException("Не удалось распарсить количество товара или оно некорректно");
            }

            return (productName, quantity);
        }

        private void removeFromSelectedProduct(object sender, EventArgs e)
        {
            var del_item = selectedProducts.SelectedItem;

            if (del_item != null)
            {
                selectedProducts.Items.Remove(del_item);
                totalSum.Text = TotalSum().ToString();
            }
            else
            {
                return;
            }
        }

        void input_Product(string product, string num)
        {
            bool productFound = false;

            for (int i = 0; i < selectedProducts.Items.Count; i++)
            {
                string item = selectedProducts.Items[i]!.ToString()!;

                if (item.StartsWith(product + " - "))
                {
                    productFound = true;

                    if (int.TryParse(item.Split(new[] { " - ", " штук" }, StringSplitOptions.RemoveEmptyEntries)[1], out int oldQuantity))
                    {
                        // Обновляем количество
                        int newQuantity = oldQuantity + int.Parse(num);
                        selectedProducts.Items[i] = $"{product} - {newQuantity} штук";
                    }
                    break;
                }
            }

            // Если товар не найден, добавляем новый
            if (!productFound)
            {
                selectedProducts.Items.Add($"{product} - {num} штук");
            }
        }




        private void button7_Click(object sender, EventArgs e)
        {

            if (!paymantTable.Visible)
            {
                return;
            }
            foreach (DataGridViewRow row in paymantTable.Rows)
            {


                
                if (row.Cells["Оплачено"].Value.ToString() == "True")
                {
                    string date = dateTimePicker2.Value.ToShortDateString();
                    int order_id = Convert.ToInt32((row.Cells["Номер накладной"].Value.ToString()));
                    int order_item_id = Convert.ToInt32((row.Cells["Номер позиции"].Value.ToString()));
                    decimal paidAmount = GetCellValueAsDecimal(row.Cells["Сумма"]);
                    string product = row.Cells["Товар"].Value!.ToString()!;

                    string message = $"Сумма остатка: {paidAmount}:RUB\nВнести?";
                    string caption = "Подтверждение заказа";
                    MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                    MessageBoxIcon icon = MessageBoxIcon.Question;

                    DialogResult result = MessageBox.Show(message, caption, buttons, icon);

                    if (result == DialogResult.OK)
                    {
                        data.UpdatePaidStatusProduct(date,order_id, order_item_id,product, paidAmount);
                        MessageBox.Show("Заказ подтвержден!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else
                    {

                        MessageBox.Show("Заказ отменен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    updatPayTable();
                }

            }
        }
        private decimal GetCellValueAsDecimal(DataGridViewCell cell)
        {
            if (cell?.Value is not null)
            {
                return decimal.TryParse(cell.Value.ToString(), out decimal value) ? value : 0;
            }
            return 0;
        }

        void updatPayTable()
        {
            
            DataTable dt = new DataTable();
            dt = data.getAllPay();
            if (dt.Rows.Count > 0)
            {
                paymantTable.Visible = true;
                paymantTable.DataSource = dt;
            }
            else
            {
                paymantTable.Visible = false;
                MessageBox.Show("У вас пока нет неоплаченных позиций!");
            }
        }



        private void clientTabs_Click(object sender, EventArgs e)
        {

            int select_index_tab = clientTabs.SelectedIndex;

            switch (select_index_tab)
            {
                case 0:
                   
                    break;
                case 1:
                    updatPayTable();
                    break;
                case 2:
                    displayInv();
                    break;
                case 3:
                   
                    break;

            }
        }

        //void displayClientInfo(string clientName)
        //{

        //    DataTable dataTable = data.getClientInfo(clientName);
        //    DataRow row = dataTable.Rows[0];
        //    id_client.Text = $"Клиент№{row["id"]?.ToString()}";
        //    name_client.Text = $"Имя: {row["name"]?.ToString()}";
        //    info_client.Text = $"Контакты: {row["contact_info"]?.ToString()}";

        //}

        void displayInv()
        {
            DataTable dt = new DataTable();
            dt = data.GetAllInvoices();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("У вас пока нет заказов!");
                invTable.Visible = false;
                return;
            }
            invTable.Visible = true;
            invTable.DataSource = dt;

        }





        private void button4_Click_1(object sender, EventArgs e)
        {
            string dateStartStr = dateTimePicker3.Value.ToShortDateString();
            string dateEndStr = dateTimePicker4.Value.ToShortDateString();

            // Парсим в DateTime
            DateTime startDate = DateTime.Parse(dateStartStr);
            DateTime endDate = DateTime.Parse(dateEndStr);
            DataTable dataTable = new DataTable();
            List<int> clientsId = new List<int>();

            CheckBox[] checkBoxes = new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6 };

            foreach (var checkBox in checkBoxes)
            {
                if (checkBox.Checked)
                {
                    int id = data.GetSupplierId(checkBox.Text);
                    clientsId.Add(id);
                }
            }





            dataTable = data.getReport1(clientsId,startDate, endDate);
            reportTable.DataSource = dataTable;
        }

       

        private void button6_Click(object sender, EventArgs e)
        {
            if (reportTable.DataSource is DataTable table)
            {
                Data.ExcelHelper.SaveDataTableToExcel(table);
            }
            else
            {
                MessageBox.Show("Отчет не сформирован!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string dateStartStr = dateTimePicker5.Value.ToShortDateString();
            string dateEndStr = dateTimePicker2.Value.ToShortDateString();

            // Парсим в DateTime
            DateTime startDate = DateTime.Parse(dateStartStr);
            DateTime endDate = DateTime.Parse(dateEndStr);
            reportTable2.DataSource = data.getReport2(startDate, endDate);


        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
