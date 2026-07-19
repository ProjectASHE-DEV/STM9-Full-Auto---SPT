using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STM9FullAuto;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.mrlahey.stm9fullauto";
    public override string Name { get; init; } = "STM-9 Full Auto Conversion";
    public override string Author { get; init; } = "MrLahey";
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("4.0.13");
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class STM9Mod(
    ISptLogger<STM9Mod> logger,
    DatabaseService databaseService)
    : IOnLoad
{
    public Task OnLoad()
    {
        string stm9Id = "60339954d62c9b14ed777c06";

        var items = databaseService.GetTables().Templates.Items;

        if (items.TryGetValue(stm9Id, out var stm9))
        {
            stm9.Properties.WeapFireType.Add("fullauto");
            stm9.Properties.BFirerate = 750;

            logger.Success("[STM9-FullAuto] SUCCESS: Converted STM-9 to a Fully Automatic 9x19 laserbeam (Subject to user skill)");
        }
        else
        {
            logger.Error("[STM9-FullAuto] ERROR: Could not find STM-9 ID in the database.");
        }

        return Task.CompletedTask;
    }
}
