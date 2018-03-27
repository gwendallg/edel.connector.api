﻿using System;
using System.ComponentModel.DataAnnotations;
using Autumn.Mvc.Data.Annotations;
using Autumn.Mvc.Data.MongoDB.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Adventiel.Connector.Api.Models.V1.Cattles.Production.Milk
{
    [BsonIgnoreExtraElements]
    [Collection(Name = "cattleMastitis")]
    [Entity(Name = "cattle-mastitis")]
    public class MastitisModel : AbstractCattleDataModel
    {
        public string CollectOriginCode { get; set; }
        public string MastitisSeverity { get; set; }
        [Required]
        public DateTime CollectMastitisDate { get; set; }
    }
}