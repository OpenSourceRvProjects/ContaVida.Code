using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Models.EventCounter;
using ContaVida.MVC.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventCounterController : ControllerBase
    {

        private IHttpContextAccessor _accessor;
        private IEventCounterService _eventService;
        private readonly IWebHostEnvironment _hostingEnv;

        public EventCounterController(IHttpContextAccessor accessor, IEventCounterService eventService, IWebHostEnvironment hostingEnv)
        {
            _accessor = accessor;
            _eventService = eventService;
            _hostingEnv = hostingEnv;
        }
        // GET: api/<EventCounterController>
        [HttpGet]
        [LoggedUserDataFilter]
        public async Task<ActionResult> Get()
        {
            var eventList = await _eventService.GetCounterList();
            return Ok(eventList);
        }

        [HttpGet]
        [LoggedUserDataFilter]
        [Route("getCountersResume")]
        public async Task<ActionResult> GetResume()
        {
            var counterResults = await _eventService.GetCounterResults();
            return Ok(counterResults);
        }

        // GET api/<EventCounterController>/5
        [HttpGet]
        [AllowAnonymous]
        [Route("getById")]
        public async Task<IActionResult> Get(Guid counterID)
        {
            var counterData = await _eventService.GetCounterData(counterID);
            if (counterData == null)
            {
                return Unauthorized();
            }
            return Ok(counterData);
        }

        // POST api/<EventCounterController>
        [HttpPost]
        [LoggedUserDataFilter]
        public async Task<IActionResult> Post([FromBody] NewEventCounterModel newEvent)
        {
            await _eventService.AddEventCounter(newEvent);
            return Ok();
        }

        [HttpPost]
        [LoggedUserDataFilter]
        [Route("eventsBatchForTesting")]
        public async Task<IActionResult> PostBatck([FromBody] List<NewEventCounterModel> newEventList)
        {

            if (_hostingEnv.EnvironmentName == "PROD")
                return Ok();

            foreach (var newEvent in newEventList)
            {
                try
                {
                    await _eventService.AddEventCounter(newEvent);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return Ok();
        }

        // PUT api/<EventCounterController>/5
        [HttpPut]
        [Route("changeCounterPrivacy")]
        [LoggedUserDataFilter]

        public async Task<IActionResult> Put(Guid id, [FromBody] CounterPrivacySetModel setting)
        {
            await _eventService.SetPrivacyCounter(id, setting);
            return Ok();
        }

        [HttpPut]
        [Route("setTimeRefreshCounterUI")]
        [LoggedUserDataFilter]

        public async Task<IActionResult> PutRefreshCounter(Guid id, [FromBody] CounterRefreshTimerSetModel setting)
        {
            await _eventService.SetRefresherCounterUI(id, setting);
            return Ok();
        }

        [HttpPut]
        [Route("editCounterEvent")]
        [LoggedUserDataFilter]

        public async Task<IActionResult> PutEvent(Guid id, [FromBody] CounterDataModel eventObject, bool isRelapse = false, string relapseMessage = null, int? relapseReason = null)
        {
            await _eventService.UpdateEventCounter(id, eventObject, isRelapse, relapseMessage, relapseReason);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteCounterByID")]
        [ExceptionManager]
        [LoggedUserDataFilter]
        public async Task<IActionResult> DeleteCounterByID(Guid counterID)
        {
            await _eventService.DeleteEventCounterByID(counterID);
            return Ok();
        }

    }
}
