namespace Hapi.Data.Abstracts
{
    public interface ITokenResponse
    {
        string AccessToken { get; set; }
        string TokenType { get; set; }
    }
}