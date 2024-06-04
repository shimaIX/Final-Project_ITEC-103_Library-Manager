using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Library_Manager
{
    public partial class Transaction : UserControl
    {

        SqlConnection connectionString = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True");
        string checker = "false";
        public Transaction()
        {
            InitializeComponent();

            this.Load += new EventHandler(Transaction_Load);
            comboBox_Sort_Category.Items.AddRange(new string[]
       {
            "StudentNum",
            "Name",
            "YCS",
            "TitleBook",
            "BorrowedDate",
            "ReturnedDate",
       });
            comboBox1.Items.AddRange(new string[]
      {
            "StudentNum",
            "Name",
            "YCS",
            "TitleBook",
            "BorrowedDate",
            "ReturnedDate",
      });


            comboBox_Sort_Category.SelectedIndexChanged += comboBox_Sort_Category_SelectedIndexChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            button_ListOfBooks_Sort.Click += button_ListOfBooks_Sort_Click_1;
            maskedTextBox1.TextChanged += maskedTextBox1_TextChanged;
            maskedTextBox2.TextChanged += maskedTextBox2_TextChanged;
            maskedTextBox1.MaskInputRejected += maskedTextBox1_MaskInputRejected;
            maskedTextBox2.MaskInputRejected += maskedTextBox2_MaskInputRejected;
        }




        private void LoadData()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [transaction]", connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteSelectedRow()
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("No row selected for deletion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Do you want to delete the entire row?", "Delete Row?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;

                if (rowIndex < 0 || rowIndex >= dataGridView1.RowCount)
                {
                    MessageBox.Show("Invalid row index.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dataGridView1.Rows[rowIndex].Cells["studentID"].Value == null)
                {
                    MessageBox.Show("Invalid student ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int studentID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["studentID"].Value);

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM [transaction] WHERE studentID = @studentID";

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@studentID", studentID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Reload the data to reflect the changes
                    LoadData();
                    DefaultSort();

                    MessageBox.Show("You have successfully deleted the selected row.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Total(); // Update totals or other dependent logic if necessary
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting row: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DefaultSort()
        {
            comboBox_Sort_Category.SelectedIndex = 0;
            comboBox_Sort_Order.SelectedIndex = 0;
            SortDataTable();
        }

        private void SortDataTable()
        {
            if (dataGridView1.DataSource is DataTable dataTable)
            {
                string sortColumn = "";
                switch (comboBox_Sort_Category.SelectedItem.ToString())
                {
                    case "StudentNum":
                        sortColumn = "StudentNum";
                        break;
                    case "Name":
                        sortColumn = "Name";
                        break;
                    case "YCS":
                        sortColumn = "YCS";
                        break;
                    case "TitleBook":
                        sortColumn = "TitleBook";
                        break;
                    case "BorrowedDate":
                        sortColumn = "BorrowedDate";
                        break;
                    case "ReturnedDate":
                        sortColumn = "ReturnedDate";
                        break;
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    string sortOrder = comboBox_Sort_Order.SelectedItem.ToString() == "Ascending" ? "ASC" : "DESC";
                    dataTable.DefaultView.Sort = $"{sortColumn} {sortOrder}";
                    dataGridView1.DataSource = dataTable.DefaultView.ToTable();
                }
            }

        }

        private void Total()
        {

            int columnIndex1 = 5; // Index of the BorrowedDate column
            int total1 = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell value is not null
                //if (row.Cells[columnIndex1].Value != null)
                //{
                //    total1 += 1;
                //}

                if (!String.IsNullOrWhiteSpace(row.Cells[columnIndex1].Value?.ToString()))
                {
                    total1 += 1;
                }

            }

            int columnIndex2 = 6; // Index of the ReturnedDate column
            int total2 = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell value is not null 
                //if (row.Cells[columnIndex2].Value != null)
                //{
                //    total2 += 1;
                //}

                if (!String.IsNullOrWhiteSpace(row.Cells[columnIndex2].Value?.ToString()))
                {
                    total2 += 1;
                }

            }

            total1 -= total2;

            LoadData();
            label_totalbookstitle.Text = dataGridView1.RowCount.ToString();
            label_totalbooksmissing.Text = total1.ToString();
            label5.Text = total2.ToString();
        }

        private void Transaction_Load(object sender, EventArgs e)
        {
            LoadData();
            DefaultSort();
            Total();
            ColumnWidth();
            radioButton_Borrowed.Checked = true;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ColumnWidth()
        {
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[4].Width = 150;
        }

        private void button_Transaction_Add_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                

                connect.Open();
                string query = "SELECT * FROM library WHERE Title LIKE @searchTerm";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
                string check = textBox_Transaction_TitleOfBook.Text.Trim(); // Make sure you're getting the search term from the correct text box.
                CheckBooks(check);
                adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + check + "%");
                DataTable dt = new DataTable();

                if (checker == "true")
                {

                    if (button_Transaction_Add.Text == "ADD")
                    {
                        CheckIfEmptyOrNot("Cannot update if empty.");
                        if (thereIsTrue == true)
                        {
                            return;
                        }
                        try
                        {

                            DataTable table = new DataTable();
                            string insertData = "INSERT INTO [transaction] (StudentNum, Name, YCS, TitleBook, BorrowedDate, ReturnedDate) " +
                            "VALUES(@StudentNum, @Name, @YCS, @TitleBook, @BorrowedDate, @ReturnedDate)";

                            using (SqlCommand cmd = new SqlCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@StudentNum", textBox_Transaction_StudentNo.Text.Trim());
                                cmd.Parameters.AddWithValue("@Name", textBox_Transaction_Name.Text.Trim());
                                cmd.Parameters.AddWithValue("@YCS", textBox_Transaction_YCS.Text.Trim());
                                cmd.Parameters.AddWithValue("@TitleBook", textBox_Transaction_TitleOfBook.Text.Trim());
                                cmd.Parameters.AddWithValue("@BorrowedDate", maskedTextBox1.Text.Trim());
                                cmd.Parameters.AddWithValue("@ReturnedDate", maskedTextBox2.Text.Trim());

                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Registered successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            ClearEverything();
                            LoadData();
                            DefaultSort();
                            Total();
                            checker = "false";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        CheckIfEmptyOrNot("Cannot update if empty.");
                        if (thereIsTrue == true)
                        {
                            return;
                        }

                        try
                        {
                            int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                            int studentID = Convert.ToInt32(selectedRow.Cells["studentID"].Value);

                            string studentNum = textBox_Transaction_StudentNo.Text.Trim();
                            string name = textBox_Transaction_Name.Text.Trim();
                            string ycs = textBox_Transaction_YCS.Text.Trim();
                            string titleBook = textBox_Transaction_TitleOfBook.Text.Trim();

                            string borrowedDate = maskedTextBox1.Text.Trim();
                            string returnedDate = maskedTextBox2.Text.Trim();

                            string qry = "UPDATE [transaction] SET StudentNum=@StudentNum, Name=@Name, YCS=@YCS, TitleBook=@TitleBook, BorrowedDate=@BorrowedDate, ReturnedDate=@ReturnedDate WHERE studentID=@StudentID";
                            using (SqlCommand sc = new SqlCommand(qry, connect))
                            {
                                sc.Parameters.AddWithValue("@StudentNum", studentNum);
                                sc.Parameters.AddWithValue("@Name", name);
                                sc.Parameters.AddWithValue("@YCS", ycs);
                                sc.Parameters.AddWithValue("@TitleBook", titleBook);
                                sc.Parameters.AddWithValue("@BorrowedDate", borrowedDate);
                                sc.Parameters.AddWithValue("@ReturnedDate", returnedDate);
                                sc.Parameters.AddWithValue("@StudentID", studentID);

                                int i = sc.ExecuteNonQuery();
                                if (i >= 1)
                                    MessageBox.Show(i + " Record Updated Successfully: " + studentNum, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    MessageBox.Show("Failed to update the record. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            ClearEverything();
                            LoadData();
                            DefaultSort();
                            Total();
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("Error: " + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("No books found with the title given " + check, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        bool thereIsTrue = false;

        private void CheckIfEmptyOrNot(string a)
        {
            bool checkStudentNo = (textBox_Transaction_StudentNo.Text == "") ? true : false;
            bool checkName = (textBox_Transaction_Name.Text == "") ? true : false;
            bool checkYCS = (textBox_Transaction_YCS.Text == "") ? true : false;
            bool checkTitleOfBook = (textBox_Transaction_TitleOfBook.Text == "") ? true : false;
            bool checkBorrowed = (maskedTextBox1.Text == "") ? true : false;
            bool checkReturned = (maskedTextBox2.Text == "") ? true : false;

            if (checkStudentNo == true || checkName == true || checkYCS == true || checkTitleOfBook == true)
            {
                MessageBox.Show(a, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                thereIsTrue = true;
                return;
            }
            else
            {
                thereIsTrue = false;
                return;
            }
        }

        bool update;

        private void button_Transaction_Edit_Click(object sender, EventArgs e)
        {

        }

        private void button_Transaction_Clear_Click(object sender, EventArgs e)
        {
            if (button_Transaction_Clear.Text == "CLEAR")
            {
                ClearEverything();
                return;
            }
        }

        private void ClearEverything()
        {
            textBox_Transaction_StudentNo.Clear();
            textBox_Transaction_Name.Clear();
            textBox_Transaction_YCS.Clear();
            textBox_Transaction_TitleOfBook.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            

            //radioButton_Borrowed.Checked = false;
            //radioButton_Returned.Checked = false;
        }

        private void button_Transaction_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete entire row?", "Delete Row?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(rowIndex);
                MessageBox.Show("You have successfully deleted the selected row.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }
        }

        bool returnedDate = false;



        int indexRow;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (update == true)
            {
                indexRow = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                textBox_Transaction_StudentNo.Text = selectedRow.Cells[0].Value.ToString();
                textBox_Transaction_Name.Text = selectedRow.Cells[1].Value.ToString();
                textBox_Transaction_YCS.Text = selectedRow.Cells[2].Value.ToString();
                textBox_Transaction_TitleOfBook.Text = selectedRow.Cells[3].Value.ToString();
                //returnedYes.Checked = false;
                //returnedNo.Checked = false;
                //borrowedYes.Checked = false;
                //borrowedNo.Checked = false;
            }
            else
            {
                ClearEverything();
            }

        }



        private void button_ListOfBooks_Cancel_Click(object sender, EventArgs e)
        {
            DefaultSort();
        }



        private void SortColumn(string columnName, bool ascending)
        {
            // Sort the DataGridView by the specified column

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button_Search_Find_Click_1(object sender, EventArgs e)
        {

        }

        private void button_ListOfBooks_Sort_Click_1(object sender, EventArgs e)
        {
            SortDataTable();
        }

        private void comboBox_Sort_Category_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Transaction_StudentNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Transaction_YCS_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel_ListOfBooks_UpperRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [transaction]", connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox_Transaction_Status_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            if (update == true)
            {
                indexRow = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                textBox_Transaction_StudentNo.Text = selectedRow.Cells[1].Value.ToString();
                textBox_Transaction_Name.Text = selectedRow.Cells[2].Value.ToString();
                textBox_Transaction_YCS.Text = selectedRow.Cells[3].Value.ToString();
                textBox_Transaction_TitleOfBook.Text = selectedRow.Cells[4].Value.ToString();
                maskedTextBox1.Text = selectedRow.Cells[5].Value.ToString();
                maskedTextBox2.Text = selectedRow.Cells[6].Value.ToString();


                return;
            }

            ClearEverything();
        }

        private void label_totalbookstitle_Click(object sender, EventArgs e)
        {

        }

        private void label_totalbooksmissing_Click(object sender, EventArgs e)
        {

        }

        private void button_Transaction_Edit_Click_1(object sender, EventArgs e)
        {
            if (button_Transaction_Edit.Text == "EDIT")
            {
                update = true;
                button_Transaction_Edit.Text = "CANCEL";
                button_Transaction_Add.Text = "UPDATE";
                button_Transaction_Clear.Text = "";
            }
            else
            {
                update = false;
                button_Transaction_Edit.Text = "EDIT";
                button_Transaction_Add.Text = "ADD";
                button_Transaction_Clear.Text = "CLEAR";
                ClearEverything();
            }
        }

        private void button_Transaction_Delete_Click_1(object sender, EventArgs e)
        {
            DeleteSelectedRow();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox_Sort_Order_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortDataTable();
        }

        private void radioButton_Returned_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void radioButton_Borrowed_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Borrowed.Checked)
            {
                maskedTextBox2.Enabled = false; // Disable the MaskedTextBox
            }
            else
            {
                maskedTextBox2.Enabled = true; // Enable the MaskedTextBox if the radio button is unchecked
            }
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text) || maskedTextBox1.Text == maskedTextBox1.Mask.Replace('0', ' '))
            {
                maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            }
            else
            {
                maskedTextBox1.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox2.Text) || maskedTextBox2.Text == maskedTextBox2.Mask.Replace('0', ' '))
            {
                maskedTextBox2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            }
            else
            {
                maskedTextBox2.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            }
        }


        private void textBox_Find_SearchBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button_Search_Find_Click_2(object sender, EventArgs e)
        {
            string searchTerm = textBox_Find_SearchBox.Text.Trim(); // Make sure you're getting the search term from the correct text box.
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchBooks(searchTerm);
            }
            else
            {
                MessageBox.Show("Please enter a search term.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_Search_Cancel_Click_1(object sender, EventArgs e)
        {
            textBox_Find_SearchBox.Clear();
            LoadData(); // Reload data to show all records after clearing search.
            DefaultSort();
        }


        private void SearchBooks(string searchTerm)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = null;
                string selectedItem = comboBox1.SelectedItem?.ToString();

                switch (selectedItem)
                {
                    case "StudentNum":
                        query = "SELECT * FROM [transaction] WHERE StudentNum LIKE @searchTerm";
                        break;
                    case "Name":
                        query = "SELECT * FROM [transaction] WHERE Name LIKE @searchTerm";
                        break;
                    case "YCS":
                        query = "SELECT * FROM [transaction] WHERE YCS LIKE @searchTerm";
                        break;
                    case "TitleBook":
                        query = "SELECT * FROM [transaction] WHERE TitleBook LIKE @searchTerm";
                        break;
                    case "BorrowedDate":
                        query = "SELECT * FROM [transaction] WHERE BorrowedDate LIKE @searchTerm";
                        break;
                    case "ReturnedDate":
                        query = "SELECT * FROM [transaction] WHERE ReturnedDate LIKE @searchTerm";
                        break;
                    default:
                        MessageBox.Show("Please select a valid search category.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No books found with the given " + searchTerm, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CheckBooks(string check)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM library WHERE Title LIKE @searchTerm";
     
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + check + "%");
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        checker = "true";
                        return;
                    }
                    else
                    {
                        return;
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_Transaction_Clear_Click_1(object sender, EventArgs e)
        {
            if (button_Transaction_Clear.Text == "CLEAR")
            {
                ClearEverything();
                Total();
                return;
            }


        }

        private void button_ListOfBooks_Cancel_Click_1(object sender, EventArgs e)
        {
            DefaultSort();
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            label7.Visible = true;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }
    }

    }


