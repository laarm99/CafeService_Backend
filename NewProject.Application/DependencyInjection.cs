using Microsoft.Extensions.DependencyInjection;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.EmployeeInfo;
using MultipleFormat.Application.Modules.Employees;
using MultipleFormat.Application.Modules.PermissionsRequest;
using MultipleFormat.Application.Modules.TimesAdjustmentRequest;
using MultipleFormat.Application.Modules.VacationsRequest;
using NewProject.Application.Common.CurrentUser;
using NewProject.Application.Modules.Dashboard;
using MultipleFormat.Application.Modules.FormRequests;
using MultipleFormat.Application.Modules.Supervisors;
using NewProject.Application.Modules.Users;
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
            services.AddSingleton(sp => new NewProject.Application.Common.Storage.AttachmentStorage(
                sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>()?["Attachments:RootPath"]));
            services.AddScoped<DashboardService>();
            services.AddScoped<VacationRequestService>(); 
            services.AddScoped<PermissionRequestService>();
            services.AddScoped<ChangesDepOrShiftRequestService>();
            services.AddScoped<TimeAdjustmentRequestService>();           
            services.AddScoped<EmployeeInfoService>();
            services.AddScoped<FormRequestService>();
            services.AddScoped<FormRequestApprovalService>();
            services.AddScoped<FormRequestAdminService>();
            services.AddScoped<NewProject.Application.Common.Email.IEmailService, NewProject.Application.Common.Email.LoggingEmailService>();
            services.AddScoped<SupervisorService>();
            services.AddScoped<UserService>();
            services.AddScoped<EmployeeSupervisorService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
