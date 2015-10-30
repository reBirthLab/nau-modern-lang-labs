using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace Phonebook
{
    public partial class MainForm : Form
    {
        static public string DBFile = Application.StartupPath + "\\database.xml";
        static public XDocument xDocument;
        static public string Caption = "Phonebook";

        public MainForm()
        {
            InitializeComponent();
        }

        void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                ItemForm newForm = new ItemForm(true, false);
                newForm.Text = "Add New Item";
                newForm.lableRegDate.Text = DateTime.Now.ToString();

                newForm.ShowDialog();
                LoadPhoneBookItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        void buttonClearSearchTextBox_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            LoadPhoneBookItems();
        }

        void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count < 1) return;

                string id = listView1.SelectedItems[0].Name.Replace("Item", "");

                var item = (from q in xDocument.Descendants("Item")
                            where q.Attribute("ID").Value == id
                            select q).First();
                if (item == null) return;

                ItemForm editForm = new ItemForm(false, true);
                editForm.Text = "Edit Item";
                editForm.textBoxAddress.Text = item.Attribute("Address").Value;
                editForm.textBoxEMail.Text = item.Attribute("Email").Value;
                editForm.textBoxMobile.Text = item.Attribute("Mobile").Value;
                editForm.textBoxName.Text = item.Attribute("Name").Value;
                editForm.textBoxPhone.Text = item.Attribute("Phone").Value;
                editForm.lableRegDate.Text = item.Attribute("RegDate").Value;
                editForm.ItemID = id;

                editForm.ShowDialog();
                LoadPhoneBookItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count < 1) return;
                if (MessageBox.Show("Are you sure to delete the item ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                string id = listView1.SelectedItems[0].Name.Replace("Item", "");
                var item = (from q in xDocument.Descendants("Item")
                            where q.Attribute("ID").Value == id
                            select q).First();
                item.Remove();
                WriteToFile(xDocument.ToString(), DBFile);
                LoadPhoneBookItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void LoadPhoneBookItems()
        {
            try
            {
                listView1.Items.Clear();

                var items = from q in xDocument.Descendants("Item") select q;
                if (items.Count() < 1)
                {
                    Text = Caption + ": 0 Contacts";
                    return;
                }

                foreach (var item in items)
                {
                    ListViewItem listViewItems;

                    listViewItems = new ListViewItem(new string[]
                                    { item.Attribute("Name").Value,
                                          item.Attribute("Phone").Value,
                                          item.Attribute("Mobile").Value,
                                          item.Attribute("Email").Value,
                                          item.Attribute("Address").Value,
                                          item.Attribute("RegDate").Value});

                    listViewItems.Name = "Item" + item.Attribute("ID").Value;
                    listView1.Items.Add(listViewItems);

                    int contactsNumbers = xDocument.Descendants("Item").Count();
                    Text = Caption + ": " + contactsNumbers.ToString() + " Contacts";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {

                if (!File.Exists(DBFile))
                {
                    xDocument = new XDocument(

                        new XComment("\n Don't edit manually \n"),

                        new XElement("PhoneBook",
                            new XElement("Items")));

                    WriteToFile(xDocument.ToString(), DBFile);
                }

                xDocument = XDocument.Parse(ReadFromFile(DBFile));
                LoadPhoneBookItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);

                try
                {
                    File.Delete(DBFile);
                }
                catch
                {
                    MessageBox.Show("Please delete the DataBase file", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSearch.Text.Trim() == "")
                {
                    LoadPhoneBookItems();
                    return;
                }

                dateTimePicker1.Value = DateTime.Now;
                listView1.Items.Clear();

                var query = from q in xDocument.Descendants("Item")
                            where q.Attribute("Name").Value.ToLower().Contains(textBoxSearch.Text.Trim().ToLower()) ||
                                 q.Attribute("Phone").Value.ToLower().Contains(textBoxSearch.Text.Trim().ToLower()) ||
                                 q.Attribute("Mobile").Value.ToLower().Contains(textBoxSearch.Text.Trim().ToLower()) ||
                                 q.Attribute("Email").Value.ToLower().Contains(textBoxSearch.Text.Trim().ToLower()) ||
                                 q.Attribute("Address").Value.ToLower().Contains(textBoxSearch.Text.Trim().ToLower())
                            select q;

                if (query.Count() < 1)
                {
                    Text = Caption + ": 0 Contacts";
                    return;
                }

                foreach (var item in query)
                {
                    ListViewItem listViewItems = new ListViewItem(new string[]
                                                        { item.Attribute("Name").Value,
                                                          item.Attribute("Phone").Value,
                                                          item.Attribute("Mobile").Value,
                                                          item.Attribute("Email").Value,
                                                          item.Attribute("Address").Value,
                                                          item.Attribute("RegDate").Value});
                    listViewItems.Name = "Item" + item.Attribute("ID").Value;
                    listView1.Items.Add(listViewItems);
                }

                Text = Caption + ": " + listView1.Items.Count + " Contacts";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            buttonEdit_Click(null, null);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dateTimePicker1.Value == DateTime.Now)
                {
                    LoadPhoneBookItems();
                    return;
                }

                textBoxSearch.Text = "";
                listView1.Items.Clear();

                var query = from q in xDocument.Descendants("Item")
                            where q.Attribute("RegDate").Value.Contains(dateTimePicker1.Value.Date.ToShortDateString())
                            select q;

                if (query.Count() < 1)
                {
                    Text = Caption + ": 0 Contacts";
                    return;
                }

                foreach (var item in query)
                {
                    ListViewItem listViewItems = new ListViewItem(new string[]
                                                        { item.Attribute("Name").Value,
                                                          item.Attribute("Phone").Value,
                                                          item.Attribute("Mobile").Value,
                                                          item.Attribute("Email").Value,
                                                          item.Attribute("Address").Value,
                                                          item.Attribute("RegDate").Value});
                    listViewItems.Name = "Item" + item.Attribute("ID").Value;
                    listView1.Items.Add(listViewItems);
                }

                Text = Caption + ": " + listView1.Items.Count + " Contacts";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        public static void WriteToFile(String Data, String FileName)
        {
            try
            {
                FileStream fStream = File.Open(FileName, FileMode.Create);
                StreamWriter sWriter = new StreamWriter(fStream);

                sWriter.WriteLine(Data);

                sWriter.Close();
                fStream.Close();
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
            }
        }

        public static string ReadFromFile(String FileName)
        {
            try
            {
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);
                StreamReader sReader = new StreamReader(fStream);

                string val = sReader.ReadToEnd();

                sReader.Close();
                fStream.Close();

                return val;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file access error occurred: {0}", e.Message);
                return null;
            }
        }
    }
}
