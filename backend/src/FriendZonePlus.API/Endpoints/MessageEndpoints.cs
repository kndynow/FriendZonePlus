namespace FriendZonePlus.API.Endpoints
{
    public static class MessageEndpoints
    {
        public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Message")
                .WithTags("Message");
        }
    }
}
