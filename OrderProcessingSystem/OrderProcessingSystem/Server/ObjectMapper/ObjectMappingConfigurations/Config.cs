using Mapster;
using OrderProcessingSystem.Shared.Models;
using OrderProcessingSystemInfrastructure.DataBase.Entities;

namespace OrderProcessingSystem.Server.ObjectMapper.ObjectMappingConfigurations
{
    public static class Config
    {
        public static void RegisterMappings()
        {
            //TypeAdapterConfig<OrderEntity, OrderDTO>.NewConfig()
            //            .MapWith(sourceModelObj => MapDBModelToModel(sourceModelObj)); // bind all 

            TypeAdapterConfig<OrderProductEntity, OrderProductModel>.NewConfig()
                .Map(dest => dest.TotalPrice, src => src.TotalPrice) 
                .Map(dest => dest.Price, src => src.Product.Price);  
        }

        //public static DestinationModel MapDBModelToModel(SourceModel sourceModelObj )
        //{
        //    return new DestinationModel
        //    {

        //    };
        //}
    }
}
