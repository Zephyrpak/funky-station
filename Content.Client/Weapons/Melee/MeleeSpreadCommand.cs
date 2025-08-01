// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.CombatMode;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Console;
using Robust.Shared.Map;

namespace Content.Client.Weapons.Melee;


public sealed class MeleeSpreadCommand : IConsoleCommand
{
    public string Command => "showmeleespread";
    public string Description => "Shows the current weapon's range and arc for debugging";
    public string Help => $"{Command}";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var collection = IoCManager.Instance;

        if (collection == null)
            return;

        var overlayManager = collection.Resolve<IOverlayManager>();

        if (overlayManager.RemoveOverlay<MeleeArcOverlay>())
        {
            return;
        }

        var sysManager = collection.Resolve<IEntitySystemManager>();

        overlayManager.AddOverlay(new MeleeArcOverlay(
            collection.Resolve<IEntityManager>(),
            collection.Resolve<IEyeManager>(),
            collection.Resolve<IInputManager>(),
            collection.Resolve<IPlayerManager>(),
            sysManager.GetEntitySystem<MeleeWeaponSystem>(),
            sysManager.GetEntitySystem<SharedCombatModeSystem>(),
            sysManager.GetEntitySystem<SharedTransformSystem>()));
    }
}
