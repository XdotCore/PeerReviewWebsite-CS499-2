﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeerReviewWebsite.Classes.Data.Account;
using PeerReviewWebsite.Classes.Data.Review;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeerReviewWebsite.Classes.Data {
    /// <summary>
    /// Represents the entirety of the websites database
    /// </summary>
    public class WebsiteDbContext(DbContextOptions options) : DbContext(options) {
        /// <summary>
        /// The users in the database
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// The roles in the database
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// The documents in the database
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// The comments in the database
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        static string IdsToString(HashSet<int> ids) => string.Join(',', ids);

        static HashSet<int> StringToIds(string str) => new(str.Split(',').Select(int.Parse));

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ValueConverter<HashSet<int>, string> idsAsString = new(
                ids => IdsToString(ids),
                str => StringToIds(str));

            // Add the Users table
            modelBuilder.Entity<User>(entity => {
                entity.ToTable("Users");
                entity.HasKey(user => user.Id);
                entity.Property(user => user.FirstName);
                entity.Property(user => user.LastName);
                entity.Property(user => user.Email);
                entity.Property(user => user.Password);
                entity.Property(user => user.Roles).HasConversion(idsAsString);
                entity.Property(user => user.OwnedDocuments).HasConversion(idsAsString);
                entity.Property(user => user.OwnedComments).HasConversion(idsAsString);
            });

            // Add the Roles table
            modelBuilder.Entity<Role>(entity => {
                entity.ToTable("Roles");
                entity.HasKey(role => role.Id);
                entity.Property(role => role.Name);
                entity.Property(role => role.Permissions);
            });

            // Add the Documents table
            modelBuilder.Entity<Document>(entity => {
                entity.ToTable("Documents");
                entity.HasKey(doc => doc.Id);
                entity.Property(doc => doc.Status);
                entity.Property(doc => doc.FileName);
            });

            // Add the Comments table
            modelBuilder.Entity<Comment>(entity => {
                entity.ToTable("Comments");
                entity.HasKey(comment => comment.Id);
                entity.Property(comment => comment.Status);
                entity.Property(comment => comment.Content);
            });
        }
    }
}
