// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Content.Client.Storage.Components;
using Content.Shared.Destructible;
using Content.Shared.Foldable;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Movement.Events;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Verbs;
using Robust.Shared.GameStates;

namespace Content.Client.Storage.Systems;

public sealed class EntityStorageSystem : SharedEntityStorageSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<EntityStorageComponent, EntityUnpausedEvent>(OnEntityUnpausedEvent);
        SubscribeLocalEvent<EntityStorageComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<EntityStorageComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<EntityStorageComponent, ActivateInWorldEvent>(OnInteract, after: new[] { typeof(LockSystem) });
        SubscribeLocalEvent<EntityStorageComponent, LockToggleAttemptEvent>(OnLockToggleAttempt);
        SubscribeLocalEvent<EntityStorageComponent, DestructionEventArgs>(OnDestruction);
        SubscribeLocalEvent<EntityStorageComponent, GetVerbsEvent<InteractionVerb>>(AddToggleOpenVerb);
        SubscribeLocalEvent<EntityStorageComponent, ContainerRelayMovementEntityEvent>(OnRelayMovement);
        SubscribeLocalEvent<EntityStorageComponent, FoldAttemptEvent>(OnFoldAttempt);

        SubscribeLocalEvent<EntityStorageComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<EntityStorageComponent, ComponentHandleState>(OnHandleState);
    }

    public override bool ResolveStorage(EntityUid uid, [NotNullWhen(true)] ref SharedEntityStorageComponent? component)
    {
        if (component != null)
            return true;

        TryComp<EntityStorageComponent>(uid, out var storage);
        component = storage;
        return component != null;
    }
}
