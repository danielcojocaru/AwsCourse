using Amazon.SQS;
using Customers.Consumer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(nameof(QueueSettings)));
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.Run();