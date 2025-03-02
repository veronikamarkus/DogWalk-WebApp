using System;
using System.Collections.Generic;
using System.Linq;
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
using Walk = App.DTO.v1_0.Walk;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LocationsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location> _mapper;

        public LocationsController(IAppBLL bll, UserManager<AppUser> userManager, IMapper automapper)
        {
            _bll = bll;
            _userManager = userManager;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Location, App.BLL.DTO.Location>(automapper);
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Location>>> GetLocations()
        {
            var res = (await _bll.Locations.GetAllAsync()).Select(p => _mapper.Map(p));
            return Ok(res);
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1_0.Location>> GetLocation(Guid id)
        {
            var location = _mapper.Map(await _bll.Locations
                .FirstOrDefaultAsync(id)
            );

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(Guid id, [FromBody] App.DTO.v1_0.Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _bll.Locations.Update(_mapper.Map(location));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LocationExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1_0.Location>> PostLocation([FromBody] App.DTO.v1_0.Location location)
        {
            _bll.Locations.Add(_mapper.Map(location));
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new {
                version = HttpContext.GetRequestedApiVersion()!.ToString(),
                id = location.Id 
            }, location);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var rowsDeleted = await _bll.Locations
                .RemoveAsync(id);
            if (rowsDeleted < 1)
            {
                return NotFound();
            }
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> LocationExists(Guid id)
        {
            return await _bll.Dogs.ExistsAsync(id);
        }
    }
}
