using System.Data.Entity.ModelConfiguration;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Data.Mapping.Users
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(p => p.UserId);
            // Properties
            this.Property(p => p.EmailAddress)
                .HasMaxLength(50);
            this.Property(p => p.ContactNumber)
                .HasMaxLength(50);
            this.Property(p => p.Address1)
                .HasMaxLength(128);
            this.Property(p => p.Address2)
                .HasMaxLength(128);
            this.Property(p => p.TownCity)
                .HasMaxLength(50);
            this.Property(p => p.PostCodeZip)
                .HasMaxLength(50);
            this.Property(p => p.CountryRegion)
                .HasMaxLength(10);
            this.Property(p => p.Password)
                .HasMaxLength(50);
        }
    }
}
