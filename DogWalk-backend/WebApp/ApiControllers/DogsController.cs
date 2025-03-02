using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class DogsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;
        private readonly PublicDTOBllMapper<App.DTO.v1_0.Dog, App.BLL.DTO.Dog> _mapper;

        public DogsController(UserManager<AppUser> userManager, IAppBLL bll, IMapper autoMapper)
        {
            _userManager = userManager;
            _bll = bll;
            _mapper = new PublicDTOBllMapper<App.DTO.v1_0.Dog, App.BLL.DTO.Dog>(autoMapper);
            
        }
        
        // GET: api/Dogs
        /// <summary>
        /// Returns all Dogs visible to current user (his dogs)
        /// </summary>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.Dog&gt;</returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Dog>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Dog>>> GetDogs()
        {
            
            var usersDogs = await _bll.UsersDogs.GetAllAsync(
                Guid.Parse(_userManager.GetUserId(User))
            );

            var resList = new List<App.DTO.v1_0.Dog>();
            
            foreach (Guid dogId in usersDogs.Select(ud => ud!.DogId))
            {
                var dog = await _bll.Dogs.FirstOrDefaultAsync(dogId);
                resList.Add(_mapper.Map(dog));
            }
            
            return Ok(resList);
        }

        // GET: api/Dogs/5
        /// <summary>
        /// Return the dog by dog's id, that belongs to current user
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>App.DTO.v1_0.Dog</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<App.DTO.v1_0.Dog>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Dog>> GetDog(Guid id)
        {
            var dog = _mapper.Map(await _bll.Dogs
                .FirstOrDefaultAsync(id)
                );
        
            if (dog == null)
            {
                return NotFound();
            }
        
            return dog;
        }

        // PUT: api/Dogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update dog info
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="dog">App.DTO.v1_0.Dog</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutDog(Guid id, [FromBody] App.DTO.v1_0.Dog dog)
        {
            Console.WriteLine("dog name is:" + dog.DogName);
            if (id != dog.Id)
            {
                return BadRequest();
            }

            _bll.Dogs.Update(_mapper.Map(dog));

            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DogExists(id))
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
        
        /// <summary>
        /// Get dogs that belong to walk, by walk id
        /// </summary>
        /// <param name="walkId">Guid</param>
        /// <returns>IEnumerable&lt;App.DTO.v1_0.Dog&gt;&gt;</returns>
        [HttpGet("DogsInWalk/{walkId}")]
        [ProducesResponseType<IEnumerable<App.DTO.v1_0.Dog>>((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<App.DTO.v1_0.Dog>>> GetDogsInWalk(Guid walkId)
        {
            var dogInWalks = (await _bll.DogInWalks.GetAllAsync())
                .Where(dw => dw.WalkId == walkId);

            var res = new List<App.DTO.v1_0.Dog>();

            foreach (Guid dogId in dogInWalks.Select(dw => dw.DogId))
            {
                var dog = await _bll.Dogs.FirstOrDefaultAsync(dogId);
                res.Add(_mapper.Map(dog));
            }

            return Ok(res);
        }

        // POST: api/Dogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Create new dog
        /// </summary>
        /// <param name="dog">App.DTO.v1_0.Dog</param>
        /// <returns>App.DTO.v1_0.Dog</returns>
        [HttpPost]
        [ProducesResponseType<App.DTO.v1_0.Dog>((int) HttpStatusCode.Created)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<App.DTO.v1_0.Dog>> PostDog([FromBody] App.DTO.v1_0.Dog dog)
        {
            _bll.Dogs.Add(_mapper.Map(dog));
            _bll.UsersDogs.Add(new UsersDog()
            {
                DogId = dog.Id,
                AppUserId = Guid.Parse(_userManager.GetUserId(User))
            });
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetDog", new {
                version = HttpContext.GetRequestedApiVersion()!.ToString(),
                id = dog.Id 
            }, dog);
        }

        // DELETE: api/Dogs/5
        /// <summary>
        /// Delete dog belonging to user
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteDog(Guid id)
        {
            var rowsDeleted = await _bll.Dogs
                .RemoveAsync(id, Guid.Parse(_userManager.GetUserId(User))
                    );
            if (rowsDeleted < 1)
            {
                return NotFound();
            }
            
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> DogExists(Guid id)
        {
            return await _bll.Dogs.ExistsAsync(id,
                Guid.Parse(_userManager.GetUserId(User))
                );
           
        }
    }
}
