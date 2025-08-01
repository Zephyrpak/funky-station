// SPDX-FileCopyrightText: 2022 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Errant <35878406+dmnct@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jackson <8786660+jacksonzck@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Justin Trotter <trotter.justin@gmail.com>
// SPDX-FileCopyrightText: 2022 Júlio César Ueti <52474532+Mirino97@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Moony <moonheart08@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Morbo <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 SpaceManiac <tad@platymuus.com>
// SPDX-FileCopyrightText: 2022 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2024 SlamBamActionman <83650252+SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Radio.EntitySystems;
using Content.Shared.Radio;
using Content.Shared.Salvage;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Server.Chat.Managers;
using Content.Server.Gravity;
using Content.Server.Parallax;
using Content.Server.Procedural;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Construction.EntitySystems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;
using Content.Server.Labels;
using Robust.Shared.EntitySerialization.Systems;

namespace Content.Server.Salvage
{
    public sealed partial class SalvageSystem : SharedSalvageSystem
    {
        [Dependency] private readonly IChatManager _chat = default!;
        [Dependency] private readonly IConfigurationManager _configurationManager = default!;
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly ILogManager _logManager = default!;
        [Dependency] private readonly IMapManager _mapManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly AnchorableSystem _anchorable = default!;
        [Dependency] private readonly BiomeSystem _biome = default!;
        [Dependency] private readonly DungeonSystem _dungeon = default!;
        [Dependency] private readonly GravitySystem _gravity = default!;
        [Dependency] private readonly LabelSystem _labelSystem = default!;
        [Dependency] private readonly MapLoaderSystem _loader = default!;
        [Dependency] private readonly MetaDataSystem _metaData = default!;
        [Dependency] private readonly RadioSystem _radioSystem = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly SharedTransformSystem _transform = default!;
        [Dependency] private readonly SharedMapSystem _mapSystem = default!;
        [Dependency] private readonly ShuttleSystem _shuttle = default!;
        [Dependency] private readonly ShuttleConsoleSystem _shuttleConsoles = default!;
        [Dependency] private readonly StationSystem _station = default!;
        [Dependency] private readonly UserInterfaceSystem _ui = default!;

        private EntityQuery<MapGridComponent> _gridQuery;
        private EntityQuery<TransformComponent> _xformQuery;

        public override void Initialize()
        {
            base.Initialize();

            _gridQuery = GetEntityQuery<MapGridComponent>();
            _xformQuery = GetEntityQuery<TransformComponent>();

            InitializeExpeditions();
            InitializeMagnet();
            InitializeRunner();
        }

        private void Report(EntityUid source, string channelName, string messageKey, params (string, object)[] args)
        {
            var message = args.Length == 0 ? Loc.GetString(messageKey) : Loc.GetString(messageKey, args);
            var channel = _prototypeManager.Index<RadioChannelPrototype>(channelName);
            _radioSystem.SendRadioMessage(source, message, channel, source);
        }

        public override void Update(float frameTime)
        {
            UpdateExpeditions();
            UpdateMagnet();
            UpdateRunner();
        }
    }
}

