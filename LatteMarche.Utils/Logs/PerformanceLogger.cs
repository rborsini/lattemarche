using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace LatteMarche.Utils.Logs
{
    public class PerformanceLogger
    {
        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        private ILog logger;
        private Dictionary<string, Stopwatch> watchers;

        public PerformanceLogger(ILog logger)
        {
            this.logger = logger;
            this.watchers = new Dictionary<string, Stopwatch>();
        }

        public void Start(string watcher)
        {
            if (!this.watchers.ContainsKey(watcher))
                this.watchers.Add(watcher, new Stopwatch());

            this.watchers[watcher].Restart();
        }

        public void Stop(string watcher, Level level = Level.Debug, int maxDelay = 0)
        {
            if (this.watchers.ContainsKey(watcher))
            {
                this.watchers[watcher].Stop();

                var elapsed = this.watchers[watcher].Elapsed;

                if (elapsed.TotalSeconds > maxDelay)
                {
                    try
                    {
                        switch (level)
                        {
                            case Level.Debug:
                                this.logger.Debug($"{watcher} ({elapsed})");
                                break;
                            case Level.Info:
                                this.logger.Info($"{watcher} ({elapsed})");
                                break;
                            case Level.Error:
                                this.logger.Error($"{watcher} ({elapsed})");
                                break;
                            case Level.Warn:
                                this.logger.Warn($"{watcher} ({elapsed})");
                                break;
                            case Level.Fatal:
                                this.logger.Fatal($"{watcher} ({elapsed})");
                                break;
                        }
                    }
                    catch (Exception exc)
                    {
                        this.logger.Error(exc.Message);
                    }

                }
            }
        }

    }
}
