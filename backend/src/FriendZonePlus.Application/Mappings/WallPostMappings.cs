using Mapster;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.API.Mappings;

public static class WallPostMappings
{
    public static void ConfigureWallPostMappings()
    {
        // CreateWallPostDto to WallPost
        //     TypeAdapterConfig<CreateWallPostDto, WallPost>
        //         .NewConfig()
        //         .Map(dest => dest.Content, src => src.Content)
        //         .Map(dest => dest.AuthorId, src => src.AuthorId)
        //         .Map(dest => dest.TargetUserId, src => src.TargetUserId)
        //         .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
        //         .Ignore(dest => dest.Id)
        //         .Ignore(dest => dest.Author)
        //         .Ignore(dest => dest.TargetUser);

        //     // WallPost -> WallPostResponseDto
        //     TypeAdapterConfig<WallPost, WallPostResponseDto>
        //         .NewConfig()
        //         .Map(dest => dest.Id, src => src.Id)
        //         .Map(dest => dest.AuthorId, src => src.AuthorId)
        //         .Map(dest => dest.TargetUserId, src => src.TargetUserId)
        //         .Map(dest => dest.Content, src => src.Content)
        //         .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        //     // UpdateWallPostDto -> WallPost (för uppdateringar)
        //     TypeAdapterConfig<UpdateWallPostDto, WallPost>
        //         .NewConfig()
        //         .Map(dest => dest.Id, src => src.Id)
        //         .Map(dest => dest.Content, src => src.Content)
        //         .Ignore(dest => dest.AuthorId)
        //         .Ignore(dest => dest.TargetUserId)
        //         .Ignore(dest => dest.CreatedAt)
        //         .Ignore(dest => dest.Author)
        //         .Ignore(dest => dest.TargetUser);

    }
}
