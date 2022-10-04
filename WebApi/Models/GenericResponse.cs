using System.Net;
using Newtonsoft.Json;

namespace WebApi.Models
{
    public class GenericResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public GenericResponse()
        {
            StatusCode = (int)HttpStatusCode.OK;
            IsSuccessful = true;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
