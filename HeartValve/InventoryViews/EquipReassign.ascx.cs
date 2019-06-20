using System;
using HeartValve.Shared.Data;

namespace HeartValve.Inventory
{
    public partial class EquipReassign : System.Web.UI.UserControl
    {
        private object _dataItem = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object DataItem
        {
            get
            {
                return this._dataItem;
            }
            set
            {
                this._dataItem = value;
            }
        }

        protected void ReturnReason_DataBinding(object sender, EventArgs e)
        {
            HeartValveEntities db = new HeartValveEntities();

            var returnReasons = db.GetEquipReturnReasonsLU();

            ReturnReason.DataSource = returnReasons;
            ReturnReason.DataTextField = "ReasonDescription";
            ReturnReason.DataValueField = "ReturnReasonID";
        }

        protected void Employee_DataBinding(object sender, EventArgs e)
        {
            HeartValveEntities db = new HeartValveEntities();

            var employees = db.GetAllUsers();

            Employee.DataSource = employees;
            Employee.DataTextField = "FullName";
            Employee.DataValueField = "ID";
        }
    }
}