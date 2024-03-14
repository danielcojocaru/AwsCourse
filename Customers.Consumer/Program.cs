using Amazon.SQS;
using Customers.Consumer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection(nameof(AwsSettings)));
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.Run();