namespace MTGProject.Models
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_confirmed_at")]
        public DateTimeOffset EmailConfirmedAt { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("confirmed_at")]
        public DateTimeOffset ConfirmedAt { get; set; }

        [JsonProperty("last_sign_in_at")]
        public DateTimeOffset LastSignInAt { get; set; }

        [JsonProperty("app_metadata")]
        public AppMetadata AppMetadata { get; set; }

        [JsonProperty("user_metadata")]
        public Data UserMetadata { get; set; }

        [JsonProperty("identities")]
        public Identity[] Identities { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; set; }
    }

    public partial class AppMetadata
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("providers")]
        public string[] Providers { get; set; }
    }

    public partial class Identity
    {
        [JsonProperty("identity_id")]
        public Guid IdentityId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        [JsonProperty("identity_data")]
        public Data IdentityData { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("last_sign_in_at")]
        public DateTimeOffset LastSignInAt { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("phone_verified")]
        public bool PhoneVerified { get; set; }

        [JsonProperty("sub")]
        public Guid Sub { get; set; }
    }

    public partial class TokenResponse
    {
        public static TokenResponse FromJson(string json) => JsonConvert.DeserializeObject<TokenResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TokenResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
