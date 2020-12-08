using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using PublicApi.v1.DTO.Mappers;
using PublicApi.v1.DTO.Shipment;
using V1DTO=PublicApi.v1.DTO;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Shipments controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShipmentsController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;
        private readonly PublicApiMapper<DAL.App.DTO.Shipment, Shipment> _mapper = 
            new PublicApiMapper<DAL.App.DTO.Shipment, Shipment>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uow">Unit of work</param>
        public ShipmentsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Get all shipments
        /// </summary>
        /// <returns>List of shipments</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            return Ok((await _uow.Shipments.AllAsync()).Select(e => _mapper.MapShipment(e)));
        }

        /// <summary>
        /// Get shipment by id
        /// </summary>
        /// <param name="id">Shipment id</param>
        /// <returns>Shipment object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(Guid id)
        {
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(id);

            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found.");
            }

            return Ok(_mapper.MapShipment(shipment));
        }

        /// <summary>
        /// Update shipment info
        /// </summary>
        /// <param name="id">Shipment id</param>
        /// <param name="shipment">Shipment object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipment(Guid id, Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest("Id and shipment.Id does not match.");
            }
            
            var shipmentEntity = await _uow.Shipments.FirstOrDefaultAsync(id);
            if (shipmentEntity == null)
            {
                return NotFound("Shipment with such id does not exist.");
            }
            
            if (shipmentEntity.IsFinalized)
            {
                return Conflict("Cannot update finalized shipment.");
            }

            await _uow.Shipments.UpdateAsync(_mapper.MapShipment(shipment));
            await _uow.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Change shipment status to finalized
        /// </summary>
        /// <param name="id">Shipment id</param>
        /// <returns></returns>
        [HttpPut("finalize/{id}")]
        public async Task<IActionResult> FinalizeShipment(Guid id)
        {
            if (!await _uow.Shipments.ExistsAsync(id))
            {
                return NotFound("Shipment with such id was not found.");
            }
            
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(id);

            if (shipment.FlightDate < DateTime.Now)
            {
                return Conflict("Flight date cannot be in past by the moment of finalizing shipment.");
            }

            var bagsWithParcels = await _uow.BagWithParcelses.AllByShipmentAsync(shipment.Id);
            var bagsWithLetters = await _uow.BagWithLetterses.AllByShipmentAsync(shipment.Id);

            if (bagsWithParcels.Count.Equals(0) && bagsWithLetters.Count.Equals(0))
            {
                return BadRequest("Cannot finalize shipment with no bags.");
            }

            if (bagsWithParcels.Any(b => b.Parcels == null || b.Parcels.Count.Equals(0)))
            {
                return BadRequest("Cannot finalize shipment that contains empty bags.");
            }
            
            shipment.IsFinalized = true;
            await _uow.Shipments.UpdateAsync(shipment);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new shipment
        /// </summary>
        /// <param name="shipmentCreate">Shipment object</param>
        /// <returns>Created shipment</returns>
        [HttpPost]
        public async Task<ActionResult<Shipment>> PostShipment(ShipmentCreate shipmentCreate)
        {
            try
            {
                var airport = (Airport) Enum.Parse(typeof(Airport), shipmentCreate.Airport);
            }
            catch (ArgumentException)
            {
                return Conflict("Airport can be only TLL, RIX or HEL.");
            }

            if (await _uow.Shipments.ExistsByShipmentNumberAsync(shipmentCreate.ShipmentNumber))
            {
                return Conflict("Shipment with such shipment number already exists!");
            }

            if (shipmentCreate.IsFinalized)
            {
                return Conflict("Cannot finalize shipment without any bags!");
            }
            
            var shipment = new Shipment()
            {
                Airport = shipmentCreate.Airport,
                FlightDate = shipmentCreate.FlightDate,
                FlightNumber = shipmentCreate.FlightNumber,
                IsFinalized = shipmentCreate.IsFinalized,
                ShipmentNumber = shipmentCreate.ShipmentNumber
            };

            var dalEntity = _uow.Shipments.Add(_mapper.MapShipment(shipment));
            await _uow.SaveChangesAsync();
            shipment.Id = dalEntity.Id;
            
            return CreatedAtAction("GetShipment", new { id = shipment.Id }, shipment);
        }

        /// <summary>
        /// Delete shipment with all its bags and parcels
        /// </summary>
        /// <param name="id">Shipment id</param>
        /// <returns>Deleted shipment</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Shipment>> DeleteShipment(Guid id)
        {
            var shipment = await _uow.Shipments.FirstOrDefaultAsync(id);
            if (shipment == null)
            {
                return NotFound("Shipment with such id was not found.");
            }
            
            if (shipment.IsFinalized)
            {
                return Conflict("Cannot delete finalized shipment.");
            }

            await _uow.Shipments.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return Ok(_mapper.MapShipment(shipment));
        }
    }
}
