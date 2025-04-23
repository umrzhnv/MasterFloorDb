using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace База_данных_Мастер_Пол
{
    public partial class Form5 : Form
    {
        // Строка подключения к базе данных (замените на свою!)
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\USERS\\ADMIN\\SOURCE\\REPOS\\БАЗА ДАННЫХ МАСТЕР ПОЛ\\БАЗА ДАННЫХ МАСТЕР ПОЛ\\DATABASE1.MDF;Integrated Security=True;Connect Timeout=30";
        private decimal partnerDiscount = 0;
        private decimal productPrice = 0;
        public decimal totalPrice=0;
        decimal quantity = 0;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadPartners();
            LoadProducts();
            LoadEmployees();
        }
        
        private void LoadPartners()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id_Партнера, Наименование_партнера FROM Партнеры";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxPartner.DataSource = dt;
                    comboBoxPartner.DisplayMember = "Наименование_партнера"; // Поле для отображения в ComboBox
                    comboBoxPartner.ValueMember = "Id_Партнера"; // Поле для хранения значения Id
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки партнеров: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id_Продукции, Наименование_продукции FROM Импортируемая_продукция";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxProduct.DataSource = dt;
                    comboBoxProduct.DisplayMember = "Наименование_продукции"; // Поле для отображения в ComboBox
                    comboBoxProduct.ValueMember = "Id_Продукции"; // Поле для хранения значения Id
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки продукции: " + ex.Message);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, ФИО FROM Сотрудники";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxEmployee.DataSource = dt;
                    comboBoxEmployee.DisplayMember = "ФИО"; // Поле для отображения в ComboBox
                    comboBoxEmployee.ValueMember = "ID"; // Поле для хранения значения Id
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки сотрудников: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {// Важно! Убедитесь, что connectionString правильно инициализирована где-то в вашем классе, например:
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Получение данных из формы. Валидация! Проверка на null/DBNull
                    if (comboBoxPartner.SelectedValue == null || comboBoxProduct.SelectedValue == null || comboBoxEmployee.SelectedValue == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите значения для всех обязательных полей (Партнер, Продукт, Сотрудник).");
                        return; // Прерываем выполнение, если чего-то не хватает
                    }

                    int partnerId;
                    int productId;
                    int employeeId;
                    

                    try
                    {
                        Console.WriteLine($"comboBoxPartner.SelectedValue: {comboBoxPartner.SelectedValue}");
                        Console.WriteLine($"comboBoxProduct.SelectedValue: {comboBoxProduct.SelectedValue}");
                        Console.WriteLine($"comboBoxEmployee.SelectedValue: {comboBoxEmployee.SelectedValue}");



                        partnerId = Convert.ToInt32(comboBoxPartner.SelectedValue);
                        productId = Convert.ToInt32(comboBoxProduct.SelectedValue);
                        employeeId = Convert.ToInt32(comboBoxEmployee.SelectedValue);
                    }
                    catch (Exception ex) // Ловим возможные ошибки при конвертации
                    {
                        MessageBox.Show($"Ошибка преобразования идентификаторов: {ex.Message}");
                        return; // Прерываем выполнение
                    }

                    string comment = textBoxComment.Text;
                    

                    Console.WriteLine($"partnerId: {partnerId}, productId: {productId}, employeeId: {employeeId}, comment: {comment}, totalPrice{totalPrice}"); // Логирование
                    string query = "INSERT INTO Заявка (Id_Партнера, Id_Продукции, Id_Сотрудника, Дата_заявки, Дата_обработки, Статус_заявки, Комментарий_сотрудника, Предполагаемая_цена, Количество_продукции) " +
                             "VALUES (@PartnerId, @ProductId, @EmployeeId, GETDATE(), GETDATE(), N'Новая', @Comment, @totalPrice, @quantity)";

                    // 3. Создание SqlCommand с параметрами (ВАЖНО для защиты от SQL-инъекций)
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@PartnerId", SqlDbType.Int).Value = partnerId; // Явно указываем тип данных
                        command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
                        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                        command.Parameters.Add("@Comment", SqlDbType.NVarChar, 255).Value = comment ?? "";  // Явно указываем тип и размер, обрабатываем null
                        command.Parameters.Add("@totalPrice", SqlDbType.Decimal).Value = totalPrice;
                        command.Parameters.Add("@quantity", SqlDbType.Decimal).Value = quantity;



                        // ?? "" - если comment == null, то присваиваем пустую строку, чтобы избежать ошибок

                        // 4. Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"rowsAffected: {rowsAffected}"); // Логирование

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Заявка успешно отправлена!");
                            // Очистка полей формы после успешной отправки (опционально)
                            comboBoxPartner.SelectedIndex = -1;
                            comboBoxProduct.SelectedIndex = -1;
                            comboBoxEmployee.SelectedIndex = -1;
                            textBoxComment.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Не удалось отправить заявку.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке заявки: " + ex.ToString()); // Логирование
                MessageBox.Show("Ошибка при отправке заявки: " + ex.Message);
            }

        }

        private void comboBoxPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPartner.SelectedValue != null)
            {
                try
                {
                    int partnerId = Convert.ToInt32(comboBoxPartner.SelectedValue);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT Скидка FROM Партнеры WHERE Id_Партнера = @PartnerId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@PartnerId", partnerId);
                            object result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                partnerDiscount = Convert.ToDecimal(result);
                            }
                            else
                            {
                                partnerDiscount = 0;
                            }

                            labelDiscount.Text = $"Скидка партнера: {partnerDiscount:P}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки скидки: " + ex.Message);
                }
            }
            else
            {
                partnerDiscount = 0;
                labelDiscount.Text = "Скидка партнера: 0%";
            }
            UpdateCalculations();
        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProduct.SelectedValue != null)
            {
                try
                {
                    int productId = Convert.ToInt32(comboBoxProduct.SelectedValue);
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT ЦенаЗаЕдиницу FROM Импортируемая_продукция WHERE Id_Продукции = @ProductId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ProductId", productId);
                            object result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                productPrice = Convert.ToDecimal(result);
                            }
                            else
                            {
                                productPrice = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки цены товара: " + ex.Message);
                    productPrice = 0;
                }
            }
            else
            {
                productPrice = 0;
            }
            UpdateCalculations();
        }

        private void numericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdateCalculations();
        }

        private void UpdateCalculations()
        {
            quantity = numericUpDownQuantity.Value;
            decimal discountAmount = productPrice * partnerDiscount;
            decimal discountedPrice = productPrice - discountAmount;
            totalPrice = discountedPrice * quantity;

            labelPrice.Text = $"Цена товара: {productPrice:C}";
            labelDiscountAmount.Text = $"Скидка: {discountAmount:C}";
            labelDiscountedPrice.Text = $"Цена со скидкой: {discountedPrice:C}";
            labelTotalPrice.Text = $"Общая сумма: {totalPrice:C}";
        }
    }
}
