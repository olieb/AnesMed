using Microsoft.AspNet.Identity.EntityFramework;
using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Przuchodnia_Medyczna_Inz.DAL
{
    public class PrzychodniaContext : DbContext
    {
        public PrzychodniaContext()
            : base("PrzychodniaContext")
        {
            Database.SetInitializer<PrzychodniaContext>(new DropCreateDatabaseIfModelChanges<PrzychodniaContext>());
        }
        public DbSet<Osoba> Osoba { get; set; }
        public DbSet<Adres> Adres { get; set; }
        public DbSet<Wizyta> Wizyta { get; set; }
        public DbSet<Stanowisko> Stanowisko { get; set; }
        public DbSet<PacjentChorobaPrzewlekla> ChorobaPacjenta { get; set; }
        public DbSet<ChorobaPrzewlekla> ChorobaPrzewlekla { get; set; }
        public DbSet<Specjalizacja> Specjalizacja { get; set; }
        public DbSet<PlacowkaMedyczna> PlacowkaMedyczna { get; set; }
        public DbSet<Pracownik> Pracownik { get; set; }
        public DbSet<Pacjent> Pacjent { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<Pacjent>().ToTable("Pacjent");
            modelBuilder.Entity<Pracownik>().ToTable("Pracownik");

            modelBuilder.Entity<Adres>().HasKey(x => x.AdresID).Property(x => x.AdresID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Adres>().HasOptional(x => x.PlacowkaMedyczna).WithOptionalDependent(a => a.Adres);

            modelBuilder.Entity<PlacowkaMedyczna>().HasKey(x => x.PlacowkaMedycznaID).Property(x => x.PlacowkaMedycznaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
               
        }
    }
}