using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace LatteMarche.SITRA
{
    public class ServiceStartUp
    {
        #region Fields

        private log4net.ILog log = log4net.LogManager.GetLogger("Service");
        private IScheduler scheduler;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public ServiceStartUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            //RepositoriesAutoFacConfig.Configure();

            log.Info("ServiceStartUp constructor");

        }

        #endregion

        #region Methods

        public void Start()
        {
            log.Info("Starting...");

            try
            {
                //AutoFacConfig.Configure();

                NameValueCollection properties = new NameValueCollection();

                // set thread pool info
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "1";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                // construct a scheduler factory
                ISchedulerFactory schedFact = new StdSchedulerFactory(properties);

                // get a scheduler
                this.scheduler = schedFact.GetScheduler();

                // read job config section and make jobs
                JobGroupSection jobConfigSection = (JobGroupSection)ConfigurationManager.GetSection("jobGroup");
                foreach (JobConfigElement config in jobConfigSection.Configs.Where(c => c.Enabled)) // discard disabled jobs
                {
                    Type type = Type.GetType(config.Type);
                    JobBuilder builder = JobBuilder.Create(Type.GetType(config.Type))
                        .WithDescription(config.Name);

                    builder.UsingJobData("name", config.Name);
                    builder.UsingJobData("logger", config.Name);

                    foreach (JobParamElement param in config.Params)
                    {
                        builder.UsingJobData(param.Key, param.Value);
                    }

                    IJobDetail job = builder.Build();
                    ITrigger trigger = null;

                    if (String.IsNullOrEmpty(config.Cron))
                    {
                        // default value
                        trigger = TriggerBuilder.Create()
                                                .WithIdentity(config.Name + "Trigger")
                                                .StartNow()
                                                .Build();
                    }
                    else
                    {
                        trigger = TriggerBuilder.Create()
                        .WithIdentity(config.Name + "Trigger")
                        .WithCronSchedule(config.Cron)
                        .Build();
                    }

                    this.scheduler.ScheduleJob(job, trigger);
                }

                this.scheduler.Start();
            }
            catch (Exception exc)
            {
                log.Error("Error during start.", exc);
            }


            log.Info("Started.");
        }

        public void Stop()
        {
            log.Info("Stopping...");
            this.scheduler.Shutdown();
            log.Info("Stopped.");
        }

        #endregion

    }
}