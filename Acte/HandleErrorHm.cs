using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace ActeAuto
{
    /// <seealso cref="http://weblogs.asp.net/awilinsk/archive/2008/12/11/handleerrorattribute-and-health-monitoring.aspx"/>
    public class HandleErrorHmAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            new WebRequestErrorEventMvc("An unhandled exception has occurred.", this, 103005, context.Exception).Raise();
        }
    }

    public class WebRequestErrorEventMvc : WebRequestErrorEvent
    {
        public WebRequestErrorEventMvc(string message, object eventSource, int eventCode, Exception exception) : base(message, eventSource, eventCode, exception) { }
        public WebRequestErrorEventMvc(string message, object eventSource, int eventCode, int eventDetailCode, Exception exception) : base(message, eventSource, eventCode, eventDetailCode, exception) { }
    }
}