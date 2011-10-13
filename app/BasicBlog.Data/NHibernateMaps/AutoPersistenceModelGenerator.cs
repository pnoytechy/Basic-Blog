using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using BasicBlog.Core;
using BasicBlog.Data.NHibernateMaps.Conventions;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace BasicBlog.Data.NHibernateMaps
{

    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {

        #region IAutoPersistenceModelGenerator Members

        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<Class1>(new AutomappingConfiguration())
                .Conventions.Setup(GetConventions())
                .IgnoreBase<Entity>()
                .IgnoreBase(typeof(EntityWithTypedId<>))
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        #endregion

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.ForeignKeyConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.HasManyConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.HasManyToManyConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.ManyToManyTableNameConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.PrimaryKeyConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.ReferenceConvention>();
                c.Add<BasicBlog.Data.NHibernateMaps.Conventions.TableNameConvention>();
            };
        }
    }
}
