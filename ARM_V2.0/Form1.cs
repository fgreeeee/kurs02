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
using System.Threading;

namespace ARM_V2._0
{

    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form1 : Form
    {
        Thread th;

        DataBase database = new DataBase();
        
        int selectedRow;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("Product", "Product");
            dataGridView1.Columns.Add("Stat", "Stat");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("ID_Client", "ID_Client");
            dataGridView1.Columns.Add(" Date_Inst", "Дата  Date_Inst");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow(DataGridView dwg, IDataRecord record)
        {
            dwg.Rows.Add( record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), record.GetDateTime(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dwg)
        {
            dwg.Rows.Clear();

            string queryString = $"select * from  OrderRec";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dwg, reader);
            }
            reader.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }


        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from  OrderRec where concat (id, Product, Stat, Price, ID_Client, Date_Inst) like '%" + Text_Find.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }

            read.Close();
        }

        private void Text_Find_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void DeleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
        }


        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            int id;
            var Product = text_KOD.Text;
            var Stat = STAT_comboBox1.Text;
            var Price = text_PRICE.Text;
            var ID_Client = text_IDKLIENT.Text;
            var Date_Inst = text_DATE.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if(int.TryParse(Text_ID.Text, out id))
                dataGridView1.Rows[selectedRowIndex].SetValues(id, Product, Stat, Price, ID_Client, Date_Inst);
                dataGridView1.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
            }

        }

        private void RTN_CHANGE_Click(object sender, EventArgs e)
        {
            Change();
        }


        private void UpdateTab()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from OrderRec where id = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var Product = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var Stat = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var Price = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var ID_Client = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var Date_Inst = dataGridView1.Rows[index].Cells[5].Value.ToString();
                


                    var changeQuery = $"update OrderRec set Product = '{Product}', Stat = '{Stat}', Price = '{Price}', ID_Client = '{ID_Client}', Date_Inst = '{Date_Inst}' where id = '{id}'";
                    var command = new SqlCommand(changeQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            database.closeConnection();
        }


        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            UpdateTab();
        }

        private void BTN_DELETE_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                Text_ID.Text = row.Cells[0].Value.ToString();
                text_KOD.Text = row.Cells[1].Value.ToString();
                STAT_comboBox1.Text = row.Cells[2].Value.ToString();
                text_PRICE.Text = row.Cells[3].Value.ToString();
                text_IDKLIENT.Text = row.Cells[4].Value.ToString();
                text_DATE.Text = row.Cells[5].Value.ToString();
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        public void open(object obj)
        {
            Application.Run(new Form2());
        }

        private void NEW_button_Click(object sender, EventArgs e)
        {
            addNew addfrm = new addNew();
            addfrm.Show();
        }
    }


}
