/// I see that the mod upload page requires me to be able to explain what each part of this code does. 

using SPTarkov.DI.Annotations;              ///
using SPTarkov.Server.Core.DI;              ///
using SPTarkov.Server.Core.Models.Spt.Mod;  ///
using SPTarkov.Server.Core.Models.Utils;    /// This entire block simply allows the code below it to call from whichever library it needs,
using SPTarkov.Server.Core.Services;        /// Otherwise functions like stm9.Properties.BFirerate = 750; will not function, as the code does
using System.Collections.Generic;           /// not know what that is actually referencing.
using System.Linq;                          ///
using System.Threading.Tasks;               ///

namespace STM9FullAuto;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.mrlahey.stm9fullauto";                    ///
    public override string Name { get; init; } = "STM-9 Full Auto Conversion";                     ///
    public override string Author { get; init; } = "MrLahey";                                      ///
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");              ///
    public override SemanticVersioning.Range SptVersion { get; init; } = new("4.0.13");            /// This entire block contains parity information such as Author, Mod GUID, Name, Version, etc.
    public override List<string>? Contributors { get; init; }                                      /// This area was entirely copy and pasted from the following file. https://github.com/sp-tarkov/server-mod-examples/blob/main/2EditDatabase/EditDatabaseValues.cs
    public override List<string>? Incompatibilities { get; init; }                                 ///
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }   ///
    public override string? Url { get; init; }                                                     ///
    public override bool? IsBundleMod { get; init; } = false;                                      ///
    public override string License { get; init; } = "MIT";                                         ///
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)] ///
public class STM9FAMod(                                      /// This Block just tells the server to load the moad after the initial mod loader check, alsongside this it
    ISptLogger<STM9FAMod> logger,                            /// 
    DatabaseService databaseService)                         /// It also initializes the logging feature used later on (ISptLogger<STM9Mod> logger), and the Database Service that the mod uses to edit the table entry. (DatabaseService databaseService)   
    : IOnLoad                                                ///
{
    public Task OnLoad()     /// This line simply says "Hey, lets go ahead and do the following"
    {
        string stm9Id = "60339954d62c9b14ed777c06";                 ///This is the 'item id' for the STM-9 as it is in the item list. 
        var items = databaseService.GetTables().Templates.Items;    ///This line shows the specific table we will be looking in to find the above ID
        if (items.TryGetValue(stm9Id, out var stm9))                ///This line references the ID, and if it finds it the code can actually progress to the actual editing.
        {
            stm9.Properties.WeapFireType.Add("fullauto");                                                                                 ///This line adds the full auto property to the specific database line for the STM-9
            stm9.Properties.BFirerate = 750;                                                                                              ///STM-9 go brrrr
            logger.Success("[STM9FullAuto] Mod Loaded: Converted STM-9 to a Fully Automatic 9x19 laserbeam (Subject to user skill)");     ///This line outputs the success message to the console, and shows the mod has successfully edited the STM-9
        }
        else                                                                                            /// This section works in tandem with the above 'if' statement (if (items.TryGetValue(stm9Id, out var stm9). If the mod cant find this item, for whatever reason, it will instead stop loading the mod to prevent a null reference crash.
        {                                                                                               /// 
            logger.Error("[STM9FullAuto] Mod Load Error: Could not find STM-9 ID in the database.");    /// and it outputs this to the console.
        }

        return Task.CompletedTask;     ///The mod is loaded, progress to the next one.
    }
}
