// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Plykiya <58439124+Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Map;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Is the specified key within the specified range of us.
/// </summary>
public sealed partial class TargetInRangePrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    private SharedTransformSystem _transformSystem = default!;

    [DataField("targetKey", required: true)] public string TargetKey = default!;

    [DataField("rangeKey", required: true)]
    public string RangeKey = default!;
    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _transformSystem = sysManager.GetEntitySystem<SharedTransformSystem>();
    }

    public override bool IsMet(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<EntityCoordinates>(NPCBlackboard.OwnerCoordinates, out var coordinates, _entManager))
            return false;

        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entManager) ||
            !_entManager.TryGetComponent<TransformComponent>(target, out var targetXform))
            return false;

        var transformSystem = _entManager.System<SharedTransformSystem>;
        return _transformSystem.InRange(coordinates, targetXform.Coordinates, blackboard.GetValueOrDefault<float>(RangeKey, _entManager));
    }
}
