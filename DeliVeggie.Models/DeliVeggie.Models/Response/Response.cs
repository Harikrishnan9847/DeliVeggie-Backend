namespace DeliVeggie.Models.Response
{
    public class Response<T> : IResponse where T : class
    {
        public T? Data { get; set; }
    }
}