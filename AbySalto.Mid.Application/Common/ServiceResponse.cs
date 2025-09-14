namespace AbySalto.Mid.Application.Common
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public static ServiceResponse<T> Ok(T Data, string? Message = null)
        {
            return new ServiceResponse<T> { Data = Data, Success = true, Message = Message };
        }

        public static ServiceResponse<T> Fail(string Message)
        {
            return new ServiceResponse<T> { Data = default, Success = false, Message = Message };
        }
    }
}
