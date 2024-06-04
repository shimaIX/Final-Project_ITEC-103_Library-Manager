using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Library_Manager
{
    public partial class ListOfBooks : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True");
        public ListOfBooks()
        {
            InitializeComponent();
            comboBox_Sort_Category.Items.AddRange(new string[]
       {
            "DDS",
            "Title",
            "Author",
            "Year Of Publication",
            "Amount of Copy",
            "Total Amount of Copy",
            "Date Added"
       });

            comboBox1.Items.AddRange(new string[]
       {
            "DDS",
            "Title",
            "Author",
            "Year Of Publication",
            "Amount of Copy",
            "Total Amount of Copy",
       });
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox_Sort_Category.SelectedIndexChanged += comboBox_Sort_Category_SelectedIndexChanged;
            button_ListOfBooks_Sort.Click += button_ListOfBooks_Sort_Click;
        }

        private void LoadData()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM library", connection);
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
            if (MessageBox.Show("Do you want to delete the entire row?", "Delete Row?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                int bookId = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["bookId"].Value);

                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Files\Programming\Visual Studio\C#\[1] PROJECT\Library Manager\NEW\Library Manager final\Final Project (2)\Final Project\Library Manager Final Version 2.0 - Copy\Resources\loginData.mdf"";Integrated Security=True";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM library WHERE bookId = @bookId";

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@bookId", bookId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Reload the data to reflect the changes
                    LoadData();
                    DefaultSort();

                    MessageBox.Show("You have successfully deleted the selected row.","" , MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                if (comboBox_Sort_Category.SelectedItem != null)
                {
                    switch (comboBox_Sort_Category.SelectedItem.ToString())
                    {
                        case "DDS":
                            sortColumn = "DDS";
                            break;
                        case "Title":
                            sortColumn = "Title";
                            break;
                        case "Year Of Publication":
                            sortColumn = "YearOfPublication";
                            break;
                        case "Amount of Copy":
                            sortColumn = "AmountOfCopy";
                            break;
                        case "Total Amount of Copy":
                            sortColumn = "TotalAmount";
                            break;
                        case "Date Added":
                            sortColumn = "DateAdded";
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    string sortOrder = "ASC";
                    if (comboBox_Sort_Order.SelectedItem != null)
                    {
                        sortOrder = comboBox_Sort_Order.SelectedItem.ToString() == "Ascending" ? "ASC" : "DESC";
                    }

                    dataTable.DefaultView.Sort = $"{sortColumn} {sortOrder}";
                    dataGridView1.DataSource = dataTable.DefaultView.ToTable();
                }
            }
        }

        private void ListOfBooks_Load(object sender, EventArgs e)
        {
            LoadData();
            Total();
            DefaultSort();
            ColumnWidthForListOfBooks();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Total()
        {
            int columnIndex1 = 6; // Index of the column you want to sum
            int total1 = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell value is not null and can be parsed as an integer
                if (row.Cells[columnIndex1].Value != null && int.TryParse(row.Cells[columnIndex1].Value.ToString(), out int cellValue))
                {
                    total1 += cellValue;
                }
            }

            int columnIndex2 = 7; // Index of the column you want to sum
            int total2 = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell value is not null and can be parsed as an integer
                if (row.Cells[columnIndex2].Value != null && int.TryParse(row.Cells[columnIndex2].Value.ToString(), out int cellValue))
                {
                    total2 += cellValue;
                }
            }

            label_totalbookstitle.Text = dataGridView1.RowCount.ToString();
            label_availablecopy.Text = total1.ToString(); // available copy
            label_overallcopy.Text = total2.ToString(); // total copy 
        }

        private void ColumnWidthForListOfBooks()
        {
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 146;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 150;
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[5].Width = 119;
            dataGridView1.Columns[6].Width = 100;
            dataGridView1.Columns[7].Width = 100;
            dataGridView1.Columns[8].Width = 110;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button_ListOfBooks_Add_Click(object sender, EventArgs e)
        {

            try
            {
                connect.Open();

                if (button_ListOfBooks_Add.Text == "ADD")
                {
                    CheckIfEmptyOrNot("Cannot update if empty.");
                    if (comboBox_ListOfBooks_Category.SelectedIndex == -1 || thereIsTrue)
                    {
                        return;
                    }

                    int min = int.Parse(textBox_ListOfBooks_Min.Text.Trim());
                    int max = int.Parse(textBox_ListOfBooks_Max.Text.Trim());
                    if (min > max)
                    {
                        MessageBox.Show("Total copies are greater than available copies." , "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string insertData = "INSERT INTO library (DDS, Title, Author, Category, YearOfPublication, AmountOfCopy, TotalAmount, DateAdded) " +
                                        "VALUES(@DDS, @Title, @Author, @Category, @YearOfPublication, @AmountOfCopy, @TotalAmount, @DateAdded)";
                    DateTime date = DateTime.Today;

                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@DDS", textBox_ListOfBooks_DDS.Text.Trim());
                        cmd.Parameters.AddWithValue("@Title", textBox_ListOfBooks_Title.Text.Trim());
                        cmd.Parameters.AddWithValue("@Author", textBox_ListOfBooks_Author.Text.Trim());
                        cmd.Parameters.AddWithValue("@Category", comboBox_ListOfBooks_Category.Text.Trim());
                        cmd.Parameters.AddWithValue("@YearOfPublication", textBox_ListOfBooks_YoP.Text.Trim());
                        cmd.Parameters.AddWithValue("@AmountOfCopy", textBox_ListOfBooks_Min.Text.Trim());
                        cmd.Parameters.AddWithValue("@TotalAmount", textBox_ListOfBooks_Max.Text.Trim());
                        cmd.Parameters.AddWithValue("@DateAdded", date);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Book added successfully.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearEverything();
                        DefaultSort();
                    }

                    LoadData();
                    DefaultSort();
                    Total();
                }
                else // UPDATE
                {
                    CheckIfEmptyOrNot("Cannot update if empty.");
                    if (comboBox_ListOfBooks_Category.SelectedIndex == -1 || thereIsTrue)
                    {
                        return;
                    }

                    int min = int.Parse(textBox_ListOfBooks_Min.Text.Trim());
                    int max = int.Parse(textBox_ListOfBooks_Max.Text.Trim());
                    if (min > max)
                    {
                        MessageBox.Show("Total copies are greater than available copies.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                    int bookID = Convert.ToInt32(selectedRow.Cells["bookId"].Value);

                    string dds = textBox_ListOfBooks_DDS.Text.Trim();
                    string title = textBox_ListOfBooks_Title.Text.Trim();
                    string author = textBox_ListOfBooks_Author.Text.Trim();
                    string category = comboBox_ListOfBooks_Category.Text.Trim();
                    string yearPub = textBox_ListOfBooks_YoP.Text.Trim();
                    
                    DateTime dateAdd = DateTime.Today;
                    
                    string qry = "UPDATE library SET DDS=@DDS, Title=@Title, Author=@Author, Category=@Category, YearOfPublication=@YearOfPublication, AmountOfCopy=@AmountOfCopy, TotalAmount=@TotalAmount, DateAdded=@DateAdded WHERE bookId=@bookId";
                    using (SqlCommand sc = new SqlCommand(qry, connect))
                    {
                        sc.Parameters.AddWithValue("@DDS", dds);
                        sc.Parameters.AddWithValue("@Title", title);
                        sc.Parameters.AddWithValue("@Author", author);
                        sc.Parameters.AddWithValue("@Category", category);
                        sc.Parameters.AddWithValue("@YearOfPublication", yearPub);
                        sc.Parameters.AddWithValue("@AmountOfCopy", min);
                        sc.Parameters.AddWithValue("@TotalAmount", max);
                        sc.Parameters.AddWithValue("@DateAdded", dateAdd);
                        sc.Parameters.AddWithValue("@bookId", bookID);

                        int i = sc.ExecuteNonQuery();
                        if (i >= 1)
                        {
                            MessageBox.Show(i + " Book Updated Successfully: " + title, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearEverything();
                            DefaultSort();
                        }
                        else
                        {
                            MessageBox.Show("Book update failed.");
                        }

                    }

                    LoadData();
                    Total();
                    DefaultSort();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error: " + exp.ToString());
            }
            finally
            {
                if (connect.State == System.Data.ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        bool thereIsTrue = false;

        private void CheckIfEmptyOrNot(string a)
        {
            bool checkDDS = (textBox_ListOfBooks_DDS.Text == "") ? true : false;
            bool checkTitle = (textBox_ListOfBooks_Title.Text == "") ? true : false;
            bool checkAuthor = (textBox_ListOfBooks_Author.Text == "") ? true : false;
            bool checkCategory = (comboBox_ListOfBooks_Category.SelectedItem == null) ? true : false;
            bool checkYearOfPublication = (textBox_ListOfBooks_YoP.Text == "") ? true : false;
            bool checkAvailableCopy = (textBox_ListOfBooks_Min.Text == "") ? true : false;
            bool checkTotalCopy = (textBox_ListOfBooks_Max.Text == "") ? true : false;

            if (checkDDS == true || checkTitle == true || checkAuthor == true || checkCategory == true || checkYearOfPublication == true || checkAvailableCopy == true || checkTotalCopy == true)
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

        private void button_ListOfBooks_Edit_Click(object sender, EventArgs e)
        {
            if(button_ListOfBooks_Edit.Text == "EDIT")
            {
                update = true;
                button_ListOfBooks_Edit.Text = "CANCEL";
                button_ListOfBooks_Add.Text = "UPDATE";
                button_ListOfBooks_Clear.Text = "";
            }
            else
            {
                update = false;
                button_ListOfBooks_Edit.Text = "EDIT";
                button_ListOfBooks_Add.Text = "ADD";
                button_ListOfBooks_Clear.Text = "CLEAR";
                ClearEverything();
            }
        }

        private void button_ListOfBooks_Clear_Click(object sender, EventArgs e)
        {
            if(button_ListOfBooks_Clear.Text == "CLEAR")
            {
                ClearEverything();
                Total();
                return;
            }

            
        }

        private void ClearEverything()
        {
            textBox_ListOfBooks_DDS.Clear();
            textBox_ListOfBooks_Title.Clear();
            textBox_ListOfBooks_Author.Clear();
            comboBox_ListOfBooks_Category.SelectedIndex = -1;
            textBox_ListOfBooks_YoP.Clear();
            textBox_ListOfBooks_Min.Clear();
            textBox_ListOfBooks_Max.Clear();
        }

        private void button_ListOfBooks_Delete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow();
            DefaultSort();
        }






        private void SortColumn(string columnName, bool ascending)
        {
            // Sort the DataGridView by the specified column
            dataGridView1.Sort(dataGridView1.Columns[columnName], ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
        }

        int indexRow;

        private SqlConnection GetConnect()
        {
            return connect;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e, SqlConnection connect)
        {
            

        }

        private void textBox_ListOfBooks_DDS_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numbers, period, backspace, and decimal separator
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Only allow one period or decimal separator
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            if (string.IsNullOrEmpty(textBox_ListOfBooks_DDS.Text))
            {
                // Reset the ComboBox selection if the TextBox is empty
                comboBox_ListOfBooks_Category.SelectedIndex = -1;
                return;
            }

            char firstChar = textBox_ListOfBooks_DDS.Text[0];

            if (firstChar == '0')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 0;
            }
            else if (firstChar == '1')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 1;
            }
            else if(firstChar == '2')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 2;
            }
            else if (firstChar == '3')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 3;
            }
            else if (firstChar == '4')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 4;
            }
            else if (firstChar == '5')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 5;
            }
            else if (firstChar == '6')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 6;
            }
            else if (firstChar == '7')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 7;
            }
            else if (firstChar == '8')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 8;
            }
            else if (firstChar == '9')
            {
                comboBox_ListOfBooks_Category.SelectedIndex = 9;
            }
            else
            {
                comboBox_ListOfBooks_Category.SelectedIndex = -1;
            }
        }

        private void textBox_ListOfBooks_YoP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits (0-9) and control characters like backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Ensure only 4 digits are entered
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            if (textBox.Text.Length >= 4 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox_ListOfBooks_Min_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits (0-9) and control characters like backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            if (textBox.Text.Length >= 3 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox_ListOfBooks_Max_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits (0-9) and control characters like backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            if (textBox.Text.Length >= 3 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void panel_ListOfBooks_UpperRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox_ListOfBooks_DDS_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (update == true)
            {
                indexRow = e.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                textBox_ListOfBooks_DDS.Text = selectedRow.Cells[1].Value.ToString();
                textBox_ListOfBooks_Title.Text = selectedRow.Cells[2].Value.ToString();
                textBox_ListOfBooks_Author.Text = selectedRow.Cells[3].Value.ToString();
                textBox_ListOfBooks_YoP.Text = selectedRow.Cells[5].Value.ToString();
                textBox_ListOfBooks_Min.Text = selectedRow.Cells[6].Value.ToString();
                textBox_ListOfBooks_Max.Text = selectedRow.Cells[7].Value.ToString();

                if (selectedRow.Cells[4].Value.ToString() != null)
                {
                    if (selectedRow.Cells[4].Value.ToString() == "Generalities")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Generalities";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Philosophy and Psychology")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Philosophy and Psychology";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Religion")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Religion";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Social Science")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Social Science";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Language")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Language";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Natural Science and Mathematics")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Natural Science and Mathematics";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Technology (Applied Sciences)")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Technology (Applied Sciences)";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Arts and Recreation")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Arts and Recreation";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Literature")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Literature";
                    }
                    else if (selectedRow.Cells[4].Value.ToString() == "Geography and History")
                    {
                        comboBox_ListOfBooks_Category.SelectedItem = "Geography and History";
                    }
                }
                return;
            }

            ClearEverything();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connect.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM library", connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void label_totalbookstitle_Click(object sender, EventArgs e)
        {

        }

        private void button_ListOfBooks_Sort_Click(object sender, EventArgs e)
        {
            SortDataTable();
        }

        private void comboBox_Sort_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortDataTable();
        }

     

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel_ListOfBooks_Portrait_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox_Sort_Order_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox_ListOfBooks_YoP_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Find_SearchBox_TextChanged(object sender, EventArgs e)
        {

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
                    case "DDS":
                        query = "SELECT * FROM library WHERE DDS LIKE @searchTerm";
                        break;
                    case "Title":
                        query = "SELECT * FROM library WHERE Title LIKE @searchTerm";
                        break;
                    case "Author":
                        query = "SELECT * FROM library WHERE Author LIKE @searchTerm";
                        break;
                    case "Category":
                        query = "SELECT * FROM library WHERE Category LIKE @searchTerm";
                        break;
                    case "Year Of Publication":
                        query = "SELECT * FROM library WHERE YearOfPublication LIKE @searchTerm";
                        break;
                    case "Amount of Copy":
                        query = "SELECT * FROM library WHERE AmountOfCopy LIKE @searchTerm";
                        break;
                    case "Total Amount of Copy":
                        query = "SELECT * FROM library WHERE TotalAmount LIKE @searchTerm";
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
                        MessageBox.Show("No records found with the given " + searchTerm, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void button_Search_Find_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox_Find_SearchBox.Text.Trim(); // Ensure this is the correct text box for the search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchBooks(searchTerm);
            }
            else
            {
                MessageBox.Show("Please enter a search term.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_Search_Cancel_Click(object sender, EventArgs e)
        {
            DefaultSort();
            textBox_Find_SearchBox.Clear();
            LoadData();
            DefaultSort();
        }

        private void button_ListOfBooks_Cancel_Click(object sender, EventArgs e)
        {
            DefaultSort();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

      
    }
}
