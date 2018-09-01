﻿using Autumn.Mvc.Data.Annotations;
using Autumn.Mvc.Data.MongoDB.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Connector.Models.Breeders
{
    [BsonIgnoreExtraElements]
    [Collection(Name = "breeder")]
    [Resource()]
    public class Breeder : BreederEntity
    {
    }
}