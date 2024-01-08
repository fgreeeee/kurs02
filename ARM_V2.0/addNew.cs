using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ARM_V2._0
{
    public partial class addNew : Form
    {
        DataBase dataBase = new DataBase();

        public addNew()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void SAVE_button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            var Product = PROD_Box1.Text;
            var Stat = STAT_comboBox1.Text;
            var Price = PRICE_textBox1.Text;
            int Client;
            var Data_c = dateTimePicker1.Value;

            if(int.TryParse(CLIENTID_textBox1.Text, out Client))
            {
                var addquery = $"insert into OrderRec (Product, Stat, Price, ID_Client, Date_Inst) values ('{Product}', '{Stat}', '{Price}', '{Client}', '{Data_c}')";

                var command  = new SqlCommand(addquery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запись создана", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Не удалось создать запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataBase.closeConnection();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void New_CLINTbutton1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            var FIO = NewClient_tBox1.Text;
            var Number = NUMBER_textBox1.Text;

            var addquery = $"insert into Client (Fio, Number) values ('{FIO}', '{Number}')";

            var command = new SqlCommand(addquery, dataBase.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Запись создана", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();

        }
    }
}
