// SPDX-FileCopyrightText: 2025 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 2025 marc-pelletier <113944176+marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server._Funkystation.Atmos.Reactions;

[UsedImplicitly]
public sealed partial class ProtoNitrateHydrogenConversionReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        if (mixture.Temperature > 20f && mixture.GetMoles(Gas.HyperNoblium) >= 5f)
            return ReactionResult.NoReaction;

        var initPN = mixture.GetMoles(Gas.ProtoNitrate);
        var initH2 = mixture.GetMoles(Gas.Hydrogen);

        var producedAmount = Math.Min(Atmospherics.ProtoNitrateHydrogenConversionMaxRate, Math.Min(initH2, initPN));

        if (producedAmount <= 0 || initH2 - producedAmount < 0f)
            return ReactionResult.NoReaction;

        mixture.AdjustMoles(Gas.Hydrogen, -producedAmount);
        mixture.AdjustMoles(Gas.ProtoNitrate, producedAmount * 0.5f);

        var energyReleased = producedAmount * Atmospherics.ProtoNitrateHydrogenConversionEnergy;

        var heatCap = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (heatCap > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = Math.Max((mixture.Temperature * heatCap + energyReleased) / heatCap, Atmospherics.TCMB);

        return ReactionResult.Reacting;
    }
}