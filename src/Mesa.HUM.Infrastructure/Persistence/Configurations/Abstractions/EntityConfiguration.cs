namespace Mesa.HUM.Infrastructure.Persistence.Configurations.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [ExcludeFromCodeCoverage]
    internal abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public virtual void Configure ( EntityTypeBuilder<TEntity> builder )
        {
            MapKey ( builder );
            MapProperties ( builder );
            MapIndices ( builder );
            MapIgnoredProperties ( builder );
            MapOwnerships ( builder );
            MapRelationships ( builder );
        }

        public virtual void MapIgnoredProperties ( EntityTypeBuilder<TEntity> builder )
        {
        }

        public virtual void MapIndices ( EntityTypeBuilder<TEntity> builder )
        {
        }

        public abstract void MapKey ( EntityTypeBuilder<TEntity> builder );

        public virtual void MapOwnerships ( EntityTypeBuilder<TEntity> builder )
        {
        }

        public abstract void MapProperties ( EntityTypeBuilder<TEntity> builder );

        public virtual void MapRelationships ( EntityTypeBuilder<TEntity> builder )
        {
        }
    }
}