<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MembershipManage.aspx.cs" Inherits="Feng.Security.WebService.MembershipManage"
    Title="用户管理" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <div >
            <table style="font-weight: normal; font-size: 12px; font-family: Arial" border="0" cellpadding=1 cellspacing=2 background-color="white" width="100%">
                <tr bgcolor="#ffffff">
                    <td align="center" >
                        用户列表<br /><br />
                        <asp:GridView ID="GridViewMemberUser" runat="server" OnSelectedIndexChanged="GridViewMembershipUser_SelectedIndexChanged"
                            OnRowDeleted="GridViewMembership_RowDeleted" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="UserName" DataSourceID="ObjectDataSourceMembershipUser" AllowSorting="True" Font-Size="X-Small" Width="95%">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
                                <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="True" SortExpression="UserName" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                
                                <asp:BoundField DataField="PasswordQuestion" HeaderText="密码提示问题" ReadOnly="True"
                                    SortExpression="PasswordQuestion" />
  
                                <asp:BoundField DataField="Comment" HeaderText="备注" SortExpression="Comment" />
                                
                                <asp:BoundField DataField="CreationDate" HeaderText="注册时间" ReadOnly="True"
                                    SortExpression="CreationDate" />
                                <asp:CheckBoxField DataField="IsApproved" HeaderText="是否批准" SortExpression="IsApproved" />
                                <asp:BoundField DataField="LastLockoutDate" Visible="False" HeaderText="上次锁定时间" ReadOnly="True"
                                    SortExpression="LastLockoutDate" />
                                <asp:BoundField DataField="LastLoginDate" HeaderText="上次登录时间" SortExpression="LastLoginDate" />
                                <asp:CheckBoxField DataField="IsOnline" Visible="False" HeaderText="是否在线" ReadOnly="True" SortExpression="IsOnline" />
                                <asp:CheckBoxField DataField="IsLockedOut" HeaderText="是否锁定" ReadOnly="True"
                                    SortExpression="IsLockedOut" Visible="False" />
                                <asp:BoundField DataField="LastActivityDate" HeaderText="上次活动时间" SortExpression="LastActivityDate" Visible="False" />
                                <asp:BoundField DataField="LastPasswordChangedDate" HeaderText="上次修改密码时间" Visible="False"
                                    ReadOnly="True" SortExpression="LastPasswordChangedDate" />
                                
                                <asp:BoundField DataField="ProviderName" HeaderText="用户管理提供者" ReadOnly="True" Visible="False"
                                    SortExpression="ProviderName" />
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="ObjectDataSourceMembershipUser" runat="server" DeleteMethod="Delete"
                            InsertMethod="Insert"  SelectMethod="GetMembers"
                            TypeName="MembershipUtilities.MembershipUserODS" UpdateMethod="Update"
                            SortParameterName="SortData" OnInserted="ObjectDataSourceMembershipUser_Inserted" >
                            <DeleteParameters>
                                <asp:Parameter Name="UserName" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="isApproved" Type="Boolean" />
                                <asp:Parameter Name="comment" Type="String" />
                                <asp:Parameter Name="lastActivityDate" Type="DateTime" />
                                <asp:Parameter Name="lastLoginDate" Type="DateTime" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:Parameter Name="sortData" Type="String" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="userName" Type="String" />
                                <asp:Parameter Name="isApproved" Type="Boolean" />
                                <asp:Parameter Name="comment" Type="String" />
                                <asp:Parameter Name="lastLockoutDate" Type="DateTime" />
                                <asp:Parameter Name="creationDate" Type="DateTime" />
                                <asp:Parameter Name="email" Type="String" />
                                <asp:Parameter Name="lastActivityDate" Type="DateTime" />
                                <asp:Parameter Name="providerName" Type="String" />
                                <asp:Parameter Name="isLockedOut" Type="Boolean" />
                                <asp:Parameter Name="lastLoginDate" Type="DateTime" />
                                <asp:Parameter Name="isOnline" Type="Boolean" />
                                <asp:Parameter Name="passwordQuestion" Type="String" />
                                <asp:Parameter Name="lastPasswordChangedDate" Type="DateTime" />
                                <asp:Parameter Name="password" Type="String" />
                                <asp:Parameter Name="passwordAnswer" Type="String" />
                            </InsertParameters>
                        </asp:ObjectDataSource><br /><br />
                    </td>
                </tr>
            </table>
            <table style="font-weight: normal; font-size: 12px; font-family: Arial" background-color="white" width="100%" border=0 cellpadding=1 cellspacing=2>
                <tr valign="top" bgcolor="#ffffff">
                    <td  align="center" style="width: 44%">
                    <b></b><strong>角色管理</strong><br />
                         <asp:GridView ID="GridViewRole" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSourceRoleObject"
                            DataKeyNames="RoleName" CellPadding="3" CellSpacing="3" HorizontalAlign="Center" Width="100%">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="false" Width="250px" OnClick="ToggleInRole_Click"
                                            Text='<%# ShowInRoleStatus( (string) Eval("UserName"),(string) Eval("RoleName")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        用户角色状态
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NumberOfUsersInRole" HeaderText="用户数"
                                    SortExpression="NumberOfUsersInRole" />
                                
                                <asp:BoundField DataField="RoleName" ReadOnly="True" Visible="False" HeaderText="角色名"
                                    SortExpression="RoleName" />
                                <asp:CheckBoxField DataField="UserInRole" HeaderText="UserInRole" Visible="False"
                                    SortExpression="UserInRole" />
                            </Columns>
                        </asp:GridView>
                        <asp:CheckBox ID="CheckBoxShowRolesAssigned" runat="server" AutoPostBack="True" Text="只显示已分配给用户的角色" />
                    </td>
                    <td  align="center"  width=30%>
                    <b></b><strong>建立新角色</strong><br />
                        <asp:TextBox ID="TextBoxCreateNewRole" runat="server"></asp:TextBox><br /><br />
                        <asp:Button ID="ButtonCreateNewRole" runat="server" OnClick="ButtonCreateNewRole_Click"
                            Text="建立新角色" /><br />
                    </td>
                    <td  align="center" width=40%>
                    <b></b><strong>建立新用户</strong><br /><br />
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td style="height: 28px">
                            <asp:Label ID="Label3" Text="用户名" runat="server"></asp:Label>
                        </td>
                        <td style="height: 28px">
                            <asp:TextBox ID="TextBoxUserName" runat="server"></asp:TextBox>
                        </td>
                        </tr><tr>
                        <td style="height: 28px">
                            <asp:Label ID="Label4" Text="密码" runat="server"></asp:Label>
                        </td>
                        <td style="height: 28px">
                            <asp:TextBox ID="TextBoxPassword" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" Text="密码提示问题" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxPasswordQuestion" runat="server"></asp:TextBox>
                        </td>
                     </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label6" Text="密码提示问题答案" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxPasswordAnswer" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Email" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
                        </td>
                        </tr><tr>
                        <td>
                            <asp:Label ID="Label9" Text="是否批准" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckboxApproval" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonNewUser" runat="server" Text="建立新用户" OnClick="ButtonNewUser_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="LabelInsertMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="font-weight: normal; font-size: 12px; font-family: Arial" border="0" cellpadding=1 cellspacing=2 background-color="white" width="100%">
                <tr bgcolor="#ffffff">
                    <td align="center" >
                    <b></b><br /><br />
                    </td>
                </tr>
            </table>
            <br />
             
            <asp:ObjectDataSource ID="ObjectDataSourceRoleObject" runat="server" SelectMethod="GetRoles"
                TypeName="MembershipUtilities.RoleDataObject" InsertMethod="Insert" DeleteMethod="Delete"  >
                <SelectParameters>
                    <asp:ControlParameter ControlID="GridViewMemberUser" Name="UserName" PropertyName="SelectedValue"
                        Type="String" />
                    <asp:ControlParameter ControlID="CheckBoxShowRolesAssigned" Name="ShowOnlyAssignedRolls"
                        PropertyName="Checked" Type="Boolean" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="RoleName" Type="String" />
                </InsertParameters>
                <DeleteParameters>
                    <asp:Parameter Name="RoleName" Type="String" />
                </DeleteParameters>
            </asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
