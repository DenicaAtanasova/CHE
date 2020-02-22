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

        public DbSet<VCard> VCards { get; set; }

        public DbSet<VCardGrade> VCardGrades { get; set; }

        public DbSet<JoinRequest> JoinRequests { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CheUserCooperative>()
                .HasKey(uc => new { uc.CheUserId, uc.CooperativeId });

            builder.Entity<VCardGrade>()
                .HasKey(vcg => new { vcg.VCardId, vcg.GradeId });

            builder.Entity<Review>()
                .HasOne(r => r.Receiver)
                .WithMany(rc => rc.ReceivedReviews)
                .HasForeignKey(r => r.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Sender)
                .WithMany(s => s.SentReviews)
                .HasForeignKey(r => r.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.CoopReceiver)
                .WithMany(r => r.ReceivedJoinRequests)
                .HasForeignKey(jr => jr.CoopReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.CoopSender)
                .WithMany(s => s.SentJoinRequests)
                .HasForeignKey(jr => jr.CoopSenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.TeacherReceiver)
                .WithMany(r => r.ReceivedJoinRequests)
                .HasForeignKey(jr => jr.TeacherReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JoinRequest>()
                .HasOne(jr => jr.ParentSender)
                .WithMany(p => p.SentJoinRequests)
                .HasForeignKey(jr => jr.ParentSenderId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}