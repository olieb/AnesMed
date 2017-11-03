using Microsoft.AspNet.Identity.EntityFramework;
using Przuchodnia_Medyczna_Inz.Models;
using System;
using System.Collections.Generic;
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
        public DbSet<PacjentChorobaPrzewlekla> ChorobaPacjenta { get; set; }
        public DbSet<ChorobaPrzewlekla> ChorobaPrzewlekla { get; set; }
        public DbSet<Specjalizacja> Specjalizacja { get; set; }
        public DbSet<Zatrudnienie> Zatrudnienie { get; set; }
        public DbSet<Stanowisko> Stanowisko { get; set; }
        public DbSet<Grafik> Grafik { get; set; }
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

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Osoba>()
                .HasRequired(x => x.Adres)
                .WithRequiredPrincipal(x => x.Osoba)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Wizyta>()
            //    .HasRequired(w => w.Pacjent)
            //    .WithMany(w => w.Wizyty);
        }
    }
}