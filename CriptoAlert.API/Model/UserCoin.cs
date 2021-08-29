using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CriptoAlert.API.Model
{
    public class UserCoin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("userEmail")]
        public string userEmail { get; set; }

        [BsonElement("coinName")]
        public string coinName { get; set; }
        [BsonElement("coinTag")]
        public string coinTag { get; set; }

        [BsonElement("minValue")]
        public double minValue { get; set; }

        [BsonElement("maxValue")]
        public double maxValue { get; set; }

        [BsonElement("notificateMin")]
        public int notificateMin { get; set; }

        [BsonElement("notificateMax")]
        public int notificateMax { get; set; }
        [BsonElement("validUser")]
        public bool validUser { get; set; }
    }
}
