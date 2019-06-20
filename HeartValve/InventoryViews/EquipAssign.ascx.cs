﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using HeartValve.Shared.Data;


namespace HeartValve.Inventory
{
    public partial class EquipAssign : System.Web.UI.UserControl
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