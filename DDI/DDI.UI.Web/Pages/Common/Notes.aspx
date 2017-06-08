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

    <div class="notesdetailmodal" title="Notes Detail" style="display: none;">

        <div class="modalcontent">
            <input type="hidden" class="nd-hidparententityid" />
            <input type="hidden" class="hidentitytype" />

            <div class="fieldblock">
                <label>Title</label>
                <input type="text" class="nd-Title" />
            </div>

            <div class="fieldblock">
                <label>Description</label>
                <textarea rows ="4" cols ="86" class="nd-Description"></textarea>
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Alert Start Date</label>
                    <input type="text" class="nd-AlertStartDate datepicker" />
                </div>

                <div class="fieldblock">
                    <label>Alert End Date</label>
                    <input type="text" class="nd-AlertEndDate datepicker" />
                </div>

            </div>

            <div class="fieldblock">
                <label>Note Topics</label>
                <div class="nd-Topics noteTopicSelect"></div> 
                <div class="noteTopicSelectImage" style="display: inline-block;"></div>
                <div class="tagdropdowncontainer" style="display:none;">
                    <div class="tagdropdowndivcontainer"></div>
                    <div class="modalbuttons">
                        <input type="button" class="savenotetopics" value="Save" />
                        <a href="#" class="cancelnotetopics">Cancel</a>
                    </div>
                </div>
            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Note Code</label>
                    <select class="nd-NoteCode"></select>
                </div>

                <div class="fieldblock">
                    <label>Note Category</label>
                    <select class="nd-Category Id"></select>
                </div>

            </div>

             <div class="twocolumn">

                <div class="fieldblock">
                    <label>Contact Date</label>
                    <input type="text" class="nd-ContactDate datepicker" />
                </div>

                 <div class="fieldblock">
                    <label>Primary Contact</label>
                    <select class="nd-PrimaryContact"></select>
                </div>

            </div>

            <div class="twocolumn">

                <div class="fieldblock">
                    <label>Person Responsible</label>
                    <select class="nd-PersonResponsible"></select>
                </div>

                <div class="fieldblock">
                    <label>Contact Method</label>
                    <select class="nd-ContactMethod"></select>
                </div>

            </div>

            <div class="editnoteinfo" style="display: none;">
                <div class="twocolumn">

                    <div class="fieldblock">
                        <label>Created By</label>
                        <label class="nd-CreatedBy"></label>
                    </div>

                    <div class="fieldblock">
                        <label>Updated By</label>
                        <label class="nd-UpdatedBy"></label>
                    </div>

                </div>

                 <div class="twocolumn">

                    <div class="fieldblock">
                        <label>On</label>
                        <label class="nd-CreatedOn"></label>
                    </div>

                    <div class="fieldblock">
                        <label>On</label>
                        <label class="nd-UpdatedOn"></label>
                    </div>

                </div>
            </div>

            <div class="modalbuttons">
                <input type="button" class="savenotedetails" value="Save" />
                <a href="#" class="cancelnotesmodal">Cancel</a>
            </div>

        </div>

    </div>

</asp:Content>
