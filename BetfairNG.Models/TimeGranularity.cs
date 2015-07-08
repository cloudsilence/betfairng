﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetfairNG.Models
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum TimeGranularity
    {
        DAYS,
        HOURS,
        MINUTES
    }
}