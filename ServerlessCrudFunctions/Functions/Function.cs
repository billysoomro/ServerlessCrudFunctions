using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using ServerlessCrudFunctions.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ServerlessCrudFunctions.Functions;

public class Function
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDynamoDBContext _dynamoDBContext;

    public Function()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _dynamoDBContext = _serviceProvider.GetRequiredService<IDynamoDBContext>();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var awsConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.EUWest2,
        };
        var dynamoDbClient = new AmazonDynamoDBClient(awsConfig);

        services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
    }

    /// <summary>
    /// A simple function that retrieves a list of guitars objects from a DynamoDb table
    /// </summary>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<List<Guitar>> Get(ILambdaContext context)
    {
        var conditions = new List<ScanCondition>();
        var guitars = await _dynamoDBContext.ScanAsync<Guitar>(conditions).GetRemainingAsync();

        if (guitars == null)
        {
            return null;
        }

        return guitars;
    }

    /// <summary>
    /// A simple function that retrieves a specific guitar object from a DynamoDb table
    /// </summary>
    /// <param name="guitar">The guitar object to persist to the database</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<Guitar> GetSingle(int id, ILambdaContext context)
    {
        var guitar = await _dynamoDBContext.LoadAsync<Guitar>(id);

        if (guitar == null)
        {
            return null;
        }

        return guitar;
    }

    /// <summary>
    /// A simple function that posts a guitar object to a DynamoDb table
    /// </summary>
    /// <param name="guitar">The guitar object to persist to the database</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<Guitar> Post(Guitar guitar, ILambdaContext context)
    {
        await _dynamoDBContext.SaveAsync(guitar);

        return guitar;
    }

    /// <summary>
    /// A simple function that updates a specific guitar object in a DynamoDb table
    /// </summary>
    /// <param name="guitar">The guitar object to persist to the database</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<Guitar> Put(Guitar guitar, ILambdaContext context)
    {
        await _dynamoDBContext.SaveAsync(guitar);

        return guitar;
    }

    /// <summary>
    /// A simple function that deletes a specific guitar object in a DynamoDb table
    /// </summary>
    /// <param name="guitar">The guitar object to persist to the database</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<string> Delete(int id)
    {
        await _dynamoDBContext.DeleteAsync<Guitar>(id);

        return $"Guitar {id} Deleted";
    }
}
