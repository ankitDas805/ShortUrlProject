using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class ShortUrl
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get; set;}

    public string longUrl {get; set;}
    public string shortUrl {get; set;}
    public DateTime createdAt {get; set;}

    public int clickCount {get; set;}
}