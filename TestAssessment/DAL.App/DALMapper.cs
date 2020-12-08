using System;
using AutoMapper;
using DAL.Base;
using Domain.Enums;

namespace DAL.App
{
    public class DALMapper<TInObject, TOutObject> : BaseMapper<TInObject, TOutObject>
        where TOutObject : class?, new()
        where TInObject : class?, new()
    {
        public DALMapper() : base()
        { 
            MapperConfigurationExpression.CreateMap<Domain.BagWithLetters, DAL.App.DTO.BagWithLetters>();
            MapperConfigurationExpression.CreateMap<Domain.BagWithParcels, DAL.App.DTO.BagWithParcels>();
            MapperConfigurationExpression.CreateMap<Domain.Parcel, DAL.App.DTO.Parcel>();
            MapperConfigurationExpression.CreateMap<Domain.Shipment, DAL.App.DTO.Shipment>();
            MapperConfigurationExpression.CreateMap<DAL.App.DTO.Shipment, Domain.Shipment>();
            
            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }
    }

}