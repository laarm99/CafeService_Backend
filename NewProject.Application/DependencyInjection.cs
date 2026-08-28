using Microsoft.Extensions.DependencyInjection;
using NewProject.Application.Modules.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(

       this IServiceCollection services)
        {
            services.AddScoped<DashboardService>();

            return services;
        }
    }
}
