using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HighlightCode.App_Start;
using Newtonsoft.Json.Linq;

namespace HighlightCode.Controllers
{
    public class SharedController : ApiController
    {
        [Route("shared/g")]
        public IHttpActionResult Get(string str)
        {
            return Ok(str);
        }
        // POST: api/Shared
        [HttpPost]
        [Route("Shared/GetHighLight")]
        public IHttpActionResult GetHighLight(JObject jsonData)
        {
              dynamic json = jsonData;
            string code = json.str;
            return Ok(code.ToHighLightFormat("java"));
        }

        //[HttpPost]
        //public HttpResponseMessage GetHighLight(string )
        //{
        //    return new ResponseMessage(); str.ToHighLightFormat();
        //}
    }
}
