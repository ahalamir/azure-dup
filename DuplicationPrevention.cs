using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Dep_Prevent
{
    public class DuplicationPrevention
    {
        private readonly ILogger<DuplicationPrevention> _logger;

        public DuplicationPrevention(ILogger<DuplicationPrevention> log)
        {
            _logger = log;
        }

        [FunctionName("DuplicationPrevention")] 
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            using (var hashAlgorithm = SHA512.Create())
            {
                var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
                string checksum = BitConverter.ToString(hash);
                return new OkObjectResult(checksum);
                
            }

            
           
        }
    }
}

