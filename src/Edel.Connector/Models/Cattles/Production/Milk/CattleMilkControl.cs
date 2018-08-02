﻿using Autumn.Mvc.Data.Annotations;
using Autumn.Mvc.Data.MongoDB.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Connector.Models.Cattles.Production.Milk
{
    [BsonIgnoreExtraElements]
    [Collection(Name = "cattleMilkControl")]
    [Resource(Name = "cattle-milk-control")]
    public class CattleMilkControl : BreederEntity
    {
        public int? NumberTreatsPassage { get; set; }
        public bool? PresenceRobotTreats { get; set; }
    }
}