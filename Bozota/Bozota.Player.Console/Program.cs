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

// consumer code


CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // prevent the process from terminating.
    cts.Cancel();
};

var config1 = new ConsumerConfig
{
    GroupId = "test-consumer-group1",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
var config2 = new ConsumerConfig
{
    GroupId = "test-consumer-group2",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using (var consumer = new ConsumerBuilder<string, string>(config1).Build())
{
    using (var consumer2 = new ConsumerBuilder<string, string>(config2).Build())
    {


        consumer2.Subscribe(playerTopic);
        consumer.Subscribe(playerTopic);
        try
        {
            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                Console.WriteLine($"CONSUMER 1 event from topic {playerTopic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
                var cr2 = consumer2.Consume(cts.Token);
                Console.WriteLine($"CONSUMER 2 event from topic {playerTopic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
            }
        }
        catch (OperationCanceledException)
        {
            // Ctrl-C was pressed.
        }
        finally
        {
            consumer2.Close();
            consumer.Close();
        }
    }
}


// Producer code

//using (var producer = new ProducerBuilder<string, string>(
//         kafkaConfig.AsEnumerable()).Build())
//{
//Console.WriteLine("Trying to send message to ");
//producer.Produce(playerTopic, new Message<string, string> { Key = "Player1", Value = "moveRight" },
//    (deliveryReport) =>
//    {
//        if (deliveryReport.Error.Code != ErrorCode.NoError)
//        {
//            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
//        }
//        else
//        {
//            Console.WriteLine($"Produced event to topic {playerTopic}");
//        }
//    });
//producer.Flush(TimeSpan.FromSeconds(10));
//}






