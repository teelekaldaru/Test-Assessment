using System;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using PublicApi.v1.DTO.Mappers;
using V1DTO=PublicApi.v1.DTO;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Parcels controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ParcelsController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;
        private readonly PublicApiMapper<DAL.App.DTO.Parcel, V1DTO.Parcel> _mapper = 
            new PublicApiMapper<DAL.App.DTO.Parcel, V1DTO.Parcel>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow">Unit of work</param>
        public ParcelsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Get parcel by id
        /// </summary>
        /// <param name="id">Parcel id</param>
        /// <returns>Parcel object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<V1DTO.Parcel>> GetParcel(Guid id)
        {
            var parcel = await _uow.Parcels.FirstOrDefaultAsync(id);
            if (parcel == null)
            {
                return NotFound("Parcel with such id was not found.");
            }

            return Ok(_mapper.Map(parcel));
        }

        /// <summary>
        /// Update parcel info
        /// </summary>
        /// <param name="id">Parcel id</param>
        /// <param name="parcel">Parcel object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParcel(Guid id, V1DTO.Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return BadRequest("Id and parcel.Id does not match.");
            }
            
            if (!await _uow.Parcels.ExistsAsync(id))
            {
                return NotFound("Parcel with such id was not found.");
            }
            
            // Check if shipment is already finalized
            var bag = await _uow.BagWithParcelses.FirstOrDefaultAsync(parcel.BagWithParcelsId);
            if (bag == null)
            {
                return NotFound("Bag with such id was not found");
            }
            
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bag.ShipmentId);
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot update parcels in bag that belongs to finalized shipment.");
            }

            await _uow.Parcels.UpdateAsync(_mapper.Map(parcel));
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new parcel
        /// </summary>
        /// <param name="parcel">Parcel object</param>
        /// <returns>Created parcel</returns>
        [HttpPost]
        public async Task<ActionResult<V1DTO.Parcel>> PostParcel(V1DTO.Parcel parcel)
        {
            if (await _uow.Parcels.ExistsByParcelNumberAsync(parcel.ParcelNumber))
            {
                return Conflict("Parcel with such parcel number already exists!");
            }
            
            // Check if shipment is already finalized
            var bag = await _uow.BagWithParcelses.FirstOrDefaultAsync(parcel.BagWithParcelsId);
            if (bag == null)
            {
                return NotFound("Bag with such id was not found");
            }
            
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bag.ShipmentId);
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot add parcels in bag that belongs to shipment that is already finalized.");
            }

            parcel.DestinationCountry = parcel.DestinationCountry.ToUpper();
            
            var dalEntity = _uow.Parcels.Add(_mapper.Map(parcel));
            await _uow.SaveChangesAsync();
            parcel.Id = dalEntity.Id;

            return CreatedAtAction("GetParcel", new { id = parcel.Id }, parcel);
        }

        /// <summary>
        /// Delete parcel
        /// </summary>
        /// <param name="id">Parcel id</param>
        /// <returns>Deleted parcel</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<V1DTO.Parcel>> DeleteParcel(Guid id)
        {
            var parcel = await _uow.Parcels.FirstOrDefaultAsync(id);
            if (parcel == null)
            {
                return NotFound("Parcel with such id was not found.");
            }

            var bag = await _uow.BagWithParcelses.FirstOrDefaultAsync(parcel.BagWithParcelsId);
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(bag.ShipmentId);
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot delete parcel that belongs to bag in finalized shipment.");
            }

            await _uow.Parcels.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return Ok(_mapper.Map(parcel));
        }
    }
}
