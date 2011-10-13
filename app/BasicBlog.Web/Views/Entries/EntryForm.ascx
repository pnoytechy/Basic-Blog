<%@ Control Language="C#" AutoEventWireup="true"
	Inherits="System.Web.Mvc.ViewUserControl<BasicBlog.ApplicationServices.ViewModels.EntryFormViewModel>" %>
<%@ Import Namespace="BasicBlog.Core" %>
<%@ Import Namespace="BasicBlog.Web.Controllers" %>
 

<% if (ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] != null) { %>
    <p id="pageMessage"><%= ViewContext.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()]%></p>
<% } %>

<%= Html.ValidationSummary() %>

<% using (Html.BeginForm()) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Entry.Id", (ViewData.Model.Entry != null) ? ViewData.Model.Entry.Id : 0)%>

    <ul>
		<li>
			<label for="Entry_Content">Content:</label>
			<div>
				<%= Html.TextBox("Entry.Content", 
					(ViewData.Model.Entry != null) ? ViewData.Model.Entry.Content.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Entry.Content")%>
		</li>
		<li>
			<label for="Entry_PostingDateTime">PostingDateTime:</label>
			<div>
				<%= Html.TextBox("Entry.PostingDateTime", 
					(ViewData.Model.Entry != null) ? ViewData.Model.Entry.PostingDateTime.ToString() : "")%>
			</div>
			<%= Html.ValidationMessage("Entry.PostingDateTime")%>
		</li>
	    <li>
            <%= Html.SubmitButton("btnSave", "Save Entry") %>
	        <%= Html.Button("btnCancel", "Cancel", HtmlButtonType.Button, 
				    "window.location.href = '" + Html.BuildUrlFromExpressionForAreas<EntriesController>(c => c.Index()) + "';") %>
        </li>
    </ul>
<% } %>
