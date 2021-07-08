﻿using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Obsidian.API;
using Obsidian.Utilities;
using Obsidian.Utilities.Converters;
using Obsidian.Utilities.Registry.Enums;
using System.Collections.Generic;
using System.Net.Http;

namespace Obsidian
{
    public static class Globals
    {
        [Obsolete("Inject HttpClient or HttpClientBuilder instead")]
        public static HttpClient HttpClient { get; } = new HttpClient();
        
        public static XorshiftRandom Random { get; } = XorshiftRandom.Create();

        [Obsolete("Inject IOptions<GlobalConfig> instead.")]
        public static GlobalConfig Config { get; set; }
        
        [Obsolete("Inject an ILogger<T> for the class, then using logger.BeginScope(\"Packets\")")]
        public static ILogger PacketLogger { get; set; }
        
        [Obsolete]
        public static DefaultContractResolver ContractResolver { get; } = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        [Obsolete("Inject IOptions<JsonSerializerSettings> instead.")]
        public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings
        {
            ContractResolver = ContractResolver,
            Converters = new List<JsonConverter>
            {
                new DefaultEnumConverter<CustomDirection>(),
                new DefaultEnumConverter<Axis>(),
                new DefaultEnumConverter<Face>(),
                new DefaultEnumConverter<BlockFace>(),
                new DefaultEnumConverter<EHalf>(),
                new DefaultEnumConverter<Hinge>(),
                new DefaultEnumConverter<Instruments>(),
                new DefaultEnumConverter<Part>(),
                new DefaultEnumConverter<Shape>(),
                new DefaultEnumConverter<MinecraftType>(),
                new DefaultEnumConverter<Attachment>(),
            }
        };
    }
}
