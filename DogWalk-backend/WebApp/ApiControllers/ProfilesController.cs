using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;
using Profile = App.Domain.Profile;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfilesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Profile, App.BLL.DTO.Profile> _mapper;
        
        public ProfilesController(IAppBLL bll, UserManager<AppUser> userManager, IMapper autoMapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Profile, App.BLL.DTO.Profile>(autoMapper);;
        }

        // GET: api/Profiles
        /// <summary>
        /// Returns all Profiles visible to current user (his profile)
        /// </summary>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.Profile&gt;</returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Profile>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Profile>>> GetProfiles()
        {
            var res = (await _bll.Profiles.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User))
            )).Select(p => _mapper.Map(p));
            return Ok(res);
        }

        // GET: api/Profiles/5
        /// <summary>
        /// Get users profile by profile id
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>App.DTO.v1_0.Profile</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<App.DTO.v1_0.Profile>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Profile>> GetProfile(Guid id)
        {
            var profile =  _mapper.Map(await _bll.Profiles
                .FirstOrDefaultAsync(id));
            //     .FirstOrDefaultAsync(id, Guid.Parse(_userManager.GetUserId(User))
            // ));

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update profile info
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="profile">App.DTO.v1_0.Profile</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutProfile(Guid id, App.DTO.v1_0.Profile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }
            _bll.Profiles.Update(_mapper.Map(profile));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Add Profile
        /// </summary>
        /// <param name="profile">App.DTO.v1_0.Profile</param>
        /// <returns>App.DTO.v1_0.Profile</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Profile>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Profile>> PostProfile(App.DTO.v1_0.Profile profile)
        {
            profile.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            _bll.Profiles.Add(_mapper.Map(profile));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new {
                version = HttpContext.GetRequestedApiVersion()!.ToString(),
                id = profile.Id 
            }, profile);
        }

        // DELETE: api/Profiles/5
        /// <summary>
        /// Delete profile
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            var rowsDeleted = await _bll.Profiles
                .RemoveAsync(id, Guid.Parse(_userManager.GetUserId(User))
                );
            if (rowsDeleted < 1)
            {
                return NotFound();
            }
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        
        
        private async Task<bool> ProfileExists(Guid id)
        {
            return await _bll.Dogs.ExistsAsync(id,
                Guid.Parse(_userManager.GetUserId(User))
            );
        }
    }
}
