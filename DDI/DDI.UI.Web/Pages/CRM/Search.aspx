<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="DDI.UI.Web.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/search.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="searchcriteria">
        
        <div class="scrollable">
            <fieldset>
                <legend>Quick Search</legend>

                <div class="fieldblock">
                    <input type="text" class="quicksearch" />
                </div>

            </fieldset>

            <fieldset>
                <legend>Advanced Search</legend>

                

                <div class="accordions nocontrols">

                    <h1>CRM</h1>
                    <div>
                        <div class="fieldblock">
                            <label>ID</label>
                            <input type="number" class="searchconstituentnumber" />
                        </div>

                        <div class="fieldblock">
                            <label>Constituent Type</label>
                            <select class="searchtype"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Name</label>
                            <input type="text" class="searchname" />
                        </div>

                        <div class="fieldblock">
                            <label>Country</label>
                            <select class="searchcountry"></select>
                        </div>

                        <div class="fieldblock">
                            <label>State</label>
                            <select class="searchstate"></select>
                        </div>

                        <div class="fieldblock">
                            <label>City</label>
                            <input type="text" class="searchcity" />
                        </div>

                        <div class="fieldblock">
                            <label>Zip From</label>
                            <input type="number" class="searchzipfrom" />
                        </div>

                        <div class="fieldblock">
                            <label>Zip To</label>
                            <input type="number" class="searchzipto" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 1</label>
                            <input type="number" class="searchregion1" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 2</label>
                            <input type="number" class="searchregion2" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 3</label>
                            <input type="number" class="searchregion3" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 4</label>
                            <input type="number" class="searchregion4" />
                        </div>

                        <div class="fieldblock">
                            <label>Age From</label>
                            <input type="number" class="searchagefrom" />
                        </div>

                        <div class="fieldblock">
                            <label>Age To</label>
                            <input type="number" class="searchageto" />
                        </div>

                        <div class="fieldblock">
                            <label>Include Tags</label>
                            <input type="number" class="searchincludetags" />
                        </div>

                        <div class="fieldblock">
                            <label>Exclude Tags</label>
                            <input type="number" class="searchexcludetags" />
                        </div>

                        <div class="fieldblock">
                            <label>Alternate Id</label>
                            <input type="number" class="searchaltid" />
                        </div>

                        <div class="fieldblock">
                            <label>Created From</label>
                            <input type="number" class="searchcreatedfrom datepicker" />
                        </div>

                        <div class="fieldblock">
                            <label>Created To</label>
                            <input type="number" class="searchcreatedto datepicker" />
                        </div>
                    </div>

                    <h1>Donations</h1>
                    <div>

                    </div>

                </div>
                

            </fieldset>
        </div>

        <div class="buttons">
            <input type="button" class="clearsearch" value="Clear" />
            <input type="button" class="dosearch" value="Search" />
        </div>

    </div>

    <div class="searchresults">
        <div class="gridcontainer scrollable"></div>
    </div>

</asp:Content>
