using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.VacationsRequest;
using MultipleFormat.Application.Modules.VacationsRequest.DTOs;

namespace MultipleFormat.Web.Controllers
{
    [ApiController]
    [Route("api/vacation-request")]
    public class VacationRequestController : ControllerBase
    {
        private readonly VacationRequestService _vacationRequestService;
        public VacationRequestController(VacationRequestService vacationRequestService)
        {
            _vacationRequestService = vacationRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacationCreateRequestDTO dto)
        {
            var result = await _vacationRequestService.CreateVacationRequest(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
