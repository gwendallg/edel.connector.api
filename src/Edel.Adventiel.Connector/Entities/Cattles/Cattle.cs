﻿using Autumn.Mvc.Data.Annotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Adventiel.Connector.Entities.Cattles
{
    [BsonIgnoreExtraElements]
    [Entity(Name = "cattle", Insertable = false, Updatable = false, Deletable = false)]
    public class Cattle : AbstractCattleData
    {
        public CattleIdentity Identity { get; set; }
    }
}