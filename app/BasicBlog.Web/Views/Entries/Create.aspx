<%@ Page Title="Create Entry" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<BasicBlog.ApplicationServices.ViewModels.EntryFormViewModel>" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

	<h1>Create Entry</h1>

	<% Html.RenderPartial("EntryForm", ViewData); %>

</asp:Content>
