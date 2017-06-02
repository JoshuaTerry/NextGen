<%@ Page Title="" Language="C#" MasterPageFile="~/TabbedContent.Master" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="DDI.UI.Web.Pages.Common.Notes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/Notes.js"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="tab-notes">

        <div class="tabscontainer inner">

            <ul> 
                <li><a href="#tab-notedetail">Note Detail</a></li>
                <li><a href="#tab-attachments">Attachments</a></li>
            </ul>

            <div id="tab-notedetail">

                <h1>Note Detail<a href="#" title="New" class="newnotesdetailmodallink newbutton"></a></h1>
                <div class="notedetailsgridcontainer"></div>
                        
            </div>

            <div id="tab-attachments">

                <table class="datagrid attachmentstable">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th>File Name</th>
                            <th>Private</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>

            </div>
                    
        </div>

    </div>

</asp:Content>
