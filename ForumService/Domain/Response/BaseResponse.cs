﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Response
{
	public class BaseResponse<T>
	{
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
