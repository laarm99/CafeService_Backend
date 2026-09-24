using Microsoft.Extensions.DependencyInjection;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.PermissionsRequest;
using MultipleFormat.Application.Modules.TimesAdjustmentRequest;
using MultipleFormat.Application.Modules.VacationsRequest;
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
            services.AddScoped<VacationRequestService>(); 
            services.AddScoped<PermissionRequestService>();
            services.AddScoped<ChangesDepOrShiftRequestService>();
            services.AddScoped<TimeAdjustmentRequestService>();           

            return services;
        }
    }
}
