using Microsoft.AspNetCore.Mvc;
using MultipleFormat.Application.Modules.TimesAdjustmentRequest;
using MultipleFormat.Application.Modules.TimesAdjustmentRequest.DTOs;

namespace MultipleFormat.Web.Controllers
{
    public class TimeAdjustmentRequestController : ControllerBase
    {
        public readonly TimeAdjustmentRequestService _timeAdjustmentRequestService;

        public TimeAdjustmentRequestController(TimeAdjustmentRequestService timeAdjustmentRequestService)
        {
            _timeAdjustmentRequestService = timeAdjustmentRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TimeAdjustmentCreateRequestDTO dto)
        {
            var result = await _timeAdjustmentRequestService.CreateTimeAdjustmentRequest(dto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
