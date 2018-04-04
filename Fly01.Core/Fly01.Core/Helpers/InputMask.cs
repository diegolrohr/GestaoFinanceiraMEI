using System;

namespace Fly01.Core.Helpers
{
    public class InputMask : Attribute
    {
        public InputMask(string maskType, string format = null, int maxlength = 0)
        {
            MaskType = maskType;
            Format = format;
            Maxlength = maxlength;
        }

        public string MaskType { get; set; }
        public string Format { get; set; }
        public int Maxlength { get; set; }
    }
}
