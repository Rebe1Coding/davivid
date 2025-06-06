using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Permissions;
using Npgsql;

namespace WinFormsApp1.Data
{
    class Data
    {
        // Конструктор класса Data, инициализирующий соединение с базой данных.
        public Data()
        {
            con = new NpgsqlConnection(connString);
        }

        private readonly string connString = "Host=127.0.0.1;Username=postgres;Password=1234;Database=shopTop";
        NpgsqlConnection con;



        void OpenConnection()
        {
            try
            {
                if (con != null && con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии соединения: {ex.Message}");
            }
        }

        // Закрывает соединение с базой данных
        void CloseConnection()
        {
            try
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии соединения: {ex.Message}");
            }
        }


        public int GetSupplierId(string supplierName)
        {
            OpenConnection();
            string query = "SELECT id FROM suppliers WHERE name = @name";
            using (var cmd = new NpgsqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@name", supplierName);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception($"Поставщик '{supplierName}' не найден");

                CloseConnection();
                return Convert.ToInt32(result);
            }
        }
        public DataTable GetAllInvoices()
        {
            DataTable dt = new DataTable();

            string query = @"
        SELECT 
            id AS ""Номер накладной"",
            supplier_id AS ""ID поставщика"",
            invoice_date AS ""Дата накладной"",
            due_date AS ""Срок оплаты"",
            total_amount AS ""Общая сумма"",
            is_paid AS ""Оплачено""
        FROM invoices
        ORDER BY id";

            OpenConnection();
               
                    
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                
           
                CloseConnection();
            

            return dt;
        }

        public int GetProductPrice(string productName)
        {
            string query = "SELECT price FROM products WHERE name = @name";
            OpenConnection();
            using (var cmd = new NpgsqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@name", productName);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception($"Продукт '{productName}' не найден");
                CloseConnection();
                return Convert.ToInt32(result);
            }


        }

        private decimal GetProductPrice(string productName, NpgsqlTransaction transaction)
        {
            string query = "SELECT price FROM products WHERE name = @name";
            using (var cmd = new NpgsqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@name", productName);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception($"Продукт '{productName}' не найден");
                return Convert.ToDecimal(result);
            }
        }


