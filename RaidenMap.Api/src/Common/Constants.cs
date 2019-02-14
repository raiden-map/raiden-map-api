namespace RaidenMap.Api.src.Common
{
    public static class Constants
    {
        public const string MongoDbConnectionString = nameof(MongoDbConnectionString);

        public const string DatabaseName = nameof(DatabaseName);
        public const string TokenNetworkCollectionName = nameof(TokenNetworkCollectionName);
        public const string TokenNetworkAggregateCollectionName = nameof(TokenNetworkAggregateCollectionName);
        public const string RaidenCollectionName = nameof(RaidenCollectionName);
        public const string RaidenAggregateCollectionName = nameof(RaidenAggregateCollectionName);
        
        public const int TimeStampDelta = 30000; // Milliseconds
    }
}
