﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Libraries.Core.Infrastructure;
using Blog.Presentation.Framework.Temporary.Razor;

namespace Blog.Presentation.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //初始化应用程序
            EngineContext.Initialize(false);

            //设置安全协议
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //禁用 "X-AspNetMvc-Version" 请求头
            MvcHandler.DisableMvcResponseHeader = true;

            //清除默认视图引擎
            ViewEngines.Engines.Clear();

            //仅适用Razor视图引擎
            ViewEngines.Engines.Add(new BaseRazorViewEngine());

            //注册路由
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            //禁用 "Server" 请求头
            app?.Context?.Response.Headers.Remove("Server");
        }

    }
}
