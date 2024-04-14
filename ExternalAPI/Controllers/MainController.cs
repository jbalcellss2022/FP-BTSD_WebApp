using Asp.Versioning;
using ExternalAPI.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// Represents the main entry point for API requests in the application. This controller
    /// handles various actions related to [specific domain - e.g., user management, data processing].
    /// </summary>
    /// <remarks>
    /// This controller is designed to serve as a central hub for the application's primary functionalities.
    /// It includes methods for authenticating users, managing session states, and processing data inputs.
    /// Ensure that all methods are properly secured and accessible only to authorized users where applicable.
    /// </remarks>

    [ApiController]
    [ApiVersion(1.0)]
    [Route("v1")]
    public class MainController : ControllerBase
    {

        /******************************************************************************/
        /**************** PUBLIC METHODS **********************************************/
        /******************************************************************************/

        /// <summary>
        /// User Authentication and JWT Bearer Token Generation.
        /// </summary>
        /// <param name="userParam"></param>
        /// <param password="userParam"></param>
        /// <returns>A newly created authenthication token.</returns>
        /// <response code="200">Succesfully. Returns a newly created authenthication token.</response>
        /// <response code="400">Bad request. Error in parameters values.</response>
        /// <response code="401">Unauthorized. Username or password is incorrect.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="429">Too Many Requests. The request was not accepted because the application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "This POST method will allow you to authenticate your user code (your QRFY E-mail) with the same access data that you currently have on the QRFY portal in order to obtain the JWT Bear Token. Then in order to use the QRFY API methods you will need to use JWT Bearer authentication with your own token. The maximum duration of a token is 30 minutes, during which you can carry out all the operations you need to apply in QRFY API. Once the token has expired you must request a new one if you want to continue using the QRFY API otherwise you will receive 401 Http error message (Unauthorized)." +
            "<br /><br />Calls to the QRFY RESTful API are governed by request-based limits, which means you should consider the total number of API calls your app makes. In addition, there are resource-based rate limits and throttles. Limits are calculated using the leaky bucket algorithm. All requests that are made after rate limits have been exceeded are throttled and an HTTP 429 status code (Too Many Requests) error is returned. Requests succeed again after enough requests have emptied out of the bucket. You can see the current state of the throttle for a store by using the rate limits header." +
            "<br /><br /><i>Consider the rate of transactions per second for each Ip address and user as follows:</i>  <br /> <br />" +
            "<span id=\"especialfont\">The bucket's unit of measurement is the elapsed time in seconds that it takes to complete a request, then the bucket size is 60 seconds, and this can't be exceeded at any given time or a throttling error is returned. The bucket empties at a leak rate of one per second. The 60-second limit applies to the Ip address of the user interacting with the API. Every request to the QRFY API costs at least 0.7 seconds to run. After a request completes, the total elapsed time is calculated and subtracted from the bucket.</span><br /> <br />" +
            "<i>Bucket limit example:</i><br /> <br />" +
            "<span id=\"especialfont\">Suppose that an API user requests to a method only take 0.7 seconds or less. Each request would cost the minimum 0.7 seconds. In this scenario, it's possible for an API user to make a maximum of 85 parallel requests while remaining within the 60 second bucket limit (85=60/0.7).</span><br /> <br />" +
            "<p id=\"headerBorder\"><strong>Your API keys and token carry many privileges, so be sure to keep them secret!. Do not share your API keys or token in publicly accessible areas such GitHub, client-side code, and so forth. All API requests must be made over HTTPS and Content-type as application/json thus calls made over plain HTTP will fail with HTTP 405 status code (Method Not Allowed).</strong></p>"
            ,Tags = ["Authenticate Methods"])]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        // [Produces("application/json")]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ObjectResult Authenticate([FromForm] UserAuth userParam)
        {
            object user = null; // ctxUserService.Authenticate(userParam.Username, userParam.Password);
            if (user != null)
            {
                return this.StatusCode((int)HttpStatusCode.OK, user);
            }
            else
            {
                if (userParam.Username == null || userParam.Password == null)
                {
                    return this.StatusCode((int)HttpStatusCode.BadRequest, "Bad request. Error in parameters.");
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized. Username or password is incorrect.");
                }
            }
        }

        /// <summary>
        /// Authenticates a user based on null parameters.
        /// </summary>
        /// <returns>
        /// A JSON content result indicating the authentication status. Returns a 400 error code with a description of an error in parameters.
        /// </returns>
        /// 
        [NonAction]
        [AllowAnonymous]
        [HttpGet("authenticate")]
        public IActionResult Authenticate()
        {
            return this.StatusCode((int)HttpStatusCode.BadRequest, "Bad request. Error in parameters.");
        }

        /******************************************************************************/
        /**************** PRIVATE METHODS (FOR AUTHENTICATED USERS) *******************/
        /******************************************************************************/

        /// <summary>
        /// Get a list of all available ODM Projects (GetProjects)
        /// </summary>
        /// <response code="200">Succesfully. Returns a Project estructure with all available ODM Projects.</response>
        /// <response code="400">Bad request. Error in parameters values.</response>
        /// <response code="401">Unauthorized. Username or password is incorrect.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="422">Unprocessable Entity. Unexpected API error {GetProjects: projectId}.</response>
        /// <response code="429">Too Many Requests. The request was not accepted because the application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "This GET method will allow you to get all available ODM projects according to the authentication level of your user. You will need to use the value of the ProjectId field later in requests for other API methods. The result will be a JSON list with the available projects and their name labels in different languages.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = new[] { "ODM Values" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]

        [Produces("application/json")]
        [HttpGet("GetProjects")]
        [Authorize]
        public IActionResult GetProjects()
        {
                return StatusCode(200);
        }
    }
}
