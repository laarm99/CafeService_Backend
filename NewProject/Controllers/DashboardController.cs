using Microsoft.AspNetCore.Mvc;
using NewProject.Application.Modules.Dashboard;
using NewProject.Common.Results;
using NewProject.Web.Extensions;

namespace NewProject.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("grid")]
        public IActionResult GetGrid()
        {
            var res = _dashboardService.GetGrid();

            return this.FromServiceResult(res);
        }

    }
}
