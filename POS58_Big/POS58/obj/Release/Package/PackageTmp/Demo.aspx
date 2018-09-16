<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="POS58.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 800px;
            height: 600px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: 3em">
        <img alt="" class="auto-style1" src="Images/sausage.jpg" width="100%" /><br />
        30 NT <br /><br />數量<asp:DropDownList ID="ddlQty" runat="server" Height="100px" Width="30%" Font-Size="1.5em">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
        </asp:DropDownList><br /><br />
        <asp:Button ID="Button1" runat="server" Text="購買" Font-Size="1.5em" Height="100px" OnClick="Button1_Click" Width="50%" />
    </div>
    </form>
</body>
</html>
