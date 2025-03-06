using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Messaging.ServiceBus;
using Gdot.GFT.Common.Services.ProductConfiguration;
using Gdot.GFT.Common.Services.ProductConfiguration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MyConfigClient.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController(IConfiguration configuration, IProductConfigurationContext configurationContext, IServiceProvider serviceProvider) : ControllerBase
{

    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly IProductConfigurationContext _configurationContext = configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    [HttpPost]
    public async Task<ActionResult> Publish()
    {
        var productConfigurationSettings = _configuration.GetSection("ProductConfiguration").Get<ProductConfigurationSettings>()
            ?? throw new InvalidOperationException("ProductConfiguration settings not configured properly.");
        var topicName = productConfigurationSettings.ServiceBus.TopicName
            ?? throw new InvalidOperationException("ServiceBus.TopicName not configured properly.");

        await using (var client = new ServiceBusClient(productConfigurationSettings.ServiceBus.ConnectionString))
        {
            ServiceBusSender sender = client.CreateSender(topicName);
            foreach (var i in Enumerable.Range(1, 100))
            {
                var message = GetConfigurationMessage(i);
                var messageBody = JsonSerializer.Serialize(message);
                var serviceBusMessage = new ServiceBusMessage(messageBody)
                {
                    ContentType = "application/json"
                };

                await sender.SendMessageAsync(serviceBusMessage);
            }
        }

        return Ok();
    }

    [HttpGet("Value")]
    public ActionResult GetConfigurationValue()
    {
        //var productConfigurationSettings = _serviceProvider.GetRequiredService<IOptions<ProductConfigurationSettings>>().Value;

        //TabapayOptions tabapayOptions = new TabapayOptions
        //{ 
        //    SourceAccountID = "123",
        //    SubClientID = "456",
        //    RetryMaxTimeInMs = 1000
        //};
        //var option = JsonSerializer.Serialize(tabapayOptions);

        //var dic = new Dictionary<string, object>()
        //{
        //    ["Value"] = tabapayOptions
        //};
        //var data1 = JsonSerializer.Serialize(dic);

        //var test = _configuration["ProductConfiguration"]?.ToString();
        var data = JsonSerializer.Serialize(_configurationContext.Configurations![("tpg","fundingProcessor")]);
        return Ok(data);
    }

    private static object GetConfigurationMessage(int i)
    {
        return new
        {
            Type = "fundingProcessor",
            ProgramCode = "intuitqb",
            PartnerId = $"87dc60c4-714e-47da-a8b8-a351d14f4618",
            ComponentName = "ECTPush",
            Meta = new
            {
                CreateTime = "2024-10-21T18:25:43.511Z",
                UpdateTime = "2024-10-21T18:25:43.511Z",
                Version = "1.0",
                ChangedBy = $"User name {i}",
                CreatedBy = "Some original writer"
            },
            Value = new
            {
                Priority = "MCSendProcessor",
                MCSendProcessor = new[]
                {
                    new
                    {
                        PartnerId = $"ptnr_R0CPnJjHXDP1prH-pGtnuRrWYHb",
                        PaymentType = "BDB",
                        TransferType = new[] { "SingleCommitDisbursementExternal" },
                        CardAccepterId = "000000000GDC028",
                        Statementdescriptor = "Amazon flex Payout"
                    }
                },
                VisaDirectProcessor = new[]
                {
                    new
                    {
                        TransactionDescription = "Amazon Flex Payout",
                        TransferType = new[] { "SingleCommitDisbursementExternal" },
                        PanEntryMode = "01",
                        PosConditionCode = "59",
                        MotoECIIndicator = "7",
                        CardAcceptorCountry = "USA",
                        CardAcceptorZipCode = "98109",
                        CardAcceptorCounty = "Seattle",
                        CardAcceptorState = "WA",
                        IdCode = "051900002",
                        Name = "Amazon Digital",
                        TerminalId = "1001",
                        MerchantCategoryCode = "4215",
                        MerchantPseudoAbaNumber = "840044307",
                        SenderName = "Amazon",
                        SenderAddress = "410 Terry Ave N",
                        SenderCity = "Seattle",
                        SenderStateCode = "WA",
                        SenderPostalCode = "98109",
                        SenderCountryCode = "840",
                        SourceOfFundsCode = "05",
                        AcquirerCountryCode = "840",
                        AcquiringBin = "424817",
                        BusinessApplicationIdForDisbursement = "FD",
                        BusinessApplicationIdForA2A = "AA",
                        TransactionCurrencyCode = "840",
                        MvvAcquirerAssigned = "0000",
                        MvvVisaAssigned = "200559"
                    }
                },
                BaaSSettings = new[]
                {
                    new
                    {
                        AdjustmentDescription = "Amazon flex Payout",
                        TransferType = new[] { "SingleCommitDisbursementExternal", "SingleCommitDisbursementExternal2" }
                    }
                },
                NecSettings = new[]
                {
                    new
                    {
                        TransactionDescription = "Amazon flex Payout",
                        TransferType = new[] { "SingleCommitDisbursementExternal" }
                    }
                },
                Level1 = new[]
                {
                    new
                    {
                        Level2 =new[]
                        {
                            new
                            {
                                Level3 = "Value of Level 3"                                
                            }
                        }
                    }
                }
            }
        };
    }
}


public class TabapayOptions
{
    [JsonPropertyName("sourceAccountID")]
    public required string SourceAccountID { get; set; }
    [JsonPropertyName("subClientID")]
    public required string SubClientID { get; set; }
    [JsonPropertyName("retryMaxTimeInMs")]
    public int RetryMaxTimeInMs { get; set; }
}
