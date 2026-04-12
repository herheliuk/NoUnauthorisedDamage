namespace Oxide.Plugins;

[Info("No Unauthorised Damage", "&anhe", "1.1.5")]
[Description("Prevents players from damaging other players’ belongings.")]
public class NoUnauthorisedDamage : RustPlugin
{
    private object OnEntityTakeDamage(BaseCombatEntity entity, HitInfo info) =>
        // Allow if
        (
            // World
            entity.OwnerID == 0 ||
            // Player’s
            (
                info.InitiatorPlayer is { } player &&
                (
                    // Yours
                    entity.OwnerID == player.userID ||
                    // Team’s
                    RelationshipManager.ServerInstance.FindTeam(player.currentTeam)
                        ?.members.Contains(entity.OwnerID) == true ||
                    // Authorised
                    entity.GetBuildingPrivilege()
                        ?.IsAuthed(player) == true ||
                    // Admin
                    player.IsAdmin
                )
            ) ||
            // Decay
            info?.damageTypes?.Has(Rust.DamageType.Decay) == true
        )
            ? null : false;
}