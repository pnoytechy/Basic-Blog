<%@ Page Title="Entry Details" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<BasicBlog.Core.Entry>" %>
<%@ Import Namespace="BasicBlog.Web.Controllers" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>Entry Details</h1>

    <ul>
		<li>
			<label for="Entry_Content">Content:</label>
            <span id="Entry_Content"><%= Server.HtmlEncode(ViewData.Model.Content.ToString()) %></span>
		</li>
		<li>
			<label for="Entry_PostingDateTime">PostingDateTime:</label>
            <span id="Entry_PostingDateTime"><%= Server.HtmlEncode(ViewData.Model.PostingDateTime.ToString()) %></span>
		</li>
	    <li class="buttons">
            <%= Html.Button("btnBack", "Back", HtmlButtonType.Button, 
                "window.location.href = '" + Html.BuildUrlFromExpressionForAreas<EntriesController>(c => c.Index()) + "';") %>
        </li>
	</ul>

</asp:Content>
