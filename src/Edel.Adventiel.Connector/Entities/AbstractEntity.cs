﻿using Autumn.Mvc.Data.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Edel.Adventiel.Connector.Entities
{

    public abstract class AbstractEntity
    {
        [Ignore] public Metadata Metadata { get; set; }
    }
}