using MongoDB.Driver;

public class ShortUrlService
{
    private readonly IMongoCollection<ShortUrl> collection;

    public ShortUrlService(IMongoClient client,IConfiguration configuration)
    {
        var dbName = configuration["MongoDBSettings:DatabaseName"];
        var dataBase = client.GetDatabase(dbName);

        collection = dataBase.GetCollection<ShortUrl>("urlmapping");
    }

    public async Task<bool> Insert(string longUrl,string shortUrl)
    {
        await collection.InsertOneAsync(new ShortUrl
        {
            longUrl = longUrl,
            clickCount = 0,
            createdAt = DateTime.Now,
            shortUrl = shortUrl
        });

        return true;
    }

    public async Task<string> GetLongUrl(string shortUrl)
    {
        var response =  await collection.Find(x=>x.shortUrl == shortUrl).FirstOrDefaultAsync();

        if(response != null)
        {
            var filter = Builders<ShortUrl>.Filter.Where(x=>x.shortUrl == shortUrl);

            var update = Builders<ShortUrl>.Update.Inc(x=>x.clickCount,1);
            var newClickCount =  response.clickCount += 1;

            await collection.UpdateOneAsync(filter,update);
            return response.longUrl;
        }
        else
        {
            return string.Empty;
        }
    }
}