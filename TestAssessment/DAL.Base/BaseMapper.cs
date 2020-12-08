using System;
using AutoMapper;
using AutoMapper.Configuration;
using Contracts.DAL.Base;

namespace DAL.Base
{
    public class BaseMapper<TLeftObject, TRightObject> : IBaseMapper<TLeftObject, TRightObject>
        where TRightObject : class?, new()
        where TLeftObject : class?, new()
    {
        protected IMapper Mapper;
        protected readonly MapperConfigurationExpression MapperConfigurationExpression;

        public BaseMapper()
        {
            MapperConfigurationExpression = new MapperConfigurationExpression();
            MapperConfigurationExpression.CreateMap<TLeftObject, TRightObject>();
            MapperConfigurationExpression.CreateMap<TRightObject, TLeftObject>();
            
            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }

        public virtual TRightObject Map(TLeftObject inObject)
        {
            return Mapper.Map<TLeftObject, TRightObject>(inObject);
        }

        public virtual TLeftObject Map(TRightObject inObject)
        {
            return Mapper.Map<TRightObject, TLeftObject>(inObject);
        }
    }

}