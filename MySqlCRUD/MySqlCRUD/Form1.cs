using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlCRUD
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=localhost;Database=trabalhopoo_server;Uid=root;Pwd=7glF@mpjN66;";
        int RegistrationID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("RegistrationAddOrEdit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_RegistrationID", RegistrationID);
                mySqlCmd.Parameters.AddWithValue("_Partnumber", txtPartnumber.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Local", txtLocal.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Quantity", txtQuantity.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Manufacturer", txtManufacturer.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Description", txtDescription.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Cadastrado com sucesso!");
                Clear();
                GridFill();
            }
        }
            void GridFill()
            {
                using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
                {
                    mysqlCon.Open();
                    MySqlDataAdapter sqlDa = new MySqlDataAdapter("RegistrationViewAll", mysqlCon);
                    sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dtblRegistration = new DataTable();
                    sqlDa.Fill(dtblRegistration);
                    dgvRegistration.DataSource = dtblRegistration;
                    dgvRegistration.Columns[0].Visible = false;
                }
            }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridFill();
        }

        void Clear()
        {
            txtPartnumber.Text = txtManufacturer.Text = txtLocal.Text = txtQuantity.Text = txtDescription.Text = txtSearch.Text = "";
            RegistrationID = 0;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            if (dgvRegistration.CurrentRow.Index != -1)
            {
                txtPartnumber.Text = dgvRegistration.CurrentRow.Cells[1].Value.ToString();
                txtManufacturer.Text = dgvRegistration.CurrentRow.Cells[2].Value.ToString();
                txtLocal.Text = dgvRegistration.CurrentRow.Cells[3].Value.ToString();
                txtQuantity.Text = dgvRegistration.CurrentRow.Cells[4].Value.ToString();
                txtDescription.Text = dgvRegistration.CurrentRow.Cells[5].Value.ToString();
                RegistrationID = Convert.ToInt32(dgvRegistration.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Atualizar";
                btnDelete.Enabled = Enabled;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("RegistrationSearchByValue", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                DataTable dtblRegistration = new DataTable();
                sqlDa.Fill(dtblRegistration);
                dgvRegistration.DataSource = dtblRegistration;
                dgvRegistration.Columns[0].Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("RegistrationDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_RegistrationID", RegistrationID);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deletado com sucesso!");
                Clear();
                GridFill();
            }
        }


    }
}
