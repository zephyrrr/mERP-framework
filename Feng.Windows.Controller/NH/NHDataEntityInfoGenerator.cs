using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    public class NHDataEntityMetadataGenerator : IEntityMetadataGenerator
    {
        public IEntityMetadata GenerateEntityMetadata(object entityParam)
        {
            try
            {
                if (entityParam is Type)
                {
                    Type type = (Type)entityParam;
                    var sf =  Feng.NH.NHibernateHelper.GetSessionFactory(type);
                    if (sf == null)
                    {
                        var sfm = ServiceProvider.GetService<ISessionFactoryManager>();
                        sf = sfm.GetSessionFactory(Feng.Utils.RepositoryHelper.GetConfigNameFromType(type));
                    }
                    return Feng.NH.TypedEntityMetadata.GenerateEntityInfo(sf, type);
                }
                else if (entityParam is Feng.Data.SearchManager)
                {
                    return new UntypedEntityMetadata((entityParam as Feng.Data.SearchManager).TableName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
