using NewProject.Application.Modules.Dashboard.DTOs;
using NewProject.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewProject.Application.Modules.Dashboard
{
    public class DashboardService
    {
        public ServiceResult<List<DashboardGridDTO>> GetGrid()
        {            
            var data = new List<DashboardGridDTO>
            {
                new()
                {
                    Id = 1,
                    Name = "Name 1",
                },
                new()
                {
                    Id= 2,
                    Name= "Name 2"
                }
            };            

            return ServiceResult<List<DashboardGridDTO>>.Success(data);
        }
    }     
}
