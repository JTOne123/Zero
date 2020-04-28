﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero.NETCore.Inject;

namespace Zero.NETCore.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TimerAttribute : ActionFilterAttribute
    {
        private readonly int _timeOutSeconds = 0;
        public TimerAttribute(int timeOutSeconds = 2000)
        {
            this._timeOutSeconds = timeOutSeconds;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ticks = Environment.TickCount;

            await next();

            var time = Environment.TickCount - ticks;

            if (time > _timeOutSeconds)
            {
                var controllerName = context.RouteData.Values["Controller"].ToString();

                var actionName = context.RouteData.Values["Action"].ToString();

                var message = string.Format("Controller:[{0}] Action:[{1}],本次请求耗时 {2} 秒.", controllerName, actionName, (double)time / 1000);

                new LogClient().WriteCustom(message, "TimeOut");
            }
        }
    }
}
