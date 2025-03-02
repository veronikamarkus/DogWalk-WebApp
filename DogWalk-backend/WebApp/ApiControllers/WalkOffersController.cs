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
    public class WalkOffersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.WalkOffer, App.BLL.DTO.WalkOffer> _mapper;

        public WalkOffersController(UserManager<AppUser> userManager, IAppBLL bll, IMapper autoMapper)
        {
            _userManager = userManager;
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.WalkOffer, App.BLL.DTO.WalkOffer>(autoMapper);
        }
        
        // GET: api/WalkOffers
        /// <summary>
        /// Get all walk offers
        /// </summary>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.WalkOffer&gt;</returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.WalkOffer>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.WalkOffer>>> GetWalkOffers()
        {
            var walkOffers = await _bll.WalkOffers.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User)
                ));
            
            return Ok(walkOffers);
        }
        
        /// <summary>
        /// Get walk offers that belong to certain walk, using walk id
        /// </summary>
        /// <param name="walkId">Guid</param>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.WalkOffer&gt;&gt;</returns>
        [HttpGet("WalkOffersByWalk/{walkId}")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.WalkOffer>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.WalkOffer>>> GetWalkOffersByWalk(Guid walkId)
        {
            var walkOffers = (await _bll.WalkOffers.GetAllAsync())
                .Where(wo => wo.WalkId == walkId);

            return Ok(walkOffers);
        }

        // GET: api/WalkOffers/5
        /// <summary>
        /// Get walkOffer by walkOffer id
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>App.DTO.v1_0.WalkOffer</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<App.DTO.v1_0.WalkOffer>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.WalkOffer>> GetWalkOffer(Guid id)
        {
            var walkOffer = _mapper.Map(await _bll.WalkOffers
                .FirstOrDefaultAsync(id));

            if (walkOffer == null)
            {
                return NotFound();
            }

            return walkOffer;
        }

        // PUT: api/WalkOffers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update walk Offer
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="walkOffer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutWalkOffer(Guid id, App.DTO.v1_0.WalkOffer walkOffer)
        {
            if (id != walkOffer.Id)
            {
                return BadRequest();
            }

            _bll.WalkOffers.Update(_mapper.Map(walkOffer));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WalkOfferExists(id))
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

        // POST: api/WalkOffers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new walk offer from current user
        /// </summary>
        /// <param name="walkOffer">App.DTO.v1_0.WalkOffer</param>
        /// <returns>App.DTO.v1_0.WalkOffer</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.WalkOffer>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.WalkOffer>> PostWalkOffer(App.DTO.v1_0.WalkOffer walkOffer)
        {
            walkOffer.AppUserId = Guid.Parse(_userManager.GetUserId(User));
            _bll.WalkOffers.Add(_mapper.Map(walkOffer));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetWalkOffer", new {
                version = HttpContext.GetRequestedApiVersion()!.ToString(),
                id = walkOffer.Id 
            }, walkOffer);
        }

        // DELETE: api/WalkOffers/5
        /// <summary>
        /// Delete walk offer
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteWalkOffer(Guid id)
        {
            var rowsDeleted = await _bll.WalkOffers
                .RemoveAsync(id, Guid.Parse(_userManager.GetUserId(User))
                );
            
            if (rowsDeleted < 1)
            {
                return NotFound();
            }
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> WalkOfferExists(Guid id)
        {
            return await _bll.WalkOffers.ExistsAsync(id);
        }
    }
}
