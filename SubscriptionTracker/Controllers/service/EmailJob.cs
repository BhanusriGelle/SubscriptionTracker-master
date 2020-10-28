using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using Quartz;
using Quartz.Impl;
using SubscriptionTracker.Models;


namespace SubscriptionTracker.Controllers.service
{
    public class EmailJob : IJob
    {
        public void Execute(Quartz.IJobExecutionContext context)
        {

            //message.Subject = "Test";
            //message.Body = "Test at " + DateTime.Now;
            //using (SmtpClient client = new SmtpClient
            //{
            //    EnableSsl = true,
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    Credentials = new NetworkCredential("user@gmail.com", "password")
            //})
            //{
            //    client.Send(message);
            //}

            //find emails with due date
            using (SubTrackerContext dc = new SubTrackerContext())
            {
                //var v = dc.ServicesTable.Where(a => a.EndDate == DateTime.Now.AddDays(1).Date).FirstOrDefault();
                DateTime tommorow = DateTime.Now.AddDays(1).Date;
                var userMail = from a in dc.ServicesTable.Where(a => a.EndDate == tommorow)
                               select a.User.EmailId;

                if(userMail != null) { 
                    foreach (var email in userMail)
                    {

                
                        var from = new MailAddress("trackersubscription@gmail.com", "SubTracker");
                        var to = new MailAddress(email);
                        var frompw = "subscriptionTracker123";
                        string sub = "Renew your Subscription";
                        string body = "<br/><br/>Please renew your subscription, as it will expire tommorow inorder to continue using the service - <strong>SubTracker</strong>" +
                                    "<br/><br/> Click this link to go to your account.";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(from.Address, frompw)


                        };
                        using (var message = new MailMessage(from, to))
                        {
                            message.Subject = sub;
                            message.Body = body;
                            message.IsBodyHtml = true;

                            smtp.Send(message);

                        }
                    }
                }
            }

        }
    }

    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInHours(24)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            //      )
            //    .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}