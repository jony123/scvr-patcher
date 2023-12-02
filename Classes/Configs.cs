﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'System.Text.Json' then do:
//
//    using Bluscream;
//
//    var hmdConfigs = HmdConfigs.FromJson(jsonString);
#nullable enable
#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8603

namespace SCVRPatcher {
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;

    public partial class HmdConfig {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Headset Name")]
        public virtual string HeadsetName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("DB-UID")]
        public virtual long? DbUid { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("All Possible Lens Configurations")]
        public virtual List<string> AllPossibleLensConfigurations { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Hz")]
        public virtual string Hz { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("SC Attributes FOV")]
        public virtual long? ScAttributesFov { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Error Report (SC FOV Cap 120)")]
        public virtual string ErrorReportScFovCap120 { get; set; } // List<ErrorReportScFovCap120>

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Suggested Minimum VorpX Pixel 1:1 Zoom")]
        public virtual double? SuggestedMinimumVorpXPixel11Zoom { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Suggested Maximum VorpX Pixel 1:1 Zoom")]
        public virtual double? SuggestedMaximumVorpXPixel11Zoom { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("VorpX Config Pixel 1:1 Zoom")]
        public virtual string? VorpXConfigPixel11Zoom { get; set; } // VorpXConfigPixel11Zoom?

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Custom Resolution List")]
        public virtual List<string> CustomResolutionList { get; set; } = new();
    }

    public enum ErrorReportScFovCap120 { ScCanTNativelyRunYourHeadsetSFov, UseVorpXZoomFunction };

    public enum VorpXConfigPixel11Zoom { WAwAwAw, YouWillNeedToFindThisSetting };

    public partial class HmdConfigs {
        public static Dictionary<string, HmdConfig> FromJson(string json) => JsonSerializer.Deserialize<Dictionary<string, HmdConfig>>(json, Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this Dictionary<string, HmdConfig> self) => JsonSerializer.Serialize(self, Converter.Settings);
    }

    internal static class Converter {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General) {
            Converters =
            {
                ErrorReportScFovCap120Converter.Singleton,
                VorpXConfigPixel11ZoomConverter.Singleton,
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            },
        };
    }

    internal class ErrorReportScFovCap120Converter : JsonConverter<ErrorReportScFovCap120> {
        public override bool CanConvert(Type t) => t == typeof(ErrorReportScFovCap120);

        public override ErrorReportScFovCap120 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString();
            switch (value) {
                case "SC can't natively run your headset's FOV":
                    return ErrorReportScFovCap120.ScCanTNativelyRunYourHeadsetSFov;
                case "Use VorpX Zoom Function":
                    return ErrorReportScFovCap120.UseVorpXZoomFunction;
            }
            throw new Exception("Cannot unmarshal type ErrorReportScFovCap120");
        }

        public override void Write(Utf8JsonWriter writer, ErrorReportScFovCap120 value, JsonSerializerOptions options) {
            switch (value) {
                case ErrorReportScFovCap120.ScCanTNativelyRunYourHeadsetSFov:
                    JsonSerializer.Serialize(writer, "SC can't natively run your headset's FOV", options);
                    return;
                case ErrorReportScFovCap120.UseVorpXZoomFunction:
                    JsonSerializer.Serialize(writer, "Use VorpX Zoom Function", options);
                    return;
            }
            throw new Exception("Cannot marshal type ErrorReportScFovCap120");
        }

        public static readonly ErrorReportScFovCap120Converter Singleton = new ErrorReportScFovCap120Converter();
    }

    internal class VorpXConfigPixel11ZoomConverter : JsonConverter<VorpXConfigPixel11Zoom> {
        public override bool CanConvert(Type t) => t == typeof(VorpXConfigPixel11Zoom);

        public override VorpXConfigPixel11Zoom Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString();
            switch (value) {
                case "You will need to find this setting":
                    return VorpXConfigPixel11Zoom.YouWillNeedToFindThisSetting;
                case "w Aw aw aw":
                    return VorpXConfigPixel11Zoom.WAwAwAw;
            }
            throw new Exception("Cannot unmarshal type VorpXConfigPixel11Zoom");
        }

        public override void Write(Utf8JsonWriter writer, VorpXConfigPixel11Zoom value, JsonSerializerOptions options) {
            switch (value) {
                case VorpXConfigPixel11Zoom.YouWillNeedToFindThisSetting:
                    JsonSerializer.Serialize(writer, "You will need to find this setting", options);
                    return;
                case VorpXConfigPixel11Zoom.WAwAwAw:
                    JsonSerializer.Serialize(writer, "w Aw aw aw", options);
                    return;
            }
            throw new Exception("Cannot marshal type VorpXConfigPixel11Zoom");
        }

        public static readonly VorpXConfigPixel11ZoomConverter Singleton = new VorpXConfigPixel11ZoomConverter();
    }

    public class DateOnlyConverter : JsonConverter<DateOnly> {
        private readonly string serializationFormat;
        public DateOnlyConverter() : this(null) { }

        public DateOnlyConverter(string? serializationFormat) {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }

    public class TimeOnlyConverter : JsonConverter<TimeOnly> {
        private readonly string serializationFormat;

        public TimeOnlyConverter() : this(null) { }

        public TimeOnlyConverter(string? serializationFormat) {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }

    internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset> {
        public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private string? _dateTimeFormat;
        private CultureInfo? _culture;

        public DateTimeStyles DateTimeStyles {
            get => _dateTimeStyles;
            set => _dateTimeStyles = value;
        }

        public string? DateTimeFormat {
            get => _dateTimeFormat ?? string.Empty;
            set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
        }

        public CultureInfo Culture {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) {
            string text;


            if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal) {
                value = value.ToUniversalTime();
            }

            text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

            writer.WriteStringValue(text);
        }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string? dateText = reader.GetString();

            if (string.IsNullOrEmpty(dateText) == false) {
                if (!string.IsNullOrEmpty(_dateTimeFormat)) {
                    return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
                } else {
                    return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
                }
            } else {
                return default(DateTimeOffset);
            }
        }


        public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
    }
}
#pragma warning restore CS8618
#pragma warning restore CS8601
#pragma warning restore CS8603

