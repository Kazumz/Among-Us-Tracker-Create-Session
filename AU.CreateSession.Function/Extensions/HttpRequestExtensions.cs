﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace AU.CreateSession.Function.Extensions
{
    public class HttpResponseBody<T>
    {
        public bool IsValid { get; set; }

        public T Value { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }

    public static class HttpRequestExtensions
    {
        public static async Task<HttpResponseBody<T>> GetBodyAsync<T>(this HttpRequest request)
        {
            var body = new HttpResponseBody<T>();
            var bodyString = await request.ReadAsStringAsync();
            body.Value = JsonConvert.DeserializeObject<T>(bodyString);

            var results = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), results, true);
            body.ValidationResults = results;
            return body;
        }
    }
}
