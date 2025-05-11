using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.User.UserActionResp;

namespace EvenimentMD.LogicHelper.Atributes
{
    public class IsProviderAttribute : ActionFilterAttribute
    {
        private readonly ISession _session;
        public IsProviderAttribute()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _session = bl.GetSessionBL();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionKey = HttpContext.Current.Request.Cookies["X-KEY"];

            if(sessionKey != null)
            {
                UserResp profile = _session.GetUserByCookie(sessionKey.Value);

                if (profile != null && profile.role != URole.serviceProvider) 
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller  = "Home", action = "Index"}));
                }
            }
        }
    }
}