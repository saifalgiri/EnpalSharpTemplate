using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnpalSharpTemplate.Model
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
