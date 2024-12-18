using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Models;
using OrderProcessingSystem.Shared.Models.DTOs;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.UserRepo;
using System.Security.Claims;

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

            TypeAdapterConfig<UserEntity, UserDTO>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.IsCustomer, src => src.IsCustomer)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
                //.Map(dest => dest.Claims, src => src.UserClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList())
                .Map(dest => dest.Claims, src => src.UserClaims.Select(c => $"{c.ClaimType}:{c.ClaimValue}").ToList())
                .Map(dest => dest.Roles, src => src.UserRoles
                                                   .Select(ur => ur.Role.Name).ToList());
            TypeAdapterConfig<UserDTO, UserEntity>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.IsCustomer, src => src.IsCustomer)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber.ToString())
                .Map(dest => dest.UserClaims, src => src.Claims.Select(c => new IdentityUserClaim<int>
                {
                    ClaimType = c.Split(':', StringSplitOptions.None)[0],
                    ClaimValue = c.Split(':', StringSplitOptions.None)[1],
                    UserId = src.Id // Assuming the UserId is needed
                }))
                .Map(dest => dest.UserRoles, src => src.Roles.Select(role => new UserRoleEntity
                {
                    Role = new RoleEntity { Name = role }
                }));
                //.Map(dest => dest.UserRoles, src => src.Roles.Select(roleName => new UserRoleEntity
                //{
                //    Role = new RoleEntity { Name = roleName }, 
                //    UserId = src.Id 
                //}));



        }

        //public static DestinationModel MapDBModelToModel(SourceModel sourceModelObj )
        //{
        //    return new DestinationModel
        //    {

        //    };
        //}
    }
}
