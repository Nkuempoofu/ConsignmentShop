using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>(); // shoppingCartData => will contain everything in the shopping cart list
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();
        private decimal storeProfit = 0;


        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData(); // the ConsignmentShop constructor is going to call the SetupData method which is a temporary method thats going to create some dummy data for us 

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList(); // the binding source links our items to the binding source 
            itemsListbox.DataSource = itemsBinding;

            itemsListbox.DisplayMember = "Display";
            itemsListbox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListbox.DataSource = cartBinding;

            shoppingCartListbox.DisplayMember = "Display";
            shoppingCartListbox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListbox.DataSource = vendorsBinding;

            vendorListbox.DisplayMember = "Display";
            vendorListbox.ValueMember = "Display";

        }
        private void SetupData()
        {
            //this method creates a new instance of type new Vendor,it then populates that instance with initial values of firstname, lastname and commission.  
            store.Vendors.Add(new Vendor { FirstName = "Nkue", LastName = "Mpofu" }); 
            store.Vendors.Add(new Vendor { FirstName = "Henry", LastName = "Ncube" });

            store.Items.Add(new Item { Title = "Dan Brown",
                                        Description = "The Dan Vinci Code", 
                                        Price = 10.50M, // the M suffix tells the compiler that the value is a double not a decimal 
                                        Owner = store.Vendors[0] });

            store.Items.Add(new Item { Title = "Scott Fitzgerald",
                                        Description = "The Great Gatsby",
                                        Price = 12.50M, 
                                        Owner = store.Vendors[0] });

            store.Items.Add(new Item { Title = "Marcel Proust",
                                        Description = "In Search of Lost Time",
                                        Price = 9.50M,
                                        Owner = store.Vendors[0] });

            store.Items.Add(new Item { Title = "Moby Dick",
                                        Description = "Herman Melville",
                                        Price = 11.50M,
                                        Owner = store.Vendors[1] });

            store.Items.Add(new Item { Title = "Gabriel Garcia Marquez",
                                        Description = "One Hundred Years of Solitude",
                                        Price = 10.50M,
                                        Owner = store.Vendors[1] });

            store.Items.Add(new Item { Title = "William Golding",
                                        Description = "Lord Of The Flies",
                                        Price = 9.50M,
                                        Owner = store.Vendors[1] });

            store.Name = "Seconds are Better";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            // addToCart click button will:
            // 1. Figure out what is selected from the items list.
            // 2. Copy that item to the shopping cart.
            // 3. Remove the item selected from the items list.

            Item selectedItem = (Item)itemsListbox.SelectedItem;

            shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false);
        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            // makePurchase click button will: 
            foreach (Item item in shoppingCartData)
            {
                item.Sold = true;    // 1. mark each item in the cart as sold
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price; //
                storeProfit += (1 - (decimal)item.Owner.Commission) * item.Price;
            }

            shoppingCartData.Clear(); // 2. Clear the data in the cart

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("${0}", storeProfit);

            cartBinding.ResetBindings(false);

            itemsBinding.ResetBindings(false);

            vendorsBinding.ResetBindings(false);
        }
    }
}
