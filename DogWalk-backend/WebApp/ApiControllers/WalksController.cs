using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Helpers;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalksController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Walk, App.BLL.DTO.Walk> _mapper;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location> _locationMapper;

        public WalksController(UserManager<AppUser> userManager, IAppBLL bll, IMapper autoMapper)
        {
            _userManager = userManager;
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Walk, App.BLL.DTO.Walk>(autoMapper);
            _locationMapper = new PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location>(autoMapper);
        }

        // GET: api/Walks
        [HttpGet]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Walk>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Walk>>> GetWalks()
        {
            var usersWalks = await _bll.UserInWalks.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User))
            );

            var resList = new List<App.DTO.v1_0.Walk>();
            
            foreach (Guid walkId in usersWalks.Select(uw => uw.WalkId))
            {
                var walk = await _bll.Walks.FirstOrDefaultAsync(walkId);
                resList.Add(_mapper.Map(walk));
            }
            
            return Ok(resList);
        }
        
        /// <summary>
        /// Get list of walks by their location, only active walks and those user haven't send offers yet
        /// </summary>
        /// <param name="location">string</param>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.Walk&gt;</returns>
        [HttpGet("WalksByLocation/{location}")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Walk>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Walk>>> GetWalksByLocation(string location)
        {
            var usersWalkOffers = (await _bll.WalkOffers
                .GetAllAsync(Guid.Parse(_userManager.GetUserId(User)))).Select(wo => wo.WalkId);
            
            var walks = (await _bll.Walks.GetActiveWalksByLocation(location))
                .Where(w => !usersWalkOffers.Contains(w.Id));
            
            return Ok(walks);
        }

        // GET: api/Walks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1_0.Walk>> GetWalk(Guid id)
        {
            var walk = _mapper.Map(await _bll.Walks
                .FirstOrDefaultAsync(id)
                );

            if (walk == null)
            {
                return NotFound();
            }

            return walk;
        }

        // PUT: api/Walks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=212375
        /// <summary>
        /// Update walk
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="walk">App.DTO.v1_0.Walk</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutWalk(Guid id, App.DTO.v1_0.Walk walk)
        {
            if (id != walk.Id)
            {
                return BadRequest();
            }

            _bll.Walks.Update(_mapper.Map(walk));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WalkExists(id))
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
        
        // POST: api/Walks/5/6
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("AddUserInWalk/{walkId}/{appUserId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult> GetAddUserInWalk(Guid walkId, Guid appUserId)
        {
            Console.WriteLine("adding userinwalk heh");
            Console.WriteLine("walk: " + walkId);
            Console.WriteLine("user: " + appUserId);
            _bll.UserInWalks.Add(new App.BLL.DTO.UserInWalk()
            {
                WalkId = walkId,
                AppUserId = appUserId
            });

            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Walks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{dogId}")]
        [ProducesResponseType<App.DTO.v1_0.Walk>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Walk>> PostWalk(Guid dogId, [FromBody] App.DTO.v1_0.Walk walk)
        {
            _bll.DogInWalks.Add(new App.BLL.DTO.DogInWalk()
            {
                DogId = dogId,
                WalkId = walk.Id
            });
            
            _bll.UserInWalks.Add(new App.BLL.DTO.UserInWalk()
            {
                WalkId = walk.Id,
                AppUserId = Guid.Parse(_userManager.GetUserId(User))
            });
            
            _bll.Walks.Add(_mapper.Map(walk));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetWalk", new {
                version = HttpContext.GetRequestedApiVersion()!.ToString(),
                id = walk.Id 
            }, walk);
        }

        private async Task<bool> WalkExists(Guid id)
        {
            return await _bll.Walks.ExistsAsync(id
              //  Guid.Parse(_userManager.GetUserId(User))
            );
        }
    }
}
