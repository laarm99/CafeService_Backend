using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.PermissionsRequest;
using MultipleFormat.Application.Modules.PermissionsRequest.DTOs;

namespace MultipleFormat.Web.Controllers
{
    [ApiController]
    [Route("api/permission-request")]
    public class PermissionRequestController : ControllerBase
    {
        private readonly PermissionRequestService _permissionRequestService;

        public PermissionRequestController(PermissionRequestService permissionRequestService)
        {
            _permissionRequestService = permissionRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionCreateRequestDTO dto)
        {
            var result = await _permissionRequestService.CreatePermissionRequest(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
