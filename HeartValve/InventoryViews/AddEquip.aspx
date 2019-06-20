<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InventoryViews/InventoryPages.Master" CodeBehind="AddEquip.aspx.cs" Inherits="HeartValve.Inventory.AddEquip" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="head" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .btn-no-link
        {
            color: white !important;
        }

        .rad-form-control
        {
            display: block !important;
            width: 100% !important;
            height: 34px;
        }

        .rad-form-control table input.riTextBox
        {
            padding: 6px 12px;
            height: 34px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            border-color: #ccc !important;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }
    </style>
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="sManager" runat="server"></asp:ScriptManager>
    <h2>Add/Edit Equipment</h2>
    <telerik:RadGrid ID="EquipTable" EnableViewState="true" runat="server" AutoGenerateColumns="false" GridLines="None" AllowPaging="true"
        PageSize="20" AllowSorting="true" ShowStatusBar="true" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
        OnNeedDataSource="EquipTable_NeedDataSource" OnPreRender="EquipTable_PreRender" OnUpdateCommand="EquipTable_UpdateCommand"
        OnInsertCommand="EquipTable_InsertCommand" MasterTableView-EditMode="EditForms" Skin="Silk"
        OnItemDataBound="EquipTable_ItemDataBound" OnItemCommand="EquipTable_ItemCommand">
            <MasterTableView Width="100%" UseAllDataFields="true" DataKeyNames="InventoryID" ShowHeader="true" InsertItemDisplay="Top" CommandItemDisplay="TopAndBottom" InsertItemPageIndexAction="ShowItemOnFirstPage">
                <Columns>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandcolumn"></telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn UniqueName="InventoryID" DataField="InventoryID" HeaderText="ID"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="EquipDescription" DataField="EquipDescription" HeaderText="Equipment"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Manufacturer" DataField="manufacturer" HeaderText="Manufacturer"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="SerialNo" DataField="SerialNo" HeaderText="Serial Number"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="ModelNo" DataField="ModelNo" HeaderText="Model Number"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="PhoneNo" DataField="PhoneNo" HeaderText="Phone #"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Size" DataField="Size" HeaderText="BPM Cuffsize"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="BluetoothModelNo" DataField="BluetoothModelNo" HeaderText="Bluetooth Model Number"></telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn UniqueName="macAddr" DataField="macAddr" HeaderText="MAC Address"></telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn UniqueName="Purchaser" DataField="Purchaser" HeaderText="Assigned Location"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Active" DataField="Active" HeaderText="Active"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="AcqDate" DataField="AcqDate" HeaderText="Aquired Date" DataFormatString="{0:MM/dd/yyyy}"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Name" DataField="Name" HeaderText="Current User"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="RetiredDate" DataField="RetiredDate" HeaderText="Retired Date" DataFormatString="{0:MM/dd/yyyy}"></telerik:GridBoundColumn>
                    <telerik:GridButtonColumn UniqueName="RetireBtn" Text="Retire" CommandName="Retire" ButtonCssClass="btn btn-danger btn-no-link"></telerik:GridButtonColumn>
                    <%--<telerik:GridTemplateColumn UniqueName="retireBtn">
                        <ItemTemplate>
                            <asp:Button ID="BtnRetire" runat="server" CssClass="btn btn-danger" Text="Retire" CommandName="Retire" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.inventoryID") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                </Columns>
                <CommandItemSettings AddNewRecordText="Add New Record" RefreshText="Refresh" />
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <%--@equipTypeID int,
	                    @manufacturer varchar(50),
	                    @serialNo varchar(100),
	                    @modelNo varchar(100),
	                    @macAddr varchar(100),
	                    @acqDate datetime,
	                    @retiredDate datetime,
	                    @active bit--%>
                        <div style="padding:10px">
                            <div class="row" id="">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="EquipTypeL" runat="server" Text="Equipment Type" AssociatedControlID="EquipType"></asp:Label>
                                        <asp:DropDownList ID="EquipType" CssClass="form-control" runat="server" TabIndex="1"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="ManufacturerL" runat="server" Text="Manufacturer" AssociatedControlID="Manufacturer"></asp:Label>
                                        <asp:TextBox ID="Manufacturer" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.manufacturer") %>' TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="SerialNoL" runat="server" Text="Serial #" AssociatedControlID="SerialNo"></asp:Label>
                                        <asp:TextBox ID="SerialNo" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.serialNo") %>' TabIndex="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="ModelNoL" runat="server" Text="Model #" AssociatedControlID="ModelNo"></asp:Label>
                                        <asp:TextBox ID="ModelNo" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.modelNo") %>' TabIndex="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="PhoneNumberL" runat="server" Text="Phone #" AssociatedControlID="PhoneNo"></asp:Label>
                                        <asp:TextBox ID="PhoneNo" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.phoneNo") %>' TabIndex="5"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="col-sm-4">
                                    <div class="form-group">
                                        <label>BPM Cuff Size</label>
                                        <asp:DropDownList ID="BpCuffSizeId" CssClass="form-control" runat="server" TabIndex="6"></asp:DropDownList>

                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="BluetoothModelNoL" runat="server" Text="Bluetooth Model #" AssociatedControlID="BluetoothModelNo"></asp:Label>
                                        <asp:TextBox ID="BluetoothModelNo" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.BluetoothModelNo") %>' TabIndex="7"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="MacAddrL" runat="server" Text="Mac Address" AssociatedControlID="MacAddr"></asp:Label>
                                        <asp:TextBox ID="MacAddr" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container, "DataItem.macAddr") %>' TabIndex="8"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">

                                        <label>Assigned Location</label>
                                        <asp:DropDownList ID="PurchasedBy" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label ID="EmployeeL" runat="server" Text="Assigned User" AssociatedControlID="Employee"></asp:Label>
                                        <asp:DropDownList ID="Employee" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group" style="padding-top:22px;">
                                        <asp:CheckBox ID="Active" Text="Active" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="Button1" runat="server" TabIndex="9" CssClass="btn btn-success" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'/>
                                <asp:Button ID="Button2" runat="server" TabIndex="10" CssClass="btn btn-danger" CausesValidation="false" Text='Cancel' CommandName="Cancel"/>
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <NoRecordsTemplate>
                <div>
                    There are no records to display.
                </div>
            </NoRecordsTemplate>
            </MasterTableView>
        </telerik:RadGrid>
</asp:Content>