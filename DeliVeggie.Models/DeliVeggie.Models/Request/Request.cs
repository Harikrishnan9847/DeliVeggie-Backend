namespace DeliVeggie.Models.Request
{
    public class Request<T> : IRequest where T : class
    {
        public T? Data { get; set; }
    }
}
