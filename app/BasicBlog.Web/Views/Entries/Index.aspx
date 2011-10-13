<%@ Page Title="Entries" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<BasicBlog.Core.QueryDtos.EntryDto>>" %>
<%@ Import Namespace="BasicBlog.Core.QueryDtos" %>
<%@ Import Namespace="BasicBlog.Web.Controllers" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
 <script src="<%= ResolveUrl("~") %>Scripts/jquery.1.5.1-vsdoc.js" type="text/javascript"></script>
 <script src="<%= ResolveUrl("~") %>Scripts/date.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(document).ready(function () {
           $(".blogcreatedatetime").each(function (index) {
               var blogdate = new Date($(this).text());
               $(this).text(formatDate(blogdate, 'hh:mma NNN ddo yyyy'));
           });
       })

       function DeletePost(dataval) {
           //alert($(".blogcreatedatetime").text());
           if (confirm('Are you sure you want to delete this entry?')) {
               $.ajax({
                   type: "POST",
                   url: "/Entries/DeleteEntry",
                   data: { ID: dataval },
                   success: function (data, textStatus, jqXHR) {
                       //remove item here
                       //alert(data);
                       $("#divEntry" + dataval).hide();
                   },
                   error: function () {
                       alert('Error deleting entry');
                   }
               });
           }
           return false;
       }
       function AddNew() {
           var d = new Date();
           var datestring = formatDate(d, 'MM/dd/yyyy HH:mm:ss');
           $.ajax({
               type: "POST",
               url: "/Entries/CreateEntry",
               datatype: "json",
               data: { Content: $("#iContent").val(), PostingDateTime: datestring },
               success: function (data, textStatus, jqXHR) {
                   //$("#divEntry" + dataval).hide();
                   var retID = $.parseJSON(jqXHR.responseText).Id;
                   datestring = formatDate(d, 'hh:mma NNN ddo yyyy');
                   $("#maincontainer").prepend(MarkupToAdd(retID, $("#iContent").val(), datestring));
                   $("#iContent").val('');
               },
               error: function (data, textStatus, jqXHR) {
                   alert(jqXHR);
               }
           });
       }
       

       function MarkupToAdd(retID, blogcontent, datestring) {
           var retMarkup = "<div id=\"divEntry" + retID + "\" class=\"blogentry\" >"
            + "<div class=\"blogcontent\">" + blogcontent + "</div>"
			+ "<div class=\"blogcreatedatetime\">" + datestring + "</div>"
            + "<div class=\"blogremovebtn\">"
            + "<a href=\"\" onclick=\"DeletePost(" + retID + ");return false;\">Remove</a>"
            + "<div class=\"divider\"></div>"
            + "</div> </div>";
           return retMarkup;
       }
    </script>
 </asp:Content>
<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>ATP Blog</h1>
    <br />
    <div id="maincontainer">
        <% foreach (EntryDto entryDto in ViewData.Model) { %>
            <div id="divEntry<%= entryDto.Id %>" class="blogentry" >
                <div class="blogcontent"><%= entryDto.Content %></div>
			    <div class="blogcreatedatetime">
                    <%--<%=Html.Label(entryDto.PostingDateTime.ToString("hh:mmtt MMM dd yyyy")) %>--%>
                    <%=entryDto.PostingDateTime.ToString("MM/dd/yyyy HH:mm:ss")%>
                </div>
                <div class="blogremovebtn">
                        <%= Html.AntiForgeryToken() %>
                        <a href="" onclick="DeletePost(<%= entryDto.Id %>);return false;">Remove</a>
                        <div class="divider"></div>
                </div>

            </div>
		    <%} 
        %>
    </div>

    <%--<p><%= Html.ActionLink<EntriesController>(c => c.Create(), "Create New Entry") %></p>--%>
    <div id="createnew">
        <%= Html.AntiForgeryToken() %>
        <%--<input type="text" name="content" id="iContent" /><br />--%>
        <textarea rows="5" cols="100" id="iContent"></textarea>
        <div id="AddNewBtn">
            <input type="submit" value="Add New" onclick="AddNew();" />
        </div>
    </div>

</asp:Content>
