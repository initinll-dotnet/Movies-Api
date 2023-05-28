using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Movies.Api.Sdk.Consumer;

public class AuthTokenProvider
{
    private readonly HttpClient _httpClient;
    private string _cachedToken = string.Empty;
    private static SemaphoreSlim? semaphoreSlim;

    public AuthTokenProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
        semaphoreSlim = new SemaphoreSlim(1, 1);
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
            var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));

            if (expiryDateTime > DateTime.UtcNow)
            {
                return _cachedToken;
            }
        }

        //await Lock.WaitAsync();
        await semaphoreSlim!.WaitAsync();

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5164/api/Identity/token", new
        {
            userId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            email = "nitin.londhe@genmills.com",
            customClaims = new Dictionary<string, object>
            {
                { "admin", true },
                { "trusted_member", true }
            }
        });

        var newToken = await response.Content.ReadAsStringAsync();
        _cachedToken = newToken;

        //Lock.Release();
        semaphoreSlim!.Release();

        return newToken;
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
