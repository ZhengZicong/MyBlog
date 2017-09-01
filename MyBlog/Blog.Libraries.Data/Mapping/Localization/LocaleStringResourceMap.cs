﻿using Blog.Libraries.Core.Domain.Localization;

namespace Blog.Libraries.Data.Mapping.Localization
{

    public class LocaleStringResourceMap : CustomEntityTypeConfiguration<LocaleStringResource>
    {

        public LocaleStringResourceMap()
        {
            this.ToTable("LocaleStringResource");
            this.HasKey(p => p.Id);
            this.Property(p => p.TimeStamp).IsRowVersion();
            this.Property(p => p.ResourceName).IsRequired().HasMaxLength(200);
            this.Property(p => p.ResourceValue).IsRequired();

            this.HasRequired(p => p.Language)
                .WithMany(p => p.LocaleStringResources)
                .Map(p => p.MapKey("Language_Id"));
        }

    }

}
