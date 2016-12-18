using Superior.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace Superior.DataAccess.Configurations
{
    public static class UserConfiguration
    {
        public static void SetUserConfiguration(this DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(t => t.UserName)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_UserName", 1) { IsUnique = true }));
        }
    }
}
