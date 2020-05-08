namespace CHE.Services.Mapping
{
    using System;

    public static class EntityMappingExtensions
    {
        public static TDestination Map<TSource, TDestination>(
            this TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AutoMapperConfig.MapperInstance.Map<TSource, TDestination>(source);
        }
    }
}