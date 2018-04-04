﻿using System;
using System.ComponentModel.DataAnnotations;
using Autumn.Mvc.Data.Annotations;
using Autumn.Mvc.Data.MongoDB.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Adventiel.Connector.Entities.Cattles.Breeding
{
    [BsonIgnoreExtraElements]
    [Collection(Name = "cattleCalving")]
    [Entity(Name = "cattle-calving", Insertable = false, Updatable = false, Deletable = false)]
    public class Calving : AbstractCattleData
    {
        [Required]
        public DateTime CalvingDate { get; set; }
        
        public string CalvingCondition { get; set; }
        
        public int? CalvinRank { get; set; }
        
        public bool? CalvingMultiple { get; set; }
    }
}