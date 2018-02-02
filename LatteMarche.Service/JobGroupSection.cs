using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Service
{

    /// <summary>
    /// Job Group Section
    /// </summary>
    public class JobGroupSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(JobConfigCollection), AddItemName = "jobConfig")]
        public JobConfigCollection JobConfigs
        {
            get { return (JobConfigCollection)this[""]; }
        }

        public List<JobConfigElement> Configs
        {
            get { return this.JobConfigs.Cast<JobConfigElement>().ToList(); }
        }

    }

    /// <summary>
    /// Job Config Collection
    /// </summary>
    public class JobConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobConfigElement)element).Name;
        }
    }

    /// <summary>
    /// Job Config Element
    /// </summary>
    public class JobConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = false)]
        public bool Enabled
        {
            get { return Convert.ToBoolean(this["enabled"]); }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("cron", DefaultValue = "0/15 * * * * ?", IsRequired = false)]
        public string Cron
        {
            get { return this["cron"].ToString(); }
            set { this["cron"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(JobParamCollection), AddItemName = "jobParam")]
        public JobParamCollection JobParams
        {
            get { return (JobParamCollection)this[""]; }
        }

        public List<JobParamElement> Params
        {
            get { return this.JobParams.Cast<JobParamElement>().ToList(); }
        }

    }

    /// <summary>
    /// Job Param Collection
    /// </summary>
    public class JobParamCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobParamElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobParamElement)element).Key;
        }
    }

    /// <summary>
    /// Job Param Element
    /// </summary>
    public class JobParamElement : ConfigurationElement
    {

        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (String)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (String)this["value"]; }
            set { this["value"] = value; }
        }

    }
}