        private void AddInvoiceItems(int invoiceId, string[] productNames, int[] quantities, bool payment, NpgsqlTransaction transaction)
        {
            string query = @"
            INSERT INTO invoice_items (invoice_id, product_id, quantity, unit_price, subtotal,paid)
            VALUES (@invoice_id, @product_id, @quantity, @unit_price, @subtotal,@payment);";

            for (int i = 0; i < productNames.Length; i++)
            {
                string productName = productNames[i];
                int quantity = quantities[i];

                int productId = GetProductId(productName, transaction);
                decimal unitPrice = GetProductPrice(productName, transaction);
                decimal subtotal = unitPrice * quantity;

                using (var cmd = new NpgsqlCommand(query, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@invoice_id", invoiceId);
                    cmd.Parameters.AddWithValue("@product_id", productId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@unit_price", unitPrice);
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePaidStatusProduct (string payday,int invoiceId, int invoiceItemId, string productName, decimal amount)
        {
            string paymentQuery = @"
        INSERT INTO payments (invoice_id, invoice_item_id, amount, payment_date)
        VALUES (@invoice_id, @invoice_item_id, @amount, TO_DATE(@payment_date, 'DD.MM.YYYY'));";

            string markAsPaidQuery = @"
        UPDATE invoice_items
        SET paid = TRUE
        WHERE id = @invoice_item_id;";

            OpenConnection();
            using (var transaction = con.BeginTransaction())
            {
                try
                {
                    // Вносим оплату
                    using (var cmd = new NpgsqlCommand(paymentQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@invoice_id", invoiceId);
                        cmd.Parameters.AddWithValue("@invoice_item_id", invoiceItemId);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@payment_date", payday);
                        cmd.ExecuteNonQuery();
                    }

                    // Помечаем позицию как оплаченную
                    using (var cmd = new NpgsqlCommand(markAsPaidQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@invoice_item_id", invoiceItemId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Что-то пошло не так! {ex.Message}");
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        public void MakePayment(int invoiceId, decimal amount)
        {
            OpenConnection();
            using (var transaction = con.BeginTransaction())
            {
                try
                {
                    string query = @"
                        INSERT INTO payments (invoice_id, payment_date, amount, is_prepayment)
                        VALUES (@invoice_id, CURRENT_DATE, @amount, FALSE);";

                    using (var command = new NpgsqlCommand(query, con, transaction))
                    {
                        command.Parameters.AddWithValue("@invoice_id", invoiceId);
                        command.Parameters.AddWithValue("@amount", amount);

                        command.ExecuteNonQuery();
                    }

                    // Подтверждаем транзакцию
                    transaction.Commit();
                    MessageBox.Show("Оплата успешно внесена!");
                }
                catch (Exception ex)
                {
                    // Откатываем транзакцию в случае ошибки
                    transaction.Rollback();
                    MessageBox.Show($"Ошибка при внесении оплаты: {ex.Message}");
                }
            }
            CloseConnection();
        }

        public DataTable getAllPay()
        {
            DataTable dt = new DataTable();
            string query = @$"
SELECT
    i.id AS ""Номер накладной"",
    i.invoice_date AS ""Дата накладной"",
    i.due_date AS ""Срок оплаты"",
    s.name AS ""Поставщик"",
    ii.id AS ""Номер позиции"",
    p.name AS ""Товар"",
    ii.quantity AS ""Количество"",
    ii.unit_price AS ""Цена за единицу"",
    (ii.quantity * ii.unit_price) AS ""Сумма"",
    ii.paid AS ""Оплачено""
FROM invoices i
JOIN suppliers s ON i.supplier_id = s.id
JOIN invoice_items ii ON i.id = ii.invoice_id
JOIN products p ON ii.product_id = p.id
WHERE ii.paid = FALSE 
ORDER BY i.id, ii.id;

";
            OpenConnection();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                using (NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd))
                {
                    adap.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Что пошло не так!{ex.Message}");

            }
            CloseConnection();
            return dt;

        }

        public DataTable createExelInvoces(int id)
        {
            DataTable dt = new DataTable();
            string query = @$"
SELECT
    i.id AS ""Номер накладной"",
    i.invoice_date AS ""Дата накладной"",
    i.due_date AS ""Срок оплаты"",
    s.name AS ""Поставщик"",
    ii.id AS ""Номер позиции"",
    p.name AS ""Товар"",
    ii.quantity AS ""Количество"",
    ii.unit_price AS ""Цена за единицу"",
    ii.subtotal AS ""Сумма"",  -- Используем уже вычисленный subtotal
    ii.paid AS ""Оплачено""
FROM invoices i
JOIN suppliers s ON i.supplier_id = s.id
JOIN invoice_items ii ON i.id = ii.invoice_id
JOIN products p ON ii.product_id = p.id
WHERE i.id = {id} 
ORDER BY i.id, ii.id;


";
            OpenConnection();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                using (NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd))
                {
                    adap.Fill(dt);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Что пошло не так!{ex.Message}");

            }
            CloseConnection();
            return dt;

        }


        private int GetProductId(string productName, NpgsqlTransaction transaction)
        {
            string query = "SELECT id FROM products WHERE name = @name";
            using (var cmd = new NpgsqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@name", productName);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new Exception($"Продукт '{productName}' не найден");
                return Convert.ToInt32(result);
            }
        }

        public void PayForSingleInvoiceItem(int invoiceId, int invoiceItemId, decimal amount, string paymentDateStr)
        {
            string query = @"
        INSERT INTO payments (invoice_id, invoice_item_id, amount, payment_date)
        VALUES (@invoice_id, @invoice_item_id, @amount, TO_DATE(@payment_date, 'DD.MM.YYYY'));";


            OpenConnection();

            using (var transaction = con.BeginTransaction())
            {
                try
                {
                    using (var command = new NpgsqlCommand(query, con, transaction))
                    {
                        command.Parameters.AddWithValue("@invoice_id", invoiceId);
                        command.Parameters.AddWithValue("@invoice_item_id", invoiceItemId);
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@payment_date", paymentDateStr);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Оплата прошла успешно.");
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    MessageBox.Show($"Оплата не прошла.{ex.Message}");
                }
            }
            CloseConnection();
        }
    



        public int CreateInvoice(string date, string supplierName, int totalAmount, string[] productNames, int[] quantities, bool payment)
        {
            int supplierId = GetSupplierId(supplierName);

            string insertInvoiceQuery = @"
            INSERT INTO invoices (supplier_id, invoice_date, due_date, total_amount, is_paid)
            VALUES (@supplier_id, TO_DATE(@invoice_date, 'DD.MM.YYYY'), 
                    TO_DATE(@invoice_date, 'DD.MM.YYYY') + INTERVAL '6 months', 
                    @total_amount, @payment)
            RETURNING id;";

            OpenConnection();
           
            int new_invoce =-1;
            using (var transaction = con.BeginTransaction())
            {
                try
                {
                    int invoiceId;

                    using (var cmd = new NpgsqlCommand(insertInvoiceQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                        cmd.Parameters.AddWithValue("@invoice_date", date);
                        cmd.Parameters.AddWithValue("@total_amount", totalAmount);
                        cmd.Parameters.AddWithValue("@payment", payment);
                        invoiceId = Convert.ToInt32(cmd.ExecuteScalar());
                        new_invoce = invoiceId;
                    }

                    // Добавляем товары в invoice_items
                   AddInvoiceItems(invoiceId, productNames, quantities, payment, transaction);

                    transaction.Commit();
                    MessageBox.Show("Накладная успешно создана!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Ошибка: {ex.Message}\nВсе изменения отменены.");
                }
                finally
                {
                    CloseConnection();
                }
            }
            MessageBox.Show($"{new_invoce}");
            return new_invoce;
        }



        public DataTable getReport1(List<int> clientIds, DateTime startDate, DateTime endDate)
        {
            var dt = new DataTable();

            string query = @"
  WITH supplier_invoices AS (
    SELECT 
        s.id AS supplier_id,
        s.name AS supplier_name,
        i.id AS invoice_id,
        i.total_amount,
        i.due_date,
        (SELECT COALESCE(SUM(amount), 0) 
         FROM payments p
         JOIN invoice_items ii ON p.invoice_item_id = ii.id
         WHERE ii.invoice_id = i.id) AS paid_total
    FROM suppliers s
    JOIN invoices i ON s.id = i.supplier_id
    WHERE s.id  = ANY (@clientIds) -- ID поставщиков
      AND i.invoice_date BETWEEN @startDate AND @endDate 
)
SELECT 
    supplier_id AS ""ID поставщика"",
    supplier_name AS ""Поставщик"",
    COALESCE(SUM(total_amount), 0) AS ""Общий оборот"",
    COALESCE(SUM(total_amount - paid_total), 0) AS ""Не оплачено"",
    COALESCE(SUM(
        CASE 
            WHEN due_date < CURRENT_DATE 
                 AND (total_amount - paid_total) > 0 
            THEN total_amount - paid_total 
            ELSE 0 
        END
    ), 0) AS ""Просрочено""
FROM supplier_invoices
GROUP BY supplier_id, supplier_name
ORDER BY supplier_id;";


            using (var cmd = new NpgsqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@clientIds", clientIds.ToArray());

                OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
                CloseConnection();
            }

            return dt;
        }




        public DataTable getReport2(DateTime startDate, DateTime endDate)
        {
            var dt = new DataTable();

            string query = @"
WITH product_balances AS (
    SELECT
        i.due_date,
        p.id AS product_id,
        p.name AS product_name,
        ii.id AS item_id,
        ii.subtotal,
        (SELECT COALESCE(SUM(amount), 0) 
         FROM payments 
         WHERE invoice_item_id = ii.id) AS paid_amount
    FROM products p
    JOIN invoice_items ii ON p.id = ii.product_id
    JOIN invoices i ON ii.invoice_id = i.id
    WHERE p.id IN (1,2,3,4,5,6,7,8,9,10) -- ID товаров
      AND i.due_date BETWEEN @startDate AND @endDate 
)
SELECT
    due_date AS ""Дата оплаты"",
    product_id AS ""ID товара"",
    product_name AS ""Товар"",
    COALESCE(SUM(subtotal - paid_amount), 0) AS ""Сумма к оплате""
FROM product_balances
WHERE subtotal > paid_amount -- Только неоплаченные остатки
GROUP BY due_date, product_id, product_name
HAVING COALESCE(SUM(subtotal - paid_amount), 0) > 0
ORDER BY due_date, product_id;
  ";


            using (var cmd = new NpgsqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                

                OpenConnection();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
                CloseConnection();
            }

            return dt;
        }
    }
}







//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Npgsql;

//namespace WinFormsApp1.Data
//{
//    class Data
//    {
//        public Data()
//        {
//            con = new NpgsqlConnection(connString);
//        }

//        private readonly string connString = "Host=127.0.0.1;Username=postgres;Password=1234;Database=shopGarry";
//        NpgsqlConnection con;

//        private readonly string clientsQuery = @"
//                            SELECT 
//                                id AS ""Идентификатор клиента"",
//                                name AS ""Имя клиента"",
//                                contact_info AS ""Контактная информация""
//                            FROM clients";

//        private readonly string productsQuery = @"
//                                        SELECT 
//                                            id AS ""Идентификатор товара"",
//                                            name AS ""Название товара"",
//                                            price AS ""Цена""
//                                        FROM products";

//        string ordersQuery = @"
//                                    SELECT 
//                                        id AS ""Номер заказа"",
//                                        client_id AS ""Идентификатор клиента"",
//                                        order_date AS ""Дата заказа"",
//                                    CASE 
//                                    WHEN is_active THEN 'Активный' 
//                                                    ELSE 'Завершен' 
//                                                END AS ""Статус заказа""
//                                                    FROM orders";
//        public DataTable getInfoAbout(string name)
//        {
//            string query = name switch
//            {
//                "products" => productsQuery,
//                "orders" => ordersQuery,
//                "clients" => clientsQuery,
//                _ => throw new ArgumentException($"Неизвестное имя: {name}")
//            };

//            DataTable dt = new DataTable();
//            OpenConnection();
//            try
//            {
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(query, con);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {

//                MessageBox.Show(ex.Message);
//            }
//            CloseConnection();
//            return dt;

//        }

//        public DataTable getClientInfo(string name)
//        {
//            DataTable dt = new DataTable();
//            string query = $"SELECT * FROM clients WHERE name = @name";
//            OpenConnection();
//            try
//            {
//                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
//                cmd.Parameters.AddWithValue("@name", name);
//                using (NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd))
//                {
//                    adap.Fill(dt);
//                }

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что пошло не так!{ex.Message}");

//            }
//            CloseConnection();
//            return dt;

//        }
//        public DataTable getAllProducts()
//        {
//            string query = "SELECT * FROM products";
//            DataTable dt = new DataTable();
//            OpenConnection();
//            try
//            {
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(query, con);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что пошло не так!{ex.Message}");
//            }
//            CloseConnection();
//            return dt;
//        }

//        public int priceOfProducts(string name)
//        {
//            string query = "SELECT price FROM products WHERE name = @name";
//            DataTable dt = new DataTable();

//            OpenConnection();
//            try
//            {
//                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
//                cmd.Parameters.AddWithValue("@name", name);
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка: {ex.Message}");
//            }

//            CloseConnection();
//            return Convert.ToInt32(dt.Rows[0]["price"]);
//        }
//        public DataTable getAllClients()
//        {
//            string query = "SELECT * FROM clients";
//            DataTable dt = new DataTable();
//            OpenConnection();
//            try
//            {
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(query, con);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что пошло не так!{ex.Message}");
//            }
//            CloseConnection();
//            return dt;
//        }
//        void OpenConnection()
//        {
//            try
//            {
//                if (con != null && con.State == System.Data.ConnectionState.Closed)
//                {
//                    con.Open();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка при открытии соединения: {ex.Message}");
//            }
//        }


//        void CloseConnection()
//        {
//            try
//            {
//                if (con != null && con.State == System.Data.ConnectionState.Open)
//                {
//                    con.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка при закрытии соединения: {ex.Message}");
//            }
//        }

//        public void createOrder(string date, string client, int sum, string[] products, int[] nums)
//        {
//            int clientId = getClientId(client);
//            string orderQuery = @"
//                INSERT INTO orders(client_id, order_date, total_amount, total_paid) 
//                VALUES (@client_id, TO_DATE(@order_date,'DD.MM.YYYY'), @total_amount,@total_paid)
//                RETURNING id;";


//            OpenConnection();
//            using (var transaction = con.BeginTransaction()) // Используем using для транзакции
//            {
//                try
//                {
//                    int orderId;

//                    using (var orderCommand = new NpgsqlCommand(orderQuery, con, transaction))
//                    {

//                        orderCommand.Parameters.AddWithValue("@client_id", clientId);
//                        orderCommand.Parameters.AddWithValue("@order_date", date);
//                        orderCommand.Parameters.AddWithValue("@total_amount", sum);
//                        orderCommand.Parameters.AddWithValue("@total_paid", Math.Round(sum * 0.3, 0, MidpointRounding.AwayFromZero));//30% от полной суммы
//                        orderId = Convert.ToInt32(orderCommand.ExecuteScalar());
//                    }

//                    // Добавление позиций заказа
//                    addOrderItems(orderId, products, nums, transaction);

//                    transaction.Commit();
//                    MessageBox.Show("Заказ успешно создан!");
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    MessageBox.Show($"Ошибка: {ex.Message}\nВсе изменения отменены!");
//                }
//                finally
//                {
//                    CloseConnection();
//                }
//            }
//        }

//        private void addOrderItems(int orderId, string[] products, int[] nums, NpgsqlTransaction transaction)
//        {
//            string itemsQuery = @$"INSERT INTO order_items(order_id, product_id, quantity,paid_amount) 
//                                    VALUES (@order_id, @product_id, @quantity,@paid_amount)";

//            for (int i = 0; i < products.Length; i++)
//            {
//                int productId = getProductId(products[i], transaction);
//                int new_productPrice = priceOfProductsTransaction(products[i], transaction);
//                using (var itemCommand = new NpgsqlCommand(itemsQuery, con, transaction))
//                {
//                    itemCommand.Parameters.AddWithValue("@order_id", orderId);
//                    itemCommand.Parameters.AddWithValue("@product_id", productId);
//                    itemCommand.Parameters.AddWithValue("@quantity", nums[i]);
//                    itemCommand.Parameters.AddWithValue("@paid_amount", Math.Round(new_productPrice * 0.3, 0, MidpointRounding.AwayFromZero));
//                    itemCommand.ExecuteNonQuery();
//                }
//            }
//        }


//        public int priceOfProductsTransaction(string productName, NpgsqlTransaction transaction)
//        {
//            string query = "SELECT price FROM products WHERE name = @name";
//            using (var cmd = new NpgsqlCommand(query, con, transaction))
//            {
//                cmd.Parameters.AddWithValue("@name", productName);
//                var result = cmd.ExecuteScalar();
//                if (result == null)
//                    throw new Exception($"Продукт '{productName}' не найден");
//                return Convert.ToInt32(result);
//            }
//        }
//        public int getClientId(string clientName)
//        {
//            string query = "SELECT id FROM clients WHERE name = @name";
//            OpenConnection();

//            using (var cmd = new NpgsqlCommand(query, con))
//            {
//                cmd.Parameters.AddWithValue("@name", clientName);
//                var result = cmd.ExecuteScalar();
//                if (result == null)
//                    throw new Exception($"Клиент '{clientName}' не найден");
//                CloseConnection();
//                return Convert.ToInt32(result);
//            }

//        }

//        int getProductId(string productName, NpgsqlTransaction transaction)
//        {
//            string query = "SELECT id FROM products WHERE name = @name";
//            using (var cmd = new NpgsqlCommand(query, con, transaction))
//            {
//                cmd.Parameters.AddWithValue("@name", productName);
//                var result = cmd.ExecuteScalar();
//                if (result == null)
//                    throw new Exception($"Продукт '{productName}' не найден");
//                return Convert.ToInt32(result);
//            }
//        }



//        public DataTable GetUnpaidDeliveredProductsByDay(DateTime startDate, DateTime endDate)
//        {
//            var dt = new DataTable();

//            string query = @"
//        SELECT
//            o.order_date::DATE AS ""Дата"",
//            SUM(oi.quantity * p.price) AS ""Сумма отгруженного"",
//            COALESCE(SUM(paid.amount), 0) AS ""Оплачено"",
//            SUM(oi.quantity * p.price) - COALESCE(SUM(paid.amount), 0) AS ""Не оплачено""
//        FROM order_items oi
//        JOIN orders o ON oi.order_id = o.id
//        JOIN products p ON oi.product_id = p.id
//        LEFT JOIN (
//            SELECT order_id, SUM(amount) AS amount
//            FROM payments
//            GROUP BY order_id
//        ) paid ON paid.order_id = o.id
//        WHERE
//            oi.is_delivered = TRUE
//            AND o.order_date::DATE BETWEEN @startDate AND @endDate
//        GROUP BY o.order_date::DATE
//        ORDER BY o.order_date::DATE;";


//            using (var cmd = new NpgsqlCommand(query, con))
//            {
//                cmd.Parameters.AddWithValue("@startDate", startDate);
//                cmd.Parameters.AddWithValue("@endDate", endDate.AddDays(1).AddSeconds(-1));

//                OpenConnection();
//                using (var reader = cmd.ExecuteReader())
//                {
//                    dt.Load(reader);
//                }
//                CloseConnection();
//            }

//            return dt;
//        }
//        public DataTable GetUnshippedProductsByClients(List<int> clientIds, DateTime startDate, DateTime endDate)
//        {
//            var dt = new DataTable();

//            string query = @"
//        SELECT
//            c.name AS ""Клиент"",
//            p.name AS ""Товар"",
//            oi.quantity AS ""Количество"",
//            p.price AS ""Цена"",
//            (oi.quantity * p.price) AS ""Сумма"",
//            o.order_date AS ""Дата заказа""
//        FROM order_items oi
//        JOIN orders o ON oi.order_id = o.id
//        JOIN clients c ON o.client_id = c.id
//        JOIN products p ON oi.product_id = p.id
//        WHERE
//            o.order_date BETWEEN @startDate AND @endDate
//            AND c.id = ANY(@clientIds)
//            AND oi.is_delivered = FALSE
//        ORDER BY o.order_date DESC;";


//            using (var cmd = new NpgsqlCommand(query, con))
//            {
//                cmd.Parameters.AddWithValue("@startDate", startDate);
//                cmd.Parameters.AddWithValue("@endDate", endDate);
//                cmd.Parameters.AddWithValue("@clientIds", clientIds.ToArray());

//                OpenConnection();
//                using (var reader = cmd.ExecuteReader())
//                {
//                    dt.Load(reader);
//                }
//                CloseConnection();
//            }

//            return dt;
//        }

//        public DataTable getAllClientDeliveries(string client)
//        {


//            string query = @$"SELECT
//                            o.id AS ""Номер заказа"",
//                            o.order_date AS ""Дата заказа"",
//                            p.name AS ""Товар"",
//                            oi.quantity AS ""Количество"",
//                            oi.is_delivered AS ""Доставлено""
//                            FROM
//                            clients c
//                            JOIN orders o ON c.id = o.client_id
//                            JOIN order_items oi ON o.id = oi.order_id
//                            JOIN products p ON oi.product_id = p.id
//                            WHERE
//                                c.id = {getClientId(client)} 
//                            AND oi.is_delivered = FALSE;";

//            DataTable dt = new DataTable();
//            OpenConnection();
//            try
//            {
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(query, con);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что пошло не так!{ex.Message}");
//            }
//            CloseConnection();
//            return dt;
//        }
//        public DataTable getAllClientPay(string client)
//        {


//            string query = @$"SELECT
//                            oi.id AS ""Номер позиции заказа"",
//                            o.id AS ""Номер заказа"",
//                                o.order_date AS ""Дата заказа"",
//                                    p.name AS ""Товар"",
//                                oi.quantity AS ""Количество"",
//                                        p.price AS ""Цена"",
//                                            (oi.quantity * p.price) AS ""Общая стоимость"",
//                                oi.paid_amount AS ""Сумма оплачено"",
//                                    oi.is_paid AS ""Оплатить""
//                            FROM
//                                clients c
//                                JOIN orders o ON c.id = o.client_id
//                                JOIN order_items oi ON o.id = oi.order_id
//                                JOIN products p ON oi.product_id = p.id
//                                WHERE
//                                    c.id = {getClientId(client)} -- Укажите ID клиента
//                                AND oi.is_paid = FALSE;";

//            DataTable dt = new DataTable();
//            OpenConnection();
//            try
//            {
//                NpgsqlDataAdapter adap = new NpgsqlDataAdapter(query, con);
//                adap.Fill(dt);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что пошло не так!{ex.Message}");
//            }
//            CloseConnection();
//            return dt;
//        }


//        public void updateDeliveredStatusProduct(int order_id, string product)
//        {

//            string query = @$"UPDATE order_items
//                                SET is_delivered = true 
//                            WHERE 
//                                order_id = @order_id 
//                            AND product_id = @id;";
//            OpenConnection();
//            using (var transaction = con.BeginTransaction())
//            {

//                try
//                {
//                    using (var cmd = new NpgsqlCommand(query, con, transaction))
//                    {
//                        int productId = getProductId(product, transaction);
//                        cmd.Parameters.AddWithValue("@order_id", order_id);
//                        cmd.Parameters.AddWithValue("@id", productId);
//                        cmd.ExecuteNonQuery();
//                    }
//                    transaction.Commit();
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    MessageBox.Show($"Что то пошло не так!{ex.Message}");
//                }
//                finally
//                {
//                    CloseConnection();
//                }
//            }

//        }
//        public void updatePaidStatusProduct(int order_id, int order_item_id, string product, decimal amount)
//        {

//            string query = @$"
//                      INSERT INTO payments (order_id, order_item_id, amount)
//                    VALUES (@order_id, @order_item_id, @amount); ";
//            OpenConnection();
//            using (var transaction = con.BeginTransaction())
//            {

//                try
//                {
//                    using (var cmd = new NpgsqlCommand(query, con, transaction))
//                    {
//                        //int productId = getProductId(product, transaction);
//                        cmd.Parameters.AddWithValue("@order_id", order_id);
//                        cmd.Parameters.AddWithValue("@order_item_id", order_item_id);
//                        cmd.Parameters.AddWithValue("@amount", amount);
//                        cmd.ExecuteNonQuery();
//                    }
//                    transaction.Commit();
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    MessageBox.Show($"Что то пошло не так!{ex.Message}");
//                }
//                finally
//                {
//                    CloseConnection();
//                }
//            }

//        }

//        public DataTable getClientOrders(string clientName)
//        {
//            string query = @"
//                        SELECT 
//                        id as ""Номер заказа"",
//                        order_date as ""Дата"",
//                        CASE 
//                             WHEN is_active THEN 'Активный' 
//                             ELSE 'Завершен' 
//                            END AS ""Статус""
//                            FROM orders 
//                            WHERE client_id = @client_id";
//            DataTable dt = new DataTable();
//            int clientId = getClientId(clientName);
//            OpenConnection();
//            try
//            {
//                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
//                {
//                    cmd.Parameters.AddWithValue("@client_id", clientId);
//                    NpgsqlDataAdapter adap = new NpgsqlDataAdapter(cmd);
//                    adap.Fill(dt);

//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Что то пошло не так{ex.Message}!");
//            }
//            CloseConnection();
//            return dt;


//        }
//    }
//}
