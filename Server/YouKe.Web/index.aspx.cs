using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YouKe.Web
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Header.Title = "00";
            Response.AddHeader("cmdId","6001");
            Response.AddHeader("user_id", "1");


            NameValueCollection header = Request.Headers;

            //Request.Headers;

            string[] sss = header.GetValues("Connection");

            foreach(var s in sss)
            {
                Response.Write(s + "<br />");
            }
            Response.Write(sss.Length + "<br /><br /><br />");

            string[] headerArr = Request.Headers.AllKeys;
            header.GetValues("cmdId");
            foreach(var s in header.AllKeys)
            {
                Response.Write(s+"<br />");
            }

        }
    }
}