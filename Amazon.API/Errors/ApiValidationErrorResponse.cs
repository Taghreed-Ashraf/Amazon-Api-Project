﻿using System.Collections;
using System.Collections.Generic;

namespace Amazon.API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse():base(400)
        {

        }
    }
}
