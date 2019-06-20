using System;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using HeartValve.Shared.Data;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Telerik.Web.UI;

namespace HeartValve.Inventory
{
    public partial class AddEquip : System.Web.UI.Page
    {

        private List<GetEquipmentItems_Result> EquipmentList
        {
            get
            {
                var obj = Session["Equipment"];
                if (obj == null)
                {
                    HeartValveEntities db = new HeartValveEntities();
                   
                    var list = db.GetEquipmentItems().ToList();

                    Session["Equipment"] = list;
                    return list;
                }
                else return (List<GetEquipmentItems_Result>)Session["Equipment"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session["Equipment"] = null;

        }

        protected void EquipTable_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EquipTable.MasterTableView.DataSource = EquipmentList;
        }

        protected void EquipTable_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack && this.EquipTable.MasterTableView.Items.Count > 1)
            {
                this.EquipTable.MasterTableView.Rebind();
            }
        }

        //@equipTypeID int,
        //@manufacturer varchar(50),
        //@serialNo varchar(100),
        //@modelNo varchar(100),
        //@macAddr varchar(100),
        //@acqDate datetime,
        //@retiredDate datetime,
        //@active bit
        protected void EquipTable_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

            GridEditableItem editedItem = e.Item as GridEditableItem;
            int id = Convert.ToInt32(editedItem["InventoryID"].Text);


            DropDownList equipTypeDD = (DropDownList)editedItem.FindControl("EquipType");
            TextBox manufacturerTB = (TextBox)editedItem.FindControl("Manufacturer");
            TextBox serialNoTB = (TextBox)editedItem.FindControl("SerialNo");
            TextBox modelNoTB = (TextBox)editedItem.FindControl("ModelNo");
            TextBox macAddressTB = (TextBox)editedItem.FindControl("MacAddr");
            TextBox bluetoothModelNoTB = (TextBox)editedItem.FindControl("BluetoothModelNo");
            TextBox phoneNoTB = (TextBox)editedItem.FindControl("PhoneNo");


            //RadDatePicker acqDateDP = (RadDatePicker)editedItem.FindControl("AquiredDate");
            //RadDatePicker retiredDateDP = (RadDatePicker)editedItem.FindControl("RetiredDate");
            CheckBox activeCB = (CheckBox)editedItem.FindControl("Active");
            DropDownList purchaserDD = (DropDownList)editedItem.FindControl("PurchasedBy");
            DropDownList cuffsizesDD = (DropDownList)editedItem.FindControl("BpCuffSizeId");

            DropDownList employeeDD = (DropDownList)editedItem.FindControl("Employee");

            HeartValveEntities db = new HeartValveEntities();

            GetEquipmentItems_Result item = EquipmentList.Find(i => i.InventoryID == id);

