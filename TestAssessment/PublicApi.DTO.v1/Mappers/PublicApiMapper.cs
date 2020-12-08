using System;
using AutoMapper;
using DAL.App.DTO;
using DAL.Base;

namespace PublicApi.v1.DTO.Mappers
{
    public class PublicApiMapper<TLeftObject, TRightObject> : BaseMapper<TLeftObject, TRightObject>
        where TRightObject : class?, new()
        where TLeftObject : class?, new()
    {
        public PublicApiMapper() : base()
        { 
            MapperConfigurationExpression.CreateMap<DAL.App.DTO.BagWithLetters, BagWithLetters.BagWithLetters>();
            MapperConfigurationExpression.CreateMap<DAL.App.DTO.BagWithParcels, BagWithParcels.BagWithParcels>();
            MapperConfigurationExpression.CreateMap<DAL.App.DTO.Parcel, Parcel>();
            MapperConfigurationExpression.CreateMap<DAL.App.DTO.Shipment, Shipment.Shipment>();
            MapperConfigurationExpression.CreateMap<Shipment.Shipment, DAL.App.DTO.Shipment>();
            
            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }
        
        public DAL.App.DTO.Shipment MapShipment(Shipment.Shipment shipment)
        {
            return new DAL.App.DTO.Shipment()
            {
                Airport = (Airport) Enum.Parse(typeof(Airport), shipment.Airport),
                FlightDate = shipment.FlightDate,
                FlightNumber = shipment.FlightNumber,
                Id = shipment.Id,
                IsFinalized = shipment.IsFinalized,
                ShipmentNumber = shipment.ShipmentNumber
            };
        }
        
        public Shipment.Shipment MapShipment(DAL.App.DTO.Shipment shipment)
        {
            return new Shipment.Shipment()
            {
                Airport = shipment.Airport.ToString(),
                FlightDate = shipment.FlightDate,
                FlightNumber = shipment.FlightNumber,
                Id = shipment.Id,
                IsFinalized = shipment.IsFinalized,
                ShipmentNumber = shipment.ShipmentNumber
            };
        }
    }

}