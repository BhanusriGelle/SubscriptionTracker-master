using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SubscriptionTracker.Models
{
    public class SubTrackerContext:DbContext
    {
        public SubTrackerContext()
            :base("name=SubTrackerConnection")
        {

        }
        public DbSet<User> UsersTable { get; set; }
        public DbSet<Service> ServicesTable { get; set; }

    }
}