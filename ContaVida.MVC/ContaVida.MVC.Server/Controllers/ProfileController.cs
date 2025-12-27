using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.Models.Images;
using ContaVida.MVC.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaVida.MVC.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [LoggedUserDataFilter]

    public class ProfileController : ControllerBase
    {
        private IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/<ProfileController>
        [HttpGet]
        [Route("getImages")]
        public async Task<IActionResult> Get()
        {
            var images = await _profileService.GetProfileImages();
            return Ok(images);
        }

        [HttpGet]
        [Route("getProfileData")]
        public virtual async Task<IActionResult> GetProfileData()
        {
            var images = await _profileService.GetProfileData();
            return Ok(images);
        }


        // POST api/<ProfileController>
        [HttpPost]
        [Route("addImages")]
        public async Task<IActionResult> Post([FromBody] ImageListModel images)
        {
            await _profileService.SaveProfileImages(images);
            return Ok();
        }

    }
}
