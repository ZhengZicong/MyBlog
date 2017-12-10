using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Libraries.Core.Caching;
using Blog.Libraries.Core.Configuration;
using Blog.Libraries.Core.Context;
using Blog.Libraries.Data.Domain.Member;

namespace Blog.Libraries.Services.Permissions
{

    /// <summary>
    /// 仅限使用于限制管理员(Admin)的权限
    /// </summary>
    public class RoleService : IRoleService
    {

        #region fields

        private readonly IWorkContext _workContext;
        private readonly HttpContextBase _httpContextBase;
        private readonly ICacheManager _cacheManager;
        private readonly WebConfig _webConfig;

        private const string AdminRoleCacheKey = "ransurotto.com.role.admin_{0}";
        private const string GroupRoleCacheKey = "ransurotto.com.role.group_{0}";

        #endregion

        #region Constructor

        public RoleService(IWorkContext workContext, HttpContextBase httpContextBase, ICacheManager cacheManager, WebConfig webConfig)
        {
            _workContext = workContext;
            _httpContextBase = httpContextBase;
            _cacheManager = cacheManager;
            _webConfig = webConfig;
        }

        #endregion

        #region Methods

        public bool Authorize()
        {
            var routeDataValues = _httpContextBase.Request.RequestContext.RouteData.Values;

            var areaName = routeDataValues["Area"].ToString();
            var controllerName = routeDataValues["Controller"].ToString();
            var actionName = routeDataValues["Action"].ToString();

            return Authorize(areaName, controllerName, actionName);
        }

        public bool Authorize(string area, string controllerName, string actionName)
        {
            var admin = _workContext.Admin as Admin;
            return Authorize(area, controllerName, actionName, admin);
        }

        public bool Authorize(string area, string controllerName, string actionName, Admin admin)
        {
            if (admin == null)
                return false;

            return Authorize(area, controllerName, actionName, GetRoleStringCache(admin, "admin"))
                   || Authorize(area, controllerName, actionName, GetRoleStringCache(admin, "group"));
        }

        public bool Authorize(string area, string controllerName, string actionName, string roleString)
        {
            /*调试模式不验证权限*/
            if (_webConfig.Debug)
                return true;

            var routeData = new Dictionary<string, object>
            {
                {"area", area},
                {"controller", controllerName},
                {"action", actionName}
            };

            var virtualPathData = RouteTable.Routes.GetVirtualPathForArea(_httpContextBase.Request.RequestContext, new RouteValueDictionary(routeData));
            // ASP.NET MVC5 ：System.Web.Mvc.Routing.RouteDataTokenKeys const Namespaces = "Namespaces"
            // ASP.NET MVC Source Code: https://github.com/aspnet/AspNetWebStack
            var controllerNamespaces = virtualPathData.DataTokens["Namespaces"] as string[];
            if (controllerNamespaces == null)
                throw new Exception("未找到对应路由绑定的控制器命名空间");

            for (int i = 0; i < controllerNamespaces.Length; i++)
            {
                var controllerClassName = string.Format($"{controllerNamespaces[i]}.{controllerName}Controller");
                var controllerClassType = Type.GetType(controllerClassName);
            }

            return false;
        }

        #endregion

        #region Utilities

        private string GetRoleStringCache(Admin admin, string roleStringType)
        {
            var cacheTime = (int)(DateTime.Now.AddDays(7) - DateTime.Now).TotalMinutes;
            switch (roleStringType)
            {
                case "admin":
                    return _cacheManager.Get(GetAdminRoleCacheKeyByAdmin(admin), cacheTime, () => admin.UserRole?.RoleString);
                case "group":
                    return _cacheManager.Get(GetGroupRoleCacheKeyByAdmin(admin), cacheTime, () => admin.UserRole?.RoleGroup?.RoleString);
                default:
                    throw new ArgumentException("不支持的roleStringType", "roleStringType");
            }
        }

        private string GetAdminRoleCacheKeyByAdmin(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException("admin");
            return string.Format(AdminRoleCacheKey, admin.Id);
        }

        private string GetGroupRoleCacheKeyByAdmin(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException("admin");
            return string.Format(GroupRoleCacheKey, admin.UserRole?.RoleGroup?.Id);
        }

        #endregion

    }

}
