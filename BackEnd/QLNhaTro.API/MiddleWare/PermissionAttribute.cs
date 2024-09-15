using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QLNhaTro.Commons;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.API.MiddleWare
{
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly FeatureCode[] requiredPermissions;
        public PermissionAttribute(FeatureCode permission = FeatureCode.Commons) 
        {
            requiredPermissions = new FeatureCode[] { permission };
        }
        public PermissionAttribute(params FeatureCode[] permission)
        {
            requiredPermissions = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var providedPermissions = (List<FeatureCode>)context.HttpContext.Items[CommonConstants.JWT.Permission];
                if (!providedPermissions.Any(p=>requiredPermissions.Contains(p)))
                {
                    context.Result = new JsonResult(new { message = "Không có quyền" })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }
            catch (Exception )
            {
                context.Result = new JsonResult(new { message = "Không có quyền" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            } 
        }
    }
}
