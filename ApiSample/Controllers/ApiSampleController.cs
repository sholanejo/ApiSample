using ApiSample.Application.Features.Commands;
using ApiSample.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ApiSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSampleController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<ApiSampleController> _logger;
        public ApiSampleController(ISender sender,
            ILogger<ApiSampleController> logger)
        {
            _sender = sender;
            _logger = logger;
        }


        [ProducesResponseType(typeof(ApiResponse<LoginResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResult>), (int)HttpStatusCode.BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _sender.Send(command);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
