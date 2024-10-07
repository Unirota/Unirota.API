namespace Unirota.Infrastructure.Mapping;

public static class MapSettings
{
    public static void Configure()
    {
        UsuarioMapSettings.Configure();
        GrupoMapSettings.Configure();
    }
}
