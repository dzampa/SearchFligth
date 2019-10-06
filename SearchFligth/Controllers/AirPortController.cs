using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchFligth.Class;

namespace SearchFligth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AirPortController : Controller
    {
        protected readonly ILogger Logger;

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        // GET
        // api/v1/AirPort/airport

        /// <summary>
        /// Retrieves aeroportos JSON
        /// </summary>
        /// <returns>A response with aeroportos JSON</returns>
        /// <response code="200">Returns the aeroportos JSON list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("airport")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetAirPortJson()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAirPortJson));

            Functions functions;
            List<AirPort> ListAirports;

            try
            {
                functions = new Functions();                

                ListAirports = new List<AirPort>();

                ListAirports = functions.AriPortsList();

                return new ObjectResult(ListAirports);

            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetAirPortJson), ex.Message);
                return BadRequest(ex.Message);
            }

        }

        // GET
        // api/v1/AirPort/fligths?AeroOri=FLN&AeroDes=MCZ&Data=2019-02-10

        /// <summary>
        /// Retrieves a list of available flight
        /// </summary>
        /// <param name="AeroOri">Origin airport </param>
        /// <param name="AeroDes">Destiny airport </param>
        /// <param name="Data">Data of Flight </param>
        /// <returns>A response with List of Flights</returns>
        /// <response code="200">Returns the List of Flights</response>
        /// <response code="404">If Flight not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("fligths")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetFlightsList(string AeroOri, string AeroDes, DateTime Data)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetFlightsList));            

            List<FlightList> FlightList = new List<FlightList>();
            List<NineNinePlanes> ListNine = new List<NineNinePlanes>();
            List<UberAir> ListUberAir = new List<UberAir>();

            Functions functions;

            try
            {

                if (string.IsNullOrEmpty(AeroOri))
                    return NotFound("Please send origin airport");

                if (string.IsNullOrEmpty(AeroDes))
                    return NotFound("Please send destination airport");

                if (string.IsNullOrEmpty(Data.ToString()) ||
                            Data < DateTime.Parse("2019-02-10") ||
                            Data > DateTime.Parse("2019-02-18"))
                    return NotFound("Please send travel date between 2019-02-10 and 2019-02-18");

                functions = new Functions();

                ListNine = functions.ReadJson();

                ListUberAir = functions.ReadUberAir();

                FlightList = functions.ReturnFlightList(AeroOri, AeroDes, Data, ListNine, ListUberAir);

                return new ObjectResult(FlightList);

            }
            catch (Exception ex)
            {
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlightsList), ex.Message);
                return BadRequest(ex.Message);
            }
        }

       
    }
}
