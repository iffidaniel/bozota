using Microsoft.Extensions.Configuration;
using Confluent.Kafka;


var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json");

var config = configBuilder.Build();
var playerName = config.GetValue<string>("PlayerName");
Console.WriteLine($"Connecting player {playerName}");


var playerTopic = config.GetValue<string>("KafkaPlayersTopic");
var gameStateTopic = config.GetValue<string>("KafkaGameStateTopic");


var kafkaConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddIniFile("kafka.properties").Build();

using (var producer = new ProducerBuilder<string, string>(
         kafkaConfig.AsEnumerable()).Build())
{
    Console.WriteLine("Trying to send message to ");
    producer.Produce(playerTopic, new Message<string, string> { Key = "Player1", Value = "moveRight" },
        (deliveryReport) =>
        {
            if (deliveryReport.Error.Code != ErrorCode.NoError)
            {
                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
            }
            else
            {
                Console.WriteLine($"Produced event to topic {playerTopic}");
            }
        });
    producer.Flush(TimeSpan.FromSeconds(10));
}