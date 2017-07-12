<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="DDI.UI.Web.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../../Scripts/search.js"></script>
    <link rel="stylesheet" href="..\..\CSS\search.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="UtilityMenuContainer" runat="server">

    <li><a href="#" class="addconstituent">Add Constituent</a></li> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="searchcriteria">
        
        <div class="scrollable">
            <fieldset>
                <legend>Quick Search</legend>

                <div class="fieldblock">
                    <input type="text" class="searchquicksearch" />
                </div>

            </fieldset>

            <fieldset>
                <legend>Advanced Search</legend>
                <div class="accordions nocontrols">

                    <h1>CRM</h1>
                    <div>
                        <div class="fieldblock">
                            <label>ID</label>
                            <input type="number" class="searchconstituentnumber" maxlength="64" />
                        </div>

                        <div class="fieldblock">
                            <label>Constituent Type</label>
                            <select class="searchtype"></select>
                        </div>

                        <div class="fieldblock">
                            <label>Name</label>
                            <input type="text" class="searchname" maxlength="128" />
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
                            <input type="text" class="searchcity" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Zip From</label>
                            <input type="text" class="searchzipfrom" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Zip To</label>
                            <input type="text" class="searchzipto" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 1</label>
                            <input type="text" class="searchregion1" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 2</label>
                            <input type="text" class="searchregion2" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 3</label>
                            <input type="text" class="searchregion3" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Region 4</label>
                            <input type="text" class="searchregion4" maxlength="128" />
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
                            <input type="text" class="searchincludetags" />
                        </div>

                        <div class="fieldblock">
                            <label>Exclude Tags</label>
                            <input type="text" class="searchexcludetags" />
                        </div>

                        <div class="fieldblock">
                            <label>Alternate Id</label>
                            <input type="text" class="searchaltid" maxlength="128" />
                        </div>

                        <div class="fieldblock">
                            <label>Created From</label>
                            <input type="text" class="searchcreatedfrom datepicker" maxlength="10" />
                        </div>

                        <div class="fieldblock">
                            <label>Created To</label>
                            <input type="text" class="searchcreatedto datepicker" maxlength="10" />
                        </div>
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
