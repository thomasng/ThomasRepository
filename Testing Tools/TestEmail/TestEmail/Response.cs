using System.Collections.Generic;

namespace TestEmail
{
    public class Response
    {
        public Response()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }
        public object ResponseObject { get; set; }
        public List<string> Warnings { get; set; }
        public bool WasSuccessful { get; set; }
    }
}
