using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CodeHollow.FeedReader;
using MongoDB.Driver;
using UpworkFeed.Data.Services;
using UpworkFeed.Core;
using UpworkFeed.Core.Options;
using UpworkFeed.Core.Models;
using UpworkFeed.Rss;
using UpworkFeed.Bot;
using UpworkFeed.Services;
using UpworkFeed.Services.Bot.Routes;
using UpworkFeed.Services.Bot;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var logging = builder.Logging;
var environment = builder.Environment;

environment.ContentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");

logging.AddConsole();

services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.OptionName));

services.AddSingleton<ITestInjection, TestInjection>();

services.AddSingleton<IUrlHelper, UrlHelper>();

var _dbConfig = config
    .GetSection(ApplicationOptions.OptionName)
    .Get<ApplicationOptions>()?.Database 
    ?? throw new ApplicationException($"{ApplicationOptions.OptionName} settings not provided");

var _mongoDatabase = new MongoClient(_dbConfig.ConnectionString).GetDatabase(_dbConfig.DbName);
services.AddSingleton<IMongoDatabase>(_mongoDatabase);

services.AddTransient<IUserProvider>(s =>
    new UserProvider(s.GetService<IMongoDatabase>(), _dbConfig.UserCollectionName));
services.AddTransient<IFeedProvider>(s => 
    new FeedProvider(s.GetService<IMongoDatabase>(), _dbConfig.FeedCollectionName));

services.AddSingleton<IConverter<string, FeedItem, JobOffer>, 
    CachedConverter<string, FeedItem, JobOffer, JobOfferConverter>>();

services.AddSingleton<IBot, Bot>();
services.AddSingleton<StartRoute, StartRoute>();

services.AddTransient<IFeedRequest<JobOffer>, FeedRequest<JobOffer>>();

services.AddHostedService<BotService>();

using IHost host = builder.Build();
await host.RunAsync();
