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

        config.NewConfig<UpdateUserDto, User>()
        .IgnoreNullValues(true);


        config.NewConfig<WallPost, WallPostResponseDto>()
        .Map(dest => dest.AuthorName, src => src.Author.Username)
        .Map(dest => dest.AuthorProfilePictureUrl, src => src.Author.ProfilePictureUrl);

        config.NewConfig<CreateWallPostDto, WallPost>();

        config.NewConfig<RegisterRequest, User>()
        .Ignore(dest => dest.PasswordHash);

    }
}
