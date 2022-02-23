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

namespace _20220131_Connect
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection(@"Server=DESKTOP-2JD3BUB; Database= Northwind; Trusted_Connection=True");

        string name;
        string phone;

        public Form1()
        {
            InitializeComponent();
            FillTable();
        }

        void GetData()
        {
            name = name_textbox.Text;
            phone= phone_textbox.Text;
        }

        bool Exists()
        {
            bool exists;

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand check_command = new SqlCommand($"select * from shippers where companyname='{name}'", connection);            

            SqlDataReader reader = check_command.ExecuteReader();
            

            if (reader.HasRows)
            {
                exists = true;
            }

            else
            {
                exists = false;
            }

            connection.Close();

            return (exists);           
        }

        public void FillTable()
        {
            connection.Open();

            DataSet shippers = new DataSet();
            SqlDataAdapter data_adapter = new SqlDataAdapter("select * from shippers", connection);
            data_adapter.Fill(shippers);
            datagridview.DataSource = shippers.Tables[0];

            connection.Close();
        }

        void Execute(string command_text,SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand target_command = new SqlCommand(command_text, connection);
            target_command.ExecuteReader();

            connection.Close();
        }

        private void insert_button_Click(object sender, EventArgs e)
        {
            GetData();
            InsertMethod();
            FillTable();
        }

        void InsertMethod()
        {
            if (name.Trim() == "")
            {
                MessageBox.Show("Lütfen şirket ismi giriniz.");
            }

            else if (Exists())
            {
                MessageBox.Show("Zaten bu isimde bir şirket var. İsterseniz telefon numarasını güncelleyebilirsiniz.");
            }

            else
            {
                Execute(($"Insert into shippers values('{name}',{phone})"), connection);
            }            
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            GetData();
            Update_();
            FillTable();
        }

        void Update_()
        {
            if (Exists())
            {
                Execute($"Update shippers set phone={phone} where companyname='{name}'", connection);
            }

            else
            {
                MessageBox.Show("Bu isimde bir şirket bulunmamaktadır. İsterseniz yeni kayıt oluşturabilirsiniz.");
            }            
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            GetData();
            Delete();
            FillTable();
        }

        void Delete()
        {
            if (Exists())
            {
                Execute($"Delete from shippers where companyname='{name}'", connection);
            }

            else
            {
                MessageBox.Show("Böyle bir şirket zaten bulunmamaktadır.");
            }            
        }
    }
}
