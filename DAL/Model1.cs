using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<ACTapply_T> ACTapply_T { get; set; }
        public virtual DbSet<ActivityT> ActivityT { get; set; }
        public virtual DbSet<activityTypeT> activityTypeT { get; set; }
        public virtual DbSet<ACTMember> ACTMember { get; set; }
        public virtual DbSet<adminT> adminT { get; set; }
        public virtual DbSet<baomingT> baomingT { get; set; }
        public virtual DbSet<dalogT> dalogT { get; set; }
        public virtual DbSet<OrderT> OrderT { get; set; }
        public virtual DbSet<VolIdentifyT> VolIdentifyT { get; set; }
        public virtual DbSet<volunteerT> volunteerT { get; set; }
        public virtual DbSet<zhuguanT> zhuguanT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<adminT>()
                .Property(e => e.sex)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<adminT>()
                .Property(e => e.resume)
                .IsUnicode(false);

            modelBuilder.Entity<VolIdentifyT>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<volunteerT>()
                .Property(e => e.Atelephone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<volunteerT>()
                .Property(e => e.email)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