            try
            {
                db.UpdateEquipmentItem(id, Convert.ToInt32(equipTypeDD.SelectedValue), manufacturerTB.Text, serialNoTB.Text, modelNoTB.Text, phoneNoTB.Text, macAddressTB.Text,
                    bluetoothModelNoTB.Text,Convert.ToInt32(purchaserDD.SelectedValue), Convert.ToInt32(cuffsizesDD.SelectedValue), activeCB.Checked, User.Identity.Name);

                if (!String.IsNullOrEmpty(employeeDD.SelectedValue) && employeeDD.SelectedValue.Equals(item.UserID))
                {
                    //return old assignment
                    if (item.assignmentID != null)
                    {
                        Debug.WriteLine("\nThis is the selected User: " + item.assignmentID + "\n");
                        Debug.WriteLine("\nThis is the selected User: " + User.Identity.GetUserId() +"\n");

                        string currentUserID = User.Identity.GetUserId();///gets the user AspNetUserId

                        db.ReturnEquipmentAssignment(item.assignmentID, User.Identity.Name, null, 5);
                        //db.ReturnEquipmentAssignment(item.assignmentID, currentUserID, null, 5);
                    }

                    Debug.WriteLine("\nThis is the selected employeeDD: " + employeeDD.SelectedValue);
                    Debug.WriteLine("\nThis is the selected id: " + id);
                    Debug.WriteLine("\nThis is the selected User: " + User.Identity.Name);

                    ObjectParameter assignID = new ObjectParameter("returnID", typeof(Int32));
                    ObjectParameter name = new ObjectParameter("name", typeof(String));
                    db.AddEquipmentAssignment(employeeDD.SelectedValue, id, User.Identity.Name, null,  assignID, name);

                    item.UserID = employeeDD.SelectedValue;
                    item.name = Convert.ToString(name.Value);
                    item.assignmentID = Convert.ToInt32(assignID.Value);
                }

                Debug.WriteLine("\nThis is the selected employeeDD: " + Convert.ToInt32(equipTypeDD.SelectedValue));
                Debug.WriteLine("\nThis is the selected id: " + id);
                Debug.WriteLine("\nThis is the selected User: " + User.Identity.Name);
                item.EquipTypeID = Convert.ToInt32(equipTypeDD.SelectedValue);
                item.EquipDescription = equipTypeDD.SelectedItem.Text;
                item.Manufacturer = manufacturerTB.Text;
                item.SerialNo = serialNoTB.Text;
                item.ModelNo = modelNoTB.Text;
                item.PhoneNo = phoneNoTB.Text;

                item.MacAddr = macAddressTB.Text;
                item.BluetoothModelNo = bluetoothModelNoTB.Text;

                //item.acqDate = acqDateDP.SelectedDate;
                //item.retiredDate = retiredDateDP.SelectedDate;
                item.Active = activeCB.Checked;

                item.Purchaser = purchaserDD.SelectedItem.Text;
                item.PurchaserID = Convert.ToInt32(purchaserDD.SelectedValue);
                item.Size = cuffsizesDD.SelectedItem.Text;
                item.BpCuffSizeId = Convert.ToInt32(cuffsizesDD.SelectedValue);
            }
            catch (Exception exc)
            {
                Label errorMsg = new Label();
                errorMsg.Text = "Unable to update inventory item. Reason: " + exc.Message;
                errorMsg.ForeColor = System.Drawing.Color.Red;
                EquipTable.Controls.Add(errorMsg);
            }
        }
        protected int? passRef(ref int? x)
        {  // OK, x  is known to be assigned

            return x;
        }

        protected void EquipTable_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            DropDownList equipTypeDD = (DropDownList)editedItem.FindControl("EquipType");
            TextBox manufacturerTB = (TextBox)editedItem.FindControl("Manufacturer");
            TextBox serialNoTB = (TextBox)editedItem.FindControl("SerialNo");
            TextBox modelNoTB = (TextBox)editedItem.FindControl("ModelNo");
            TextBox macAddressTB = (TextBox)editedItem.FindControl("MacAddr");
            TextBox bluetoothModelNoTB = (TextBox)editedItem.FindControl("BluetoothModelNo");
            TextBox phoneNoTB = (TextBox)editedItem.FindControl("PhoneNo");


            //RadDatePicker acqDateDP = (RadDatePicker)editedItem.FindControl("AquiredDate");
            //RadDatePicker retiredDateDP = (RadDatePicker)editedItem.FindControl("RetiredDate");
            //CheckBox activeCB = (CheckBox)editedItem.FindControl("Active");
            DropDownList purchaserDD = (DropDownList)editedItem.FindControl("PurchasedBy");
            DropDownList cuffsizesDD = (DropDownList)editedItem.FindControl("BpCuffSizeId");

            DropDownList employeeDD = (DropDownList)editedItem.FindControl("Employee");

            //InventoryDataContextDataContext context = new InventoryDataContextDataContext();
            //GetEquipmentItemsResult item = new GetEquipmentItemsResult();
            
            HeartValveEntities db = new HeartValveEntities();
            ObjectParameter id = new ObjectParameter("return_id", typeof(Int32));

            GetEquipmentItems_Result item = new GetEquipmentItems_Result();

            try
            {
                db.AddEquipmentItem(Convert.ToInt32(equipTypeDD.SelectedValue), manufacturerTB.Text, serialNoTB.Text, modelNoTB.Text, phoneNoTB.Text,
                    macAddressTB.Text, bluetoothModelNoTB.Text, Convert.ToInt32(purchaserDD.SelectedValue), Convert.ToInt32(cuffsizesDD.SelectedValue), DateTime.Now, true, User.Identity.Name, id);
                //create initial assignment
                if (!String.IsNullOrEmpty(employeeDD.SelectedValue))
                {
                    //int? assignID = null;
                    //string name = null;
                    ObjectParameter assignID = new ObjectParameter("returnID", typeof(Int32));
                    ObjectParameter name = new ObjectParameter("name", typeof(String));

                    db.AddEquipmentAssignment(employeeDD.SelectedValue, Convert.ToInt32(id.Value), User.Identity.Name, null,assignID,name);

                    item.UserID = Convert.ToString(employeeDD.SelectedValue);
                    item.name = Convert.ToString(name.Value);
                    item.assignmentID = Convert.ToInt32(assignID.Value);
                }

                item.InventoryID = Convert.ToInt32(id.Value);
                item.EquipDescription = equipTypeDD.SelectedItem.Text;
                item.EquipTypeID = Convert.ToInt32(equipTypeDD.SelectedValue);
                item.Manufacturer = manufacturerTB.Text;
                item.SerialNo = serialNoTB.Text;
                item.ModelNo = modelNoTB.Text;
                item.ModelNo = modelNoTB.Text;

                item.MacAddr = macAddressTB.Text;
                item.BluetoothModelNo = bluetoothModelNoTB.Text;
                //item.acqDate = acqDateDP.SelectedDate;
                //item.active = activeCB.Checked;
                item.Active = false;
                item.Purchaser = purchaserDD.SelectedItem.Text;
                item.PurchaserID = Convert.ToInt32(purchaserDD.SelectedValue);
                item.Size = cuffsizesDD.SelectedItem.Text;
                item.BpCuffSizeId = Convert.ToInt32(cuffsizesDD.SelectedValue); 
                item.PhoneNo = phoneNoTB.Text;

                EquipmentList.Add(item);
            }
            catch (Exception exc)
            {
                Label errorMsg = new Label();
                errorMsg.Text = "Unable to add inventory item. Reason: " + exc.Message;
                errorMsg.ForeColor = System.Drawing.Color.Red;
                EquipTable.Controls.Add(errorMsg);
            }
        }

        protected void EquipTable_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                GridEditFormItem editForm = (GridEditFormItem)e.Item;
                DropDownList typeDD = (DropDownList)editForm.FindControl("EquipType");
                DropDownList purchaserDD = (DropDownList)editForm.FindControl("PurchasedBy");
                DropDownList cuffsizesDD = (DropDownList)editForm.FindControl("BpCuffSizeId"); 
                 DropDownList employeeDD = (DropDownList)editForm.FindControl("Employee");
                Label employeeL = (Label)editForm.FindControl("EmployeeL");
                CheckBox activeCB = (CheckBox)editForm.FindControl("Active");

                HeartValveEntities db = new HeartValveEntities();
                var equipTypes = db.GetEquipTypeLU();

                typeDD.DataSource = equipTypes;
                typeDD.DataTextField = "EquipDescription";
                typeDD.DataValueField = "EquipTypeID";
                typeDD.DataBind();

                typeDD.SelectedValue = DataBinder.Eval(e.Item.DataItem, "EquipTypeID").ToString();

                var purchasers = db.GetPurchasersLU();
                purchaserDD.DataSource = purchasers;
                purchaserDD.DataTextField = "Purchaser";
                purchaserDD.DataValueField = "PurchaserID";
                purchaserDD.DataBind();
                purchaserDD.SelectedValue = DataBinder.Eval(e.Item.DataItem, "PurchaserID").ToString();

                var cuffsizes = db.GetBPMCuffSizeLU();

                cuffsizesDD.DataSource = cuffsizes;
                cuffsizesDD.DataTextField = "Size";
                cuffsizesDD.DataValueField = "BpCuffSizeId";
                cuffsizesDD.DataBind();
                cuffsizesDD.SelectedValue = DataBinder.Eval(e.Item.DataItem, "BpCuffSizeId").ToString();

                var employees = db.GetAllUsers();

                employeeDD.DataSource = employees;
                employeeDD.DataTextField = "FullName";
                employeeDD.DataValueField = "ID";
                employeeDD.DataBind();
                employeeDD.Items.Insert(0, new ListItem(String.Empty, String.Empty));

                var empID = DataBinder.Eval(e.Item.DataItem, "UserID");
                employeeDD.Visible = false;
                employeeL.Visible = false;
                if (empID != null)
                    employeeDD.SelectedValue = empID.ToString();

                if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                {
                    //insert
                    activeCB.Visible = false;
                }
                else
                {
                    //edit
                    bool active = false;

                    if (Boolean.TryParse(DataBinder.Eval(e.Item.DataItem, "Active").ToString(), out active))
                        activeCB.Checked = active;

                    if (!active)
                    {
                        employeeDD.Visible = false;
                        employeeL.Visible = false;
                    }
                }
            }

            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                LinkButton retireBtn = (LinkButton)item["retireBtn"].Controls[0];
                TableCell activeCell = item["active"];

                if (!Convert.ToBoolean(activeCell.Text))
                {
                    retireBtn.Visible = false;
                }
            }
        }

        protected void EquipTable_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Retire")
            {
                GridDataItem item = (GridDataItem)e.Item;
                int id = (int)item.GetDataKeyValue("InventoryID");

                DateTime currentDate = DateTime.Now;
                var row = EquipmentList.Find(i => i.InventoryID == id);
                row.retiredDate = currentDate;
                row.Active = false;

                HeartValveEntities db = new HeartValveEntities();

                db.RetireEquipment(id, User.Identity.Name);
                EquipTable.Rebind();
            }
        }
    }
}