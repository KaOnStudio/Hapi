namespace Hapi.Data.Abstracts
{
    public interface IResultModel<T> where T:new()
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        T Data { get; set; }       
    }
}