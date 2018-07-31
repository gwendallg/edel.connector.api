﻿using System;
using System.ComponentModel.DataAnnotations;
using Autumn.Mvc.Data.Annotations;
using Autumn.Mvc.Data.MongoDB.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Connector.Entities.Cattles.Breeding
{
    [BsonIgnoreExtraElements]
    [Collection(Name = "cattlePregnancyCheck")]
    [Resource(Name = "cattle-pregnancy-check")]
    public class CattleBreederPregnancyCheck : CattleBreederEvent
    {
        public string PregnancyCheckType { get; set; }
        public string PregnancyCheckResult { get; set; }
    }
}