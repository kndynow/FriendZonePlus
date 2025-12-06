using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using Mapster;

namespace FriendZonePlus.Application.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserProfileDto>()
        .Map(dest => dest.FollowersCount, src => src.Followers.Count)
        .Map(dest => dest.FollowingCount, src => src.Following.Count);


    }
}
