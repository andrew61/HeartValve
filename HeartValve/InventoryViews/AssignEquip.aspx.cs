using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using HeartValve.Shared.Data;


namespace HeartValve.Inventory
{
    public partial class AssignEquip : System.Web.UI.Page
    {
        private List<GetEquipmentAssignments_Result> AssignmentList
        {
            get
            {
                var obj = Session["Assignments"];
                if (obj == null)
                {
                    HeartValveEntities db = new HeartValveEntities();
                    var list = db.GetEquipmentAssignments().ToList();
                    Session["Assignments"] = list;
                    return list;
                }
                else return (List<GetEquipmentAssignments_Result>)Session["Assignments"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Assignments"] = null;
                Employee.DataBind();
                EquipType.DataBind();
            }
        }

        protected void AssignmentTable_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            AssignmentTable.MasterTableView.DataSource = AssignmentList;
            Filter();
        }

        protected void AssignmentTable_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack && this.AssignmentTable.MasterTableView.Items.Count > 1)
            {
                this.AssignmentTable.MasterTableView.Rebind();
            }
        }

        protected void AssignmentTable_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                LinkButton reassignBtn = (LinkButton)item["Reassign"].Controls[0];
                LinkButton assignBtn = (LinkButton)item["Assign"].Controls[0];
                LinkButton reclaimBtn = (LinkButton)item["Reclaim"].Controls[0];

                int? eeID = (int?)DataBinder.Eval(e.Item.DataItem, "UserEquipmentID");

                if (eeID == null)
                {
                    reassignBtn.Visible = false;
                    reclaimBtn.Visible = false;
                }
                else
                {
                    assignBtn.Visible = false;
                }

                int inventoryID = (int)item.GetDataKeyValue("InventoryID");

                HeartValveEntities db = new HeartValveEntities();

                GetEquipmentItem_Result equipItem = db.GetEquipmentItem(inventoryID).Single();
                string equipDesc = "Inventory ID:  " + inventoryID.ToString() + "\n"
                                 + "Equipment Type:  " + equipItem.EquipDescription + "\n"
                                 + "Manufacturer:  " + equipItem.Manufacturer + "\n"
                                 + "Serial #:  " + equipItem.SerialNo + "\n"
                                 + "Model #:  " + equipItem.ModelNo + "\n"
                                 + "Mac Address:  " + equipItem.MacAddr + "\n"
                                 + "Aquired Date:  " + equipItem.AcqDate.ToString();

                item["inventoryDesc"].ToolTip = equipDesc;
                item["inventoryDesc"].Text = equipItem.EquipDescription;
                item["Manufacturer"].ToolTip = equipDesc;
                item["Manufacturer"].Text = equipItem.Manufacturer;
                item["ModelNo"].ToolTip = equipDesc;
                item["ModelNo"].Text = equipItem.ModelNo;
                item["SerialNo"].ToolTip = equipDesc;
                item["SerialNo"].Text = equipItem.SerialNo;


            }
        }

        protected void AssignmentTable_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl control = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);

            HeartValveEntities db = new HeartValveEntities();

            //int id = Convert.ToInt32(editedItem["inventoryID"].Text);
            int id = (int)editedItem.GetDataKeyValue("InventoryID");
            ObjectParameter newAssignmentID = new ObjectParameter("returnID", typeof(Int32));
            ObjectParameter name = new ObjectParameter("name", typeof(String));

            try
            {
                if(((Button)e.CommandSource).Text == "Reclaim")
                {
                    DropDownList returnReasonDD = (DropDownList)control.FindControl("ReturnReason");
                    TextBox commentsTB = (TextBox)control.FindControl("Comments");
                    int assignmentID = (int)editedItem.GetDataKeyValue("UserEquipmentID");

                    db.ReturnEquipmentAssignment(assignmentID, User.Identity.Name, commentsTB.Text, Convert.ToInt32(returnReasonDD.SelectedValue));

                    GetEquipmentAssignments_Result assignment = AssignmentList.Find(i => i.InventoryID == id);

                    assignment.Comments = null;
                    assignment.DateGiven = null;
                    assignment.DateReturned = null;
                    assignment.UserEquipmentID = null;
                    assignment.UserId= null;
                    assignment.name = null;
                    assignment.ReasonDescription = null;
                    assignment.ReturnReasonID = null;
                }
                else if (((Button)e.CommandSource).Text == "Reassign")
                {
                    DropDownList returnReasonDD = (DropDownList)control.FindControl("ReturnReason");
                    DropDownList employeeDD = (DropDownList)control.FindControl("Employee");
                    TextBox commentsTB = (TextBox)control.FindControl("Comments");
                    int assignmentID = (int)editedItem.GetDataKeyValue("UserEquipmentID");

                    db.ReturnEquipmentAssignment(assignmentID, User.Identity.Name, null, Convert.ToInt32(returnReasonDD.SelectedValue));

                    db.AddEquipmentAssignment(employeeDD.SelectedValue, id, User.Identity.Name, commentsTB.Text,  newAssignmentID,  name);

                    GetEquipmentAssignments_Result assignment = AssignmentList.Find(i => i.InventoryID == id);

                    assignment.UserId = employeeDD.SelectedValue;
                    assignment.name = Convert.ToString(name.Value);
                    assignment.DateGiven = DateTime.Now;
                    assignment.UserEquipmentID = assignmentID;
                    assignment.Comments = commentsTB.Text;
                    assignment.ReasonDescription = null;
                    assignment.ReturnReasonID = null;
                    assignment.DateReturned = null;
                }
                else if (((Button)e.CommandSource).Text == "Assign")
                {
                    DropDownList employeeDD = (DropDownList)control.FindControl("Employee");
                    TextBox commentsTB = (TextBox)control.FindControl("Comments");



                    db.AddEquipmentAssignment(employeeDD.SelectedValue, id, User.Identity.Name, commentsTB.Text, newAssignmentID,  name);

                    GetEquipmentAssignments_Result assignment = AssignmentList.Find(i => i.InventoryID == id);

                    assignment.UserId = employeeDD.SelectedValue;
                    assignment.name = Convert.ToString(name.Value);
                    assignment.DateGiven = DateTime.Now;
                    assignment.UserEquipmentID = Convert.ToInt32(newAssignmentID.Value);
                    assignment.Comments = commentsTB.Text;
                }
            }
            catch(Exception exc)
            {
                Label errorMsg = new Label();
                errorMsg.Text = exc.Message;
                errorMsg.ForeColor = System.Drawing.Color.Red;
                AssignmentTable.Controls.Add(errorMsg);
            }
        }

        protected void AssignmentTable_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if(e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                if(e.CommandName == "Reclaim")
                {
                    AssignmentTable.MasterTableView.EditFormSettings.EditFormType = GridEditFormType.WebUserControl;
                    AssignmentTable.MasterTableView.EditFormSettings.UserControlName = "EquipReclaim.ascx";
                    item.FireCommandEvent("edit", e);
                }
                else if(e.CommandName == "Reassign")
                {
                    AssignmentTable.MasterTableView.EditFormSettings.EditFormType = GridEditFormType.WebUserControl;
                    AssignmentTable.MasterTableView.EditFormSettings.UserControlName = "EquipReassign.ascx";
                    item.FireCommandEvent("edit", e);
                }
                else if (e.CommandName == "Assign")
                {
                    AssignmentTable.MasterTableView.EditFormSettings.EditFormType = GridEditFormType.WebUserControl;
                    AssignmentTable.MasterTableView.EditFormSettings.UserControlName = "EquipAssign.ascx";
                    item.FireCommandEvent("edit", e);
                }
            }
        }

        protected void SelectedIndexChanged(object sender, EventArgs e)
        {
            //Filter();
            AssignmentTable.Rebind();
        }

        private void Filter()
        {
            List<GetEquipmentAssignments_Result> filteredResult = AssignmentList;

            bool assigned = AssignmentFilter.Items[0].Selected;
            bool unassigned = AssignmentFilter.Items[1].Selected;
            string employee = Employee.SelectedValue;
            string equipType = EquipType.SelectedValue;

            if(!assigned)
            {
                filteredResult = filteredResult.Where(i => i.UserEquipmentID == null).ToList();
            }
            if(!unassigned)
            {
                filteredResult = filteredResult.Where(i => i.UserEquipmentID != null).ToList();
            }
            if(employee != String.Empty)
            {
                filteredResult = filteredResult.Where(i => i.UserId == employee).ToList();
            }
            if(equipType != String.Empty)
            {
                filteredResult = filteredResult.Where(i => i.EquipTypeID == Convert.ToInt32(equipType)).ToList();
            }

            AssignmentTable.MasterTableView.DataSource = filteredResult;
        }

        protected void EquipType_DataBinding(object sender, EventArgs e)
        {
            HeartValveEntities db = new HeartValveEntities();

            var equipTypes = db.GetEquipTypeLU();

            EquipType.DataSource = equipTypes;
            EquipType.DataTextField = "EquipDescription";
            EquipType.DataValueField = "EquipTypeID";
        }

        protected void Employee_DataBinding(object sender, EventArgs e)
        {
            HeartValveEntities db = new HeartValveEntities();

            var employees = db.GetAllUsers();

            Employee.DataSource = employees;
            Employee.DataTextField = "FullName";
            Employee.DataValueField = "ID";
        }

        protected void EquipType_DataBound(object sender, EventArgs e)
        {
            EquipType.Items.Insert(0, new ListItem("ALL", String.Empty));
            EquipType.SelectedIndex = 0;
        }

        protected void Employee_DataBound(object sender, EventArgs e)
        {
            Employee.Items.Insert(0, new ListItem("ALL", String.Empty));
            Employee.SelectedIndex = 0;
        }
    }
}