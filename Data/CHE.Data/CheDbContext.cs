namespace CHE.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CheDbContext : IdentityDbContext<CheUser, CheRole, string>
    {
        public CheDbContext(DbContextOptions<CheDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Cooperative> Cooperatives { get; set; }

        public DbSet<CheUserCooperative> UserCooperatives { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<JoinRequest> JoinRequests { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CheUserCooperative>()
                .HasKey(uc => new { uc.CheUserId, uc.CooperativeId });

            builder.Entity<Review>()
                .HasOne(r => r.Receiver)
                .WithMany(rc => rc.ReviewsReceived)
                .HasForeignKey(r => r.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Sender)
                .WithMany(s => s.ReviewsSent)
                .HasForeignKey(r => r.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.Receiver)
                .WithMany(r => r.JoinRequestsReceived)
                .HasForeignKey(jr => jr.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.Sender)
                .WithMany(s => s.JoinRequestsSent)
                .HasForeignKey(jr => jr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.Cooperative)
                .WithMany(s => s.JoinRequestsReceived)
                .HasForeignKey(jr => jr.CooperativeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasOne(s => s.Cooperative)
                .WithOne(c => c.Schedule)
                .HasForeignKey<Schedule>(s => s.CooperativeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Schedule>()
                .HasOne(s => s.Teacher)
                .WithOne(t => t.Schedule)
                .HasForeignKey<Schedule>(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Profile>()
                .HasOne(p => p.Owner)
                .WithOne(o => o.Profile)
                .HasForeignKey<Profile>(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Image>()
                .HasOne(i => i.Profile)
                .WithOne(p => p.Image)
                .HasForeignKey<Image>(i => i.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasOne(e => e.Schedule)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}