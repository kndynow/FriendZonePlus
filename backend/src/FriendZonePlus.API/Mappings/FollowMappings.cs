using Mapster;
using FriendZonePlus.API.DTOs;
using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.API.Mappings;

public class FollowMappings
{

    public static void ConfigureFollowMappings()
    {
        // FollowRequestDto -> Follow
        TypeAdapterConfig<FollowUserRequestDto, Follow>
        .NewConfig()
        .Map(dest => dest.FollowerId, src => src.FollowerId)
        .Map(dest => dest.FollowedUserId, src => src.FollowedUserId)
        .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
        .Ignore(dest => dest.Id)
        .Ignore(dest => dest.Follower)
        .Ignore(dest => dest.FollowedUser);

        // Follow -> FollowResponseDto
        TypeAdapterConfig<Follow, FollowResponseDto>
        .NewConfig()
        .Map(dest => dest.Id, src => src.Id)
        .Map(dest => dest.FollowerId, src => src.FollowerId)
        .Map(dest => dest.FollowedUserId, src => src.FollowedUserId)
        .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        // User -> UserListResponseDto
        TypeAdapterConfig<User, UserListResponseDto>
        .NewConfig()
        .Map(dest => dest.Id, src => src.Id)
        .Map(dest => dest.Username, src => src.Username)
        .Map(dest => dest.Email, src => src.Email);
    }

}
