using MassTransit;
using RMQMessage;
using System;
using System.Collections.Generic;

namespace RMQProducer
{
    class Program
    {
        static IBusControl InitializeBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        }

        static NewsletterMailMessage InitializeMessage()
        {
            var message = new NewsletterMailMessage
            {
                AddressList = new List<string>() { "test@test.com" },
                NewsLetter = new NewsletterModel
                {
                    MailSubject = "Daily Newsletter of Contoso Corp.",
                    HtmlContent = "Lorem ipsum dolor sit amet, et nam mucius docendi hendrerit, an usu decore mandamus. Ei qui quod decore, cum nulla nostrud erroribus ut, est eu aperiri interesset. Legere mentitum per an. Hinc legimus nostrum cu vix."
                },
                PublishedTime = DateTime.Now,
                QueueId = Guid.NewGuid()
            };

            Console.WriteLine("Message published !\nSubject : " + message.NewsLetter.MailSubject + "\nPublished at : " + message.PublishedTime + "\nQueueId : " + message.QueueId);
            return message;
        }

        static void Main(string[] args)
        {
            var busControl = InitializeBus();
            busControl.Start();
            Console.WriteLine("Started publishing.");

            while (!Console.KeyAvailable)
            {
                var message = InitializeMessage();
                busControl.Publish(message);
            }
        }


    }
}
