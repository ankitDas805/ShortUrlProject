using Azure.Messaging.ServiceBus;
using MongoDB.Driver;

public class ShortUrlService
{
    private readonly IMongoCollection<ShortUrl> collection;
    private readonly IConfiguration configuration;

    public ShortUrlService(IMongoClient client, IConfiguration configuration)
    {
        this.configuration = configuration;
        var dbName = configuration["MongoDBSettings:DatabaseName"];
        var dataBase = client.GetDatabase(dbName);

        collection = dataBase.GetCollection<ShortUrl>("urlmapping");
    }

    public async Task<string> Insert(string longUrl, string shortUrl)
    {

        var check = await collection.Find(x => x.longUrl == longUrl).FirstOrDefaultAsync();

        if (check != null)
        {
            return check.longUrl;
        }
        else
        {
            string shortenUrl = configuration.GetSection("Baseurl").Value + "/"+shortUrl;
            await collection.InsertOneAsync(new ShortUrl
            {
                longUrl = longUrl,
                clickCount = 0,
                createdAt = DateTime.Now,
                shortUrl = shortenUrl
            });

            return shortenUrl;
        }

    }

    public async Task<string> GetLongUrl(string shortUrl)
    {
        var response = await collection.Find(x => x.shortUrl == shortUrl).FirstOrDefaultAsync();

        if (response != null)
        {
            var filter = Builders<ShortUrl>.Filter.Where(x => x.shortUrl == shortUrl);

            var update = Builders<ShortUrl>.Update.Inc(x => x.clickCount, 1);
            var newClickCount = response.clickCount += 1;

            await collection.UpdateOneAsync(filter, update);
            return response.longUrl;
        }
        else
        {
            return string.Empty;
        }
    }
}