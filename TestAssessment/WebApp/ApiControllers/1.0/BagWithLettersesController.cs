using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using PublicApi.v1.DTO.BagWithLetters;
using PublicApi.v1.DTO.Mappers;
using V1DTO=PublicApi.v1.DTO;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Bag with letters controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BagWithLettersesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;
        private readonly PublicApiMapper<DAL.App.DTO.BagWithLetters, BagWithLetters> _mapper = 
            new PublicApiMapper<DAL.App.DTO.BagWithLetters, BagWithLetters>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow">Unit of work</param>
        public BagWithLettersesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Get all bags with letters that belong to specified shipment
        /// </summary>
        /// <param name="shipmentId">Shipment id</param>
        /// <returns>List of bags with letters</returns>
        [HttpGet("shipment/{shipmentId}")]
        public async Task<ActionResult<IEnumerable<BagWithLetters>>> GetAllByShipment(Guid shipmentId)
        {
            return Ok((await _uow.BagWithLetterses
                .AllByShipmentAsync(shipmentId)).Select(e => _mapper.Map(e)));
        }

        /// <summary>
        /// Get bag with letters by id
        /// </summary>
        /// <param name="id">Bag with letters id</param>
        /// <returns>Bag with letters object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BagWithLetters>> GetBagWithLetters(Guid id)
        {
            var bagWithLetters = await _uow.BagWithLetterses.FirstOrDefaultAsync(id);

            if (bagWithLetters == null)
            {
                return NotFound("Bag with such id was not found.");
            }

            return Ok(_mapper.Map(bagWithLetters));
        }

        /// <summary>
        /// Change bag with letters info
        /// </summary>
        /// <param name="id">Bag with letters id</param>
        /// <param name="bagWithLetters">Bag with letters object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBagWithLetters(Guid id, BagWithLetters bagWithLetters)
        {
            if (id != bagWithLetters.Id)
            {
                return BadRequest("Id and bagWithLetters.Id does not match.");
            }
            
            if (!await _uow.BagWithLetterses.ExistsAsync(id))
            {
                return NotFound("Bag with such id was not found.");
            }
            
            // Check if shipment is already finalized
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithLetters.ShipmentId);
            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found");
            }
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot update bags in shipment that is already finalized.");
            }

            await _uow.BagWithLetterses.UpdateAsync(_mapper.Map(bagWithLetters));
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new bag with letters
        /// </summary>
        /// <param name="bagWithLettersCreate">Bag with letters object</param>
        /// <returns>Created bag with letters</returns>
        [HttpPost]
        public async Task<ActionResult<BagWithLetters>> PostBagWithLetters(BagWithLettersCreate bagWithLettersCreate)
        {
            if (await _uow.BagWithLetterses.ExistsByBagNumberAsync(bagWithLettersCreate.BagNumber))
            {
                return Conflict("Bag with such bag number already exists!");
            }
            
            // Check if shipment is already finalized
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithLettersCreate.ShipmentId);
            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found");
            }
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot add bags to shipment that is already finalized.");
            }
            
            var bagWithLetters = new BagWithLetters()
            {
                BagNumber = bagWithLettersCreate.BagNumber,
                CountOfLetters = bagWithLettersCreate.CountOfLetters,
                Price = bagWithLettersCreate.Price,
                Weight = bagWithLettersCreate.Weight,
                ShipmentId = bagWithLettersCreate.ShipmentId
            };
            
            var dalEntity = _uow.BagWithLetterses.Add(_mapper.Map(bagWithLetters));
            await _uow.SaveChangesAsync();
            bagWithLetters.Id = dalEntity.Id;

            return CreatedAtAction("GetBagWithLetters", new { id = bagWithLetters.Id }, bagWithLetters);
        }

        /// <summary>
        /// Delete bag with letters
        /// </summary>
        /// <param name="id">Bag with letters id</param>
        /// <returns>Deleted bag with letters</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BagWithLetters>> DeleteBagWithLetters(Guid id)
        {
            var bagWithLetters = await _uow.BagWithLetterses.FirstOrDefaultAsync(id);
            if (bagWithLetters == null)
            {
                return NotFound("Bag with such id was not found.");
            }

            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithLetters.ShipmentId);
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot delete bag that belongs to finalized shipment.");
            }

            await _uow.BagWithLetterses.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return Ok(_mapper.Map(bagWithLetters));
        }
    }
}
