using System;
using System.Collections.Generic;
using System.Text;

namespace lojaServiceContract.DTO
{
    public class BaseResponse
    {
        public bool Success { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public string Severity { get; set; }
    }
}
