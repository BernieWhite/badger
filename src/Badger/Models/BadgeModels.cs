// Copyright (c) Bernie White.
// Licensed under the MIT License.

using System.ComponentModel;

namespace Badger.Models
{
    public sealed class BadgeCreate
    {
        [DefaultValue(BadgeStatus.Unknown)]
        public BadgeStatus Status { get; set; }

        public string Label { get; set; }
    }

    public enum BadgeStatus
    {
        Unknown,

        Success,

        Error,

        Pending
    }
}
