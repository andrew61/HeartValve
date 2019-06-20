<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/InventoryViews/InventoryPages.Master" CodeBehind="AssignEquip.aspx.cs" Inherits="HeartValve.Inventory.AssignEquip" %>
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
    <h1>Assign Equipment</h1>
    <div class="panel panel-default">
        <div class="panel-heading">Filters</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-4">
                    <asp:CheckBoxList ID="AssignmentFilter" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="SelectedIndexChanged">
                        <asp:ListItem Selected="True" Text="Assigned Equipment" Value="1"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="Unassigned Equipment" Value="2"></asp:ListItem>
                    </asp:CheckBoxList>
                </div>
                <div class="col-sm-4">
                    <asp:Label ID="EquipTypeL" runat="server" AssociatedControlID="EquipType" Text="Equipment Type"></asp:Label>
                    <asp:DropDownList ID="EquipType" runat="server" CssClass="form-control" AutoPostBack="true" OnDataBinding="EquipType_DataBinding" OnDataBound="EquipType_DataBound" OnSelectedIndexChanged="SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-sm-4">
                    <asp:Label ID="EmployeeL" runat="server" AssociatedControlID="Employee" Text="User"></asp:Label>
                    <asp:DropDownList ID="Employee" runat="server" CssClass="form-control" AutoPostBack="true" OnDataBinding="Employee_DataBinding" OnDataBound="Employee_DataBound" OnSelectedIndexChanged="SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadGrid ID="AssignmentTable" EnableViewState="true" runat="server" AutoGenerateColumns="false" GridLines="None" AllowPaging="true"
        PageSize="20" AllowSorting="true" ShowStatusBar="true" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
        OnNeedDataSource="AssignmentTable_NeedDataSource" OnPreRender="AssignmentTable_PreRender" OnUpdateCommand="AssignmentTable_UpdateCommand"
        MasterTableView-EditMode="EditForms" Skin="Silk" OnItemDataBound="AssignmentTable_ItemDataBound" OnItemCommand="AssignmentTable_ItemCommand">
            <MasterTableView Width="100%" UseAllDataFields="true" DataKeyNames="InventoryID,UserID,UserEquipmentID" ShowHeader="true">
                <Columns>
                    <%--<telerik:GridBoundColumn UniqueName="inventoryID" DataField="inventoryID" HeaderText="ID"></telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn UniqueName="inventoryDesc" DataField="inventoryDesc" HeaderText="Equipment"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Manufacturer" DataField="Manufacturer" HeaderText="Manufacturer"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="ModelNo" DataField="ModelNo" HeaderText="ModelNo"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="SerialNo" DataField="SerialNo" HeaderText="SerialNo"></telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="name" DataField="name" HeaderText="User"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="dateGiven" DataField="dateGiven" HeaderText="Date Assigned"></telerik:GridBoundColumn>
<%--                <telerik:GridBoundColumn UniqueName="dateReturned" DataField="dateReturned" HeaderText="Date Reclaimed"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="reasonDescription" DataField="reasonDescription" HeaderText=""></telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn UniqueName="comments" DataField="comments" HeaderText="Comments"></telerik:GridBoundColumn>
                    <%--<telerik:GridEditCommandColumn UniqueName="EditCommandcolumn" ButtonType="LinkButton" EditText="Reclaim"><ItemStyle CssClass="btn btn-primary btn-no-link" /></telerik:GridEditCommandColumn>--%>
                    <telerik:GridButtonColumn UniqueName="Reclaim" Text="Reclaim" CommandName="Reclaim" ButtonCssClass="btn btn-danger btn-no-link"></telerik:GridButtonColumn>
                    <telerik:GridButtonColumn UniqueName="Reassign" Text="Reassign" CommandName="Reassign" ButtonCssClass="btn btn-success btn-no-link"></telerik:GridButtonColumn>
                    <telerik:GridButtonColumn UniqueName="Assign" Text="Assign" CommandName="Assign" ButtonCssClass="btn btn-info btn-no-link"></telerik:GridButtonColumn>
                </Columns>
                <%--<EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <span>Hallo</span>
                    </FormTemplate>
                </EditFormSettings>--%>
                <NoRecordsTemplate>
                <div>
                    There are no records to display.
                </div>
            </NoRecordsTemplate>
            </MasterTableView>
        </telerik:RadGrid>
</asp:Content>