﻿namespace DDI.Shared.Statics.Common
{
    public static class AddressStrings
    {
        public const string ApartmentAbbreviation = "APT";
        public const string ApartmentRegex = @"(?i)\b((?:AP(?:(ARTMENT)|(T)))(?:((\s*[.#]\s*)|(\s*)))|(?:\s*[#]\s*))";
        public const string BoxRegexGroupName = "box";
        public const string HighwayContractAbbreviation = "HC";
        public const string HighwayContractRegex = @"(?i)\b(?:h((?:wy)|(?:ighway)|(?:\s*)))\s*(?:c((?:ontract)|(?:\s*)))(?<street>.*)?\s*b((?:[o0]x)?|b[o0]x)\s*(?<box>\S+$)";
        public const string MilitaryBoxRegex = @"(?i)\b(?<military>psc|cmr|unit)(?<street>.*)?\s*b((?:[o0]x)?|b[o0]x)\s*(?<box>\S+$)";
        public const string MilitaryRegexGroupName = "military";
        public const string POBoxAbbreviation = "PO BOX";
        public const string POBoxRegex = @"(?i)\b(?:p(?:ost)?\.?\s*[o0](?:ffice)?\.?\s*b(?:[o0]x)?|b[o0]x)";
        public const string RuralRouteAbbreviation = "RR";
        public const string RuralRouteRegex = @"(?i)\b(?:r((?:t)|(?:ural)|(?:\s*)))\s*(?:(r((?:t)|(?:oute)|(?:\s*)))|(t)|(\s*))(?<street>.*)?\s*b((?:[o0]x)?|b[o0]x)\s*(?<box>\S+$)";
        public const string StreetRegexGroupName = "street";
    }
}
