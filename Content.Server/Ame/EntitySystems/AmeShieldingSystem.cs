// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 LordCarve <27449516+LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Ame.Components;
using Content.Shared.Ame.Components;
using Robust.Server.GameObjects;

namespace Content.Server.Ame.EntitySystems;

public sealed class AmeShieldingSystem : EntitySystem
{
    [Dependency] private readonly AppearanceSystem _appearanceSystem = default!;
    [Dependency] private readonly PointLightSystem _pointLightSystem = default!;

    public void SetCore(EntityUid uid, bool value, AmeShieldComponent? shield = null)
    {
        if (!Resolve(uid, ref shield))
            return;
        if (value == shield.IsCore)
            return;

        shield.IsCore = value;
        _appearanceSystem.SetData(uid, AmeShieldVisuals.Core, value);
        if (!value)
            UpdateCoreVisuals(uid, 0, false, shield);
    }

    public void UpdateCoreVisuals(EntityUid uid, int injectionStrength, bool injecting, AmeShieldComponent? shield = null)
    {
        if (!Resolve(uid, ref shield))
            return;

        if (!injecting)
        {
            _appearanceSystem.SetData(uid, AmeShieldVisuals.CoreState, AmeCoreState.Off);
            _pointLightSystem.SetEnabled(uid, false);
            return;
        }

        _pointLightSystem.SetRadius(uid, Math.Clamp(injectionStrength, 1, 12));
        _pointLightSystem.SetEnabled(uid, true);
        _appearanceSystem.SetData(uid, AmeShieldVisuals.CoreState, injectionStrength > 2 ? AmeCoreState.Strong : AmeCoreState.Weak);
    }
}
