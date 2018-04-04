﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edel.Adventiel.Connector.Api.Models.Users
{
    public class UserPutRequestModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public Dictionary<string,string> Claims { get; set; }
    }
}