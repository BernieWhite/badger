// Copyright (c) Bernie White.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Badger
{
    /// <summary>
    /// A JSON converter for reading enums as strings.
    /// </summary>
    internal sealed class StringEnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse(reader.GetString(), true, out TEnum result))
                return result;

            return default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
