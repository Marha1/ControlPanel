﻿using BellManager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.DataBase.Config
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.StartTime)
                .IsRequired();

            builder.Property(l => l.EndTime)
                .IsRequired();

            builder.Property(l => l.IsActive)
                .HasDefaultValue(true);
        }
    }
}
