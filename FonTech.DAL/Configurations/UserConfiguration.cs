using FonTech.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Login).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired();

            builder.HasMany<Report>(x => x.Reports)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id);

            builder.HasData(new List<User>()
            {
               new User()
               {
                 Id = 1,
                 Login = "WindTalkers",
                 Password = new string('+',7),
                 CreatedAt = DateTime.UtcNow,

               },
               new User() 
               {
                Id = 2,
                Login = "WindTalkers2",
                Password = new string('-',8),
                CreatedAt = DateTime.UtcNow,
               }
            }) ;
        }
    }
}
