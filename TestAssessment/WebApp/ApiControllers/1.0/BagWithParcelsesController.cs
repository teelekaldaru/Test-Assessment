using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.v1.DTO.BagWithParcels;
using PublicApi.v1.DTO.Mappers;
using V1DTO=PublicApi.v1.DTO;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Bag with parcels controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BagWithParcelsesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;
        private readonly PublicApiMapper<DAL.App.DTO.BagWithParcels, BagWithParcels> _mapper = 
            new PublicApiMapper<DAL.App.DTO.BagWithParcels, BagWithParcels>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow">Unit of work</param>
        public BagWithParcelsesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Get all bags with parcels that belong to specified shipment
        /// </summary>
        /// <param name="shipmentId">Shipment id</param>
        /// <returns>List of bags with parcels views</returns>
        [HttpGet("shipment/{shipmentId}")]
        public async Task<ActionResult<IEnumerable<BagWithParcelsView>>> GetAllByShipment(Guid shipmentId)
        {
            var mapper = new PublicApiMapper<DAL.App.DTO.BagWithParcelsView, BagWithParcelsView>();
            
            return Ok((await _uow.BagWithParcelses
                .AllByShipmentAsync(shipmentId)).Select(e => mapper.Map(e)));
        }

        /// <summary>
        /// Get bag with parcels by id
        /// </summary>
        /// <param name="id">Bag with parcels id</param>
        /// <returns>Bag with parcels object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BagWithParcels>> GetBagWithParcels(Guid id)
        {
            var bagWithParcels = await _uow.BagWithParcelses.FirstOrDefaultAsync(id);
            if (bagWithParcels == null)
            {
                return NotFound("Bag with such id was not found.");
            }

            return Ok(_mapper.Map(bagWithParcels));
        }

        /// <summary>
        /// Change bag with parcels info
        /// </summary>
        /// <param name="id">Bag with parcels id</param>
        /// <param name="bagWithParcels">Bag with parcels object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBagWithParcels(Guid id, BagWithParcels bagWithParcels)
        {
            if (id != bagWithParcels.Id)
            {
                return BadRequest("Id and bagWithParcels.Id does not match.");
            }
            
            if (!await _uow.BagWithParcelses.ExistsAsync(id))
            {
                return NotFound("Bag with such id was not found.");
            }

            // Check if shipment is already finalized
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithParcels.ShipmentId);
            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found");
            }
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot update bags in shipment that is already finalized.");
            }

            await _uow.BagWithParcelses.UpdateAsync(_mapper.Map(bagWithParcels));
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new bag with parcels
        /// </summary>
        /// <param name="bagWithParcelsCreate">Bag wth parcels object</param>
        /// <returns>Created bag with parcels</returns>
        [HttpPost]
        public async Task<ActionResult<BagWithParcels>> PostBagWithParcels(BagWithParcelsCreate bagWithParcelsCreate)
        {
            if (await _uow.BagWithParcelses.ExistsByBagNumberAsync(bagWithParcelsCreate.BagNumber))
            {
                return Conflict("Bag with such bag number already exists!");
            }

            // Check if shipment is already finalized
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithParcelsCreate.ShipmentId);
            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found");
            }
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot add bags to shipment that is already finalized.");
            }
            
            var bagWithParcels = new BagWithParcels()
            {
                BagNumber = bagWithParcelsCreate.BagNumber,
                ShipmentId = bagWithParcelsCreate.ShipmentId
            };
            
            var dalEntity = _uow.BagWithParcelses.Add(_mapper.Map(bagWithParcels));
            await _uow.SaveChangesAsync();
            bagWithParcels.Id = dalEntity.Id;

            return CreatedAtAction("GetBagWithParcels", new { id = bagWithParcels.Id }, bagWithParcels);
        }

        /// <summary>
        /// Delete bag with parcels including all its parcels
        /// </summary>
        /// <param name="id">Bag with parcels id</param>
        /// <returns>Deleted bag with parcels</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<BagWithParcels>> DeleteBagWithParcels(Guid id)
        {
            var bagWithParcels = await _uow.BagWithParcelses.FirstOrDefaultAsync(id);
            if (bagWithParcels == null)
            {
                return NotFound("Bag with such id was not found.");
            }

            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bagWithParcels.ShipmentId);
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot delete bag that belongs to finalized shipment.");
            }

            await _uow.BagWithParcelses.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return Ok(_mapper.Map(bagWithParcels));
        }
    }
}
