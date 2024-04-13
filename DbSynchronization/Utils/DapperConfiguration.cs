using System.Reflection;
using Dapper;

namespace DbSynchronization.Utils;

public static class DapperConfiguration
{
    public static void ConfigureSnakeCaseMapping()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach (var modelType in assembly.GetTypes())
        {
            SqlMapper.SetTypeMap(
                modelType,
                new CustomPropertyTypeMap(
                    modelType,
                    (type, columnName) =>
                        type.GetProperties()
                            .FirstOrDefault(prop =>
                                prop.Name.ToLower() == columnName.Replace("_", "").ToLower()
                            )!
                )
            );
        }
    }
}