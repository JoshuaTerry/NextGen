using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Statics
{
    public static class UserMessages
    {
        public static string NameIsRequired => "The {0} name is required";
        public static string CodeIsRequired => "The {0} code is required.";
        public static string MustBeNonBlank => "{0} cannot be blank.";
        public static string MustBeUnique => "{0} must be unique.";
        public static string IsRequired => "{0} is required.";
        public static string CodeMaxLengthError => "The {0} code cannot exceed than {1} characters.";
        public static string CodeAlphaNumericRequired => "The {0} code can only contain letters and numbers";
        public static string CodeIsNotUnique => "The {0} code must be unique.";
        public static string InvalidCode => "Invalid {0} code {1}";
        public static string TranDateInvalid => "A valid transaction date is required.";

    }
}
