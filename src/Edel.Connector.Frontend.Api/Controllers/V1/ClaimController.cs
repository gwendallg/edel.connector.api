﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Autumn.Mvc.Data.Models;
using Edel.Connector.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Edel.Connector.Frontend.Api.Controllers.V1
{
    [Route("v1/claim")]
    public class ClaimController : Controller
    {
        private readonly IClaimsService _claimsService;

        public ClaimController(IClaimsService claimsService)
        {
            _claimsService = claimsService;
        }
        
        /// <summary>
        /// find user
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation( Tags = new []{"claim"})]
        [HttpGet()]
        public IActionResult Get()
        {
            try
            {
                var result = new Dictionary<string, string>();

                foreach (var info in _claimsService.GetClaimsByEntityType())
                {
                    var claimKey = string.Format("{0}_{1}", info.ApiVersion, info.Name);
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append($"{ScopeType.Read}, ");
                    if (!info.IgnoreOperations.Contains(HttpMethod.Post))
                        stringBuilder.Append($"{ScopeType.Create}, ");
                    if (!info.IgnoreOperations.Contains(HttpMethod.Put))
                        stringBuilder.Append($"{ScopeType.Update}, ");
                    if (!info.IgnoreOperations.Contains(HttpMethod.Delete))
                        stringBuilder.Append($"{ScopeType.Delete}, ");
                    var claimValue = stringBuilder.ToString().Trim().TrimEnd(',');
                    result.Add(claimKey, claimValue);
                }

                result.Add("v1_user", $"{ScopeType.Read}, {ScopeType.Create}, {ScopeType.Update}");
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new ErrorModelInternalError(e));
            }
        }
    }
}