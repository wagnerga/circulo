using System.Text.RegularExpressions;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.PharmacyVisitControllerModels;
using Services.Interfaces;

namespace API.Controllers.Public
{
	[Route("[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class PharmacyVisitController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IPharmacyVisitService _pharmacyVisitService;

		public PharmacyVisitController(IPharmacyVisitService pharmacyVisitService, ILoggerFactory loggerFactory)
		{
			_pharmacyVisitService = pharmacyVisitService;
			_logger = loggerFactory.CreateLogger("PharmacyVisitController");
		}

		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetPharmaciesToVisitResponse>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<bool>))]
		public async Task<ActionResult> GetPharmaciesToVisit([FromBody] GetPharmaciesToVisitRequest request)
		{
			try
			{
				var pharmacyNames = await _pharmacyVisitService.GetPharmaciesToVisit(request.X);

				return Ok(new Response<GetPharmaciesToVisitResponse> { Result = new GetPharmaciesToVisitResponse { PharmacyNames = pharmacyNames } });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);

				return BadRequest(new Response<bool> { ErrorMessage = "Something bad happened..." });
			}
		}

		[HttpPost("insert")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<bool>))]
		public async Task<ActionResult> InsertPharmaciesToVisit([FromForm] IFormFile file)
		{
			try
			{
				using var reader = new StreamReader(file.OpenReadStream());
				var fileString = await reader.ReadToEndAsync();

				var pharmaciesVisited = new List<PharmacyVisited>();
				var pharmaciesNearby = new List<PharmacyNearby>();

				var visitedPharmacyMatches = Regex.Matches(fileString, "\"[a-zA-Z ]+:(true|false)\"");
				var nearbyPharmacyMatches = Regex.Matches(fileString, "\"[a-zA-Z ]+:[\\d]+:[\\d]+:(true|false)\"");

				foreach (Match match in visitedPharmacyMatches)
				{
					var data = match.Value.Replace("\"", "").Split(":");

					pharmaciesVisited.Add(new PharmacyVisited
					{
						PharmacyName = data[0],
						SupportsPriorAuth = data[1] == "true"
					});
				}

				foreach (Match match in nearbyPharmacyMatches)
				{
					var data = match.Value.Replace("\"", "").Split(":");

					pharmaciesNearby.Add(new PharmacyNearby
					{
						PharmacyName = data[0],
						Distance = Convert.ToInt32(data[1]),
						Copay = Convert.ToInt32(data[2]),
						SupportsPriorAuth = data[3] == "true"
					});
				}

				await _pharmacyVisitService.InsertPharmaciesToVisit(pharmaciesVisited, pharmaciesNearby);

				return Ok(new Response<bool> { Result = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);

				return BadRequest(new Response<bool> { ErrorMessage = "Something bad happened..." });
			}
		}
	}
}
