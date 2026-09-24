using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest;
using MultipleFormat.Application.Modules.ChangesDepOrShifRequest.DTOs;
using NewProject.Infrastructure.Persistence;

namespace NewProject.Controllers
{
    [ApiController]
    [Route("api/changes-department-or-shift")]
    public class DepartmentShiftChangeRequestController : ControllerBase
    {
        private readonly ChangesDepOrShiftRequestService _changesDepOrShiftRequestService;

        public DepartmentShiftChangeRequestController(ChangesDepOrShiftRequestService changesDepOrShiftRequestService)
        {
            _changesDepOrShiftRequestService = changesDepOrShiftRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChangesDepOrShiftCreateRequestDTO dto)
        {
            var result = await _changesDepOrShiftRequestService.CreateChangesDepOrShiftRequests(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
