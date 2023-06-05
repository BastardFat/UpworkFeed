using MongoDB.Driver;

namespace UpworkFeed.Data.Services;

public abstract class BaseProvider<T>
{
    private readonly IMongoDatabase _database;
    private readonly string _collectionName;

    public BaseProvider(IMongoDatabase database, string collectionName)
    {
        _database = database;
        _collectionName = collectionName;
    }

    protected IMongoCollection<T> Collection => _database.GetCollection<T>(_collectionName);
    protected FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;
    protected UpdateDefinitionBuilder<T> Update => Builders<T>.Update;
}
