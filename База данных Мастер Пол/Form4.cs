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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            // 1. Строка подключения (Замените на свою!  Теперь LocalDB)
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            // 2. SQL-запрос
            string query = "SELECT * FROM Партнеры"; // Замените на свой запрос

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 3. DataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // 4. DataSet
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Партнеры"); // Или любое другое имя таблицы в DataSet

                    // 5. Привязка к DataGridView
                    dataGridView1.DataSource = ds.Tables["Партнеры"]; // Или имя таблицы, указанное выше
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }
    }
}
