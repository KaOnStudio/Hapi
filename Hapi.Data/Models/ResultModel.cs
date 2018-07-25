using Hapi.Data.Abstracts;

namespace Hapi.Data.Models
{
    public class ResultModel<T>:IResultModel<T> where T:new()
    {
        #region Properties

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        #endregion

        #region Constructors

        public ResultModel()
        {
        }

        public ResultModel(T resultData)
        {
            IsSuccess = true;
            Data = resultData;
        }

        public ResultModel(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ResultModel(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }        

        #endregion
    }
}
