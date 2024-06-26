using Newtonsoft.Json;

namespace ContactManager.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
