using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Phonebook
{
    public partial class ItemForm : Form
    {
        public string ItemID = "";

        bool NewItem = false;
        bool EditItem = false;

        public ItemForm(bool newItem, bool editItem)
        {
            InitializeComponent();
            
            NewItem = newItem;
            EditItem = editItem;

            if (NewItem)
                Text = "Add New Item";

            else if (EditItem)
                Text = "Edit Item";

        }

        void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {

        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.Clear();
                errorProvider2.Clear();

                if (NewItem)
                {
                    if (textBoxName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(textBoxName, "Please insert a name");
                    }

                    if (textBoxMobile.Text.Trim() == "")
                    {
                        errorProvider2.SetError(textBoxMobile, "Please insert a mobile number");
                    }

                    if (textBoxName.Text.Trim() == "" || textBoxMobile.Text.Trim() == "") return;

                    int maxID = 0;

                    try
                    {
                        maxID = (from q in MainForm.xDocument.Descendants("Item") select (int)q.Attribute("ID")).Max();
                    }
                    catch
                    {
                    }

                    maxID++;

                    XElement newItem = new XElement("Item",
                                   new XAttribute("ID", maxID),
                                   new XAttribute("Name", textBoxName.Text.Trim()),
                                   new XAttribute("Mobile", textBoxMobile.Text.Trim()),
                                   new XAttribute("Phone", textBoxPhone.Text.Trim()),
                                   new XAttribute("Email", textBoxEMail.Text.Trim()),
                                   new XAttribute("Address", textBoxAddress.Text.Trim()),
                                   new XAttribute("RegDate", DateTime.Now.ToString()));

                    var ItemsElement = (from q in MainForm.xDocument.Descendants("Items")
                                        select q).First();
                    ItemsElement.Add(newItem);
                }
                else if (EditItem)
                {
                    if (textBoxName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(textBoxName, "Please insert a name");
                    }

                    if (textBoxMobile.Text.Trim() == "")
                    {
                        errorProvider2.SetError(textBoxMobile, "Please insert a mobile number");
                    }

                    if (textBoxName.Text.Trim() == "" || textBoxMobile.Text.Trim() == "") return;

                    var theItem = (from q in MainForm.xDocument.Descendants("Item")
                                   where q.Attribute("ID").Value == this.ItemID
                                   select q).First();

                    theItem.Attribute("Name").Value = textBoxName.Text.Trim();
                    theItem.Attribute("Mobile").Value = textBoxMobile.Text.Trim();
                    theItem.Attribute("Phone").Value = textBoxPhone.Text.Trim();
                    theItem.Attribute("Email").Value = textBoxEMail.Text.Trim();
                    theItem.Attribute("Address").Value = textBoxAddress.Text.Trim();
                }

                MainForm.WriteToFile(MainForm.xDocument.ToString(SaveOptions.DisableFormatting), MainForm.DBFile);

                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
