﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipReassign.ascx.cs" Inherits="HeartValve.Inventory.EquipReassign" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div style="padding:30px;">
    <p>Reassign</p>
    <div class="row">
        <div class="col-sm-4">
            <div class="form-group">
                <asp:Label ID="ReturnReasonL" AssociatedControlID="ReturnReason" runat="server" Text="Return Reason"></asp:Label>
                <asp:DropDownList runat="server" CssClass="form-control" ID="ReturnReason" OnDataBinding="ReturnReason_DataBinding"></asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <asp:Label ID="EmployeeL" AssociatedControlID="Employee" runat="server" Text="User"></asp:Label>
                <asp:DropDownList runat="server" ID="Employee" CssClass="form-control" OnDataBinding="Employee_DataBinding"></asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group">
                <asp:Label ID="CommentsL" AssociatedControlID="Comments" runat="server" Text="Comments"></asp:Label>
                <asp:TextBox ID="Comments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="form-group">
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-default" Text='Reassign' CommandName="Update"/>
            <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" CausesValidation="false" Text='Cancel' CommandName="Cancel"/>
        </div>
    </div>
</div>