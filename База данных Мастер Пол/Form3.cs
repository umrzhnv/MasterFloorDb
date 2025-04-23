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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Или другое значение
        }

        private void LoadData()
        {
            // 1. Строка подключения (Замените на свою!  Теперь LocalDB)
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;Connect Timeout=30";

            // 2. SQL-запрос
            string query = "SELECT * FROM Импортируемая_продукция"; // Замените на свой запрос

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 3. DataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // 4. DataSet
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Импортируемая_продукция"); // Или любое другое имя таблицы в DataSet

                    // 5. Привязка к DataGridView
                    dataGridView1.DataSource = ds.Tables["Импортируемая_продукция"]; // Или имя таблицы, указанное выше
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }
    }
}
