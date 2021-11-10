using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace BetaMart
{
    public partial class Form1 : Form
    {
        private string id = "";
        private int row = 0;

        public Form1()
        {
            InitializeComponent();
            resetMe();
        }

        private void resetMe()
        {
            kodeTextBox.Text = "";
            namaTextBox.Text = "";
            hargaTextBox.Text = "";
            if (kategoriComboBox.Items.Count > 0)
            {
                kategoriComboBox.SelectedIndex = 0;
            }
            updateButton.Text = "UPDATE";
            deleteButton.Text = "DELETE";
            keywordTextBox.Clear();
            keywordTextBox.Select();
            stokTextBox.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(keywordTextBox.Text.Trim()))
            {
                loadData(this.keywordTextBox.Text.Trim());
            }
            else
            {
                loadData("");
            }

            resetMe();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new OleDbCommand(mySQL, CRUD.con);
            AddParameters(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void AddParameters(string str)
        {
            CRUD.cmd.Parameters.Clear();
            if(str == "Delete" && !string.IsNullOrEmpty(this.id)){
                CRUD.cmd.Parameters.AddWithValue("id", this.id);
            }

            CRUD.cmd.Parameters.AddWithValue("kodeBarang", kodeTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("namaBarang", namaTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("harga", hargaTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("kategori", kategoriComboBox.SelectedItem.ToString().Trim());
            CRUD.cmd.Parameters.AddWithValue("stok", stokTextBox.Text.Trim());

            if (str == "Update" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("id", this.id);
            }


        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(kodeTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(namaTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(hargaTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(stokTextBox.Text.Trim())) {
                MessageBox.Show("Lengkapi data dengan lengkap!",
                    "Insert Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
        
            }

            CRUD.sql = "INSERT INTO tbl_stok(kode_barang, nama_barang, harga, kategori, stok) VALUES(@kodeBarang, @namaBarang, @harga, @kategori, @stok)";
            execute(CRUD.sql, "Insert");

            MessageBox.Show("Berhasil Menyimpan Data",
                    "Insert Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");
            resetMe();

        }

        private void loadData(string keyword)
        {
            CRUD.sql = "SELECT kode_barang, nama_barang, harga, kategori, stok FROM tbl_stok WHERE kode_barang LIKE @keyword1 OR nama_barang = @keyword2";
            string strKeyword = string.Format("%{0}%", keyword);
            CRUD.cmd = new OleDbCommand(CRUD.sql, CRUD.con);
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("keyword1", strKeyword);
            CRUD.cmd.Parameters.AddWithValue("keyword2", keyword);

            DataTable dt = CRUD.PerformCRUD(CRUD.cmd);
            if(dt.Rows.Count > 0)
            {
                row = Convert.ToInt32(dt.Rows.Count.ToString());
            }
            else
            {
                row = 0;
            }

            toolStripStatusLabel1.Text = "Number of row(s): " + row.ToString();

            DataGridView dgv = dataGridView1;
            dgv.MultiSelect = false;
            dgv.AutoGenerateColumns = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DataSource = dt;
            dgv.Columns[0].HeaderText = "Kode Barang";
            dgv.Columns[1].HeaderText = "Nama Barang";
            dgv.Columns[2].HeaderText = "Harga";
            dgv.Columns[3].HeaderText = "Kategori";
            dgv.Columns[4].HeaderText = "Stok";

            dgv.Columns[0].Width = 95;
            dgv.Columns[1].Width = 220;
            dgv.Columns[2].Width = 170;
            dgv.Columns[3].Width = 170;
            dgv.Columns[4].Width = 100;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridView dgv = dataGridView1;
                this.id = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                updateButton.Text = "UPDATE";
                deleteButton.Text = "DELETE";

                kodeTextBox.Enabled = false;
                kodeTextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[0].Value).Trim();
                namaTextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[1].Value).Trim();
                hargaTextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[2].Value).Trim();
                kategoriComboBox.SelectedItem = Convert.ToString(dgv.CurrentRow.Cells[3].Value).Trim();
                stokTextBox.Text = Convert.ToString(dgv.CurrentRow.Cells[4].Value).Trim();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Mohon select item data terlebih dahulu!",
                    "Update Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(kodeTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(namaTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(hargaTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(stokTextBox.Text.Trim()))
            {
                MessageBox.Show("Lengkapi data dengan lengkap!",
                    "Update Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string query = "UPDATE tbl_stok SET nama_barang = '" + namaTextBox.Text + "', harga = '"+hargaTextBox.Text+"', kategori = '" + kategoriComboBox.Text + "', stok = '"+stokTextBox.Text+"' WHERE kode_barang = '"+kodeTextBox.Text+"'";
            MessageBox.Show(query);
            MessageBox.Show("Berhasil Mengupdate Data",
                    "Update Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");
            resetMe();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Mohon select item data terlebih dahulu!",
                    "Delete Data : Betamart",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Apakah Anda Yakin Menghapus Data Ini?", "Delete Data : Betamart",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                CRUD.sql = "DELETE FROM tbl_stok WHERE kode_barang = @kodeBarang";
                execute(CRUD.sql, "Delete");

                MessageBox.Show("Berhasil Menghapus Data",
                        "Delete Data : Betamart",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                kodeTextBox.Enabled = true;
                loadData("");
                resetMe();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
