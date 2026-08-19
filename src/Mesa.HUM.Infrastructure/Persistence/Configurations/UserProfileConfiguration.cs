namespace Mesa.HUM.Infrastructure.Persistence.Configurations
{
    using System.Diagnostics.CodeAnalysis;
    using Mesa.HUM.Domain.Profiles;
    using Mesa.HUM.Domain.Profiles.ValueObjects;
    using Mesa.HUM.Infrastructure.Persistence.Configurations.Abstractions;
    using Mesa.HUM.Infrastructure.Persistence.Configurations.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [ExcludeFromCodeCoverage]
    internal class UserProfileConfiguration : EntityConfiguration<UserProfile>
    {
        public override void MapIgnoredProperties ( EntityTypeBuilder<UserProfile> builder )
        {
            base.MapIgnoredProperties ( builder );

            builder.Ignore ( x => x.DomainEvents );
        }

        public override void MapIndices ( EntityTypeBuilder<UserProfile> builder )
        {
            base.MapIndices ( builder );

            builder
                .HasIndex ( x => x.Id )
                .IsUnique ( true );
        }

        public override void MapKey ( EntityTypeBuilder<UserProfile> builder )
        {
            builder.HasKey ( x => x.Id );
        }

        public override void MapOwnerships ( EntityTypeBuilder<UserProfile> builder )
        {
            base.MapOwnerships ( builder );

            builder
                .OwnsOne ( p => p.ChppToken , chppToken =>
                {
                    chppToken
                        .Property ( t => t.Token )
                        .HasColumnType ( DataTypes.Text )
                        .HasMaxLength ( 36 )
                        .IsFixedLength ( true )
                        .IsRequired ( true );

                    chppToken
                        .Property ( t => t.TokenSecret )
                        .HasColumnType ( DataTypes.Text )
                        .HasMaxLength ( 36 )
                        .IsFixedLength ( true )
                        .IsRequired ( true );

                    chppToken
                        .Property ( t => t.ObtainedAt )
                        .HasColumnType ( DataTypes.Text )
                        .HasMaxLength ( 23 )
                        .IsFixedLength ( true )
                        .IsRequired ( true );

                    chppToken
                        .Property ( t => t.ExpiresAt )
                        .HasColumnType ( DataTypes.Text )
                        .HasMaxLength ( 23 )
                        .IsFixedLength ( true )
                        .IsRequired ( true );
                } );
        }

        public override void MapProperties ( EntityTypeBuilder<UserProfile> builder )
        {
            builder
                .Property ( x => x.Id )
                .HasColumnType ( DataTypes.Text )
                .HasConversion (
                    id => id.Value ,
                    value => new ProfileId ( value ) )
                .HasMaxLength ( 36 )
                .IsFixedLength ( true )
                .IsRequired ( true )
                .ValueGeneratedNever ( );

            builder
                .Property ( x => x.SynchronizedAt )
                .HasColumnType ( DataTypes.Text )
                .HasMaxLength ( 23 )
                .IsFixedLength ( true )
                .IsRequired ( false );
        }
    }
}