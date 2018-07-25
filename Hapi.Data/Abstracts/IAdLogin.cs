namespace Hapi.Data.Abstracts
{
    public interface IAdLogin:IUserPasword
    {
        string DomainName { get; set; }
    }
}