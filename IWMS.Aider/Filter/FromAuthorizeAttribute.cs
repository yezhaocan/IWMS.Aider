using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWMS.Aider.Filter
{
    public class FromAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public FromAuthorizeAttribute()
        {
        }
        public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            string urlReferer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
            if (urlReferer != "http://218.18.232.141:8088/wmsweb/Web/Query/UniCodeQuery.aspx?dirid=WS201700"
                && urlReferer!= "http://218.18.232.141:8088/iwms_aider"
                && urlReferer != "http://218.18.232.141:8088/iwms_aider/home/index")
            {
                filterContext.Result = new RedirectResult("/Home/Error");
            }
            return;
        }
        
    }
}
