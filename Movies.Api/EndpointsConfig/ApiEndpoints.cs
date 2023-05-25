namespace Movies.Api.EndpointsConfig;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        
        public const string Rate = $"{Base}/{{id:guid}}/ratings";
        public const string DeleteRatings = $"{Base}/{{id:guid}}/ratings";
    }

    public static class Ratings
    {
        private const string Base = $"{ApiBase}/ratings";

        public const string GetUserRatings = $"{Base}/me";
    }

    public static class Ping
    {
        private const string Base = $"{ApiBase}/ping";

        public const string Ping1 = $"{Base}/1";
        public const string Ping2 = $"{Base}/2";
        public const string Ping3 = $"{Base}/3";
        public const string Ping4 = $"{Base}/4";
        public const string Ping5 = $"{Base}/5";
    }
}
