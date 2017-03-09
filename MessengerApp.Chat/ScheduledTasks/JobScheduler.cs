using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Chat.ScheduledTasks
{
    public class JobScheduler 
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<MessagesReaderFromServiceJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s => s.WithIntervalInSeconds(3).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))).Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
