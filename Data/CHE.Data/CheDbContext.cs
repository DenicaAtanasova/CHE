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

        public DbSet<Parent> Parents { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Cooperative> Cooperatives { get; set; }

        public DbSet<ParentCooperative> ParentsCooperatives { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<JoinRequest> JoinRequests { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Messenger> Messengers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessengerUser> MessengersUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ParentCooperative>()
                .HasKey(uc => new { uc.ParentId, uc.CooperativeId });

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
                .HasOne(jr => jr.Sender)
                .WithMany(s => s.JoinRequestsSent)
                .HasForeignKey(jr => jr.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasOne(s => s.Owner)
                .WithOne(t => t.Schedule)
                .HasForeignKey<Schedule>(s => s.OwnerId)
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

            builder.Entity<MessengerUser>()
                .HasKey(mu => new { mu.MessengerId, mu.UserId });

            builder.Entity<Messenger>()
                .HasOne(m => m.Cooperative)
                .WithOne(c => c.Messenger)
                .HasForeignKey<Messenger>(m => m.CooperativeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Messenger)
                .WithMany(msg => msg.Messages)
                .HasForeignKey(m => m.MessengerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Parent>()
                .HasOne(p => p.User)
                .WithOne(u => u.Parent)
                .HasForeignKey<Parent>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }
}