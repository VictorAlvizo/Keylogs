using System;

namespace KeyLogs
{
    class KeyFilter //Needed for idenifiying any special keys for program using this
    {
        public enum SpecialKeys
        {
            None,
            Backspace,
            Enter,
            Escape,
            Shift,
            Caps,
            Insert,
            Space,
            Tab,
            Tilde,
            Control,
            Plus,
            Minus,
            LBracket,
            RBracket,
            Slash,
            Question,
            Colon,
            Apost
        }

        public KeyFilter(string filterKey)
        {
            DetermineType(filterKey);
        }

        public SpecialKeys GetKeyType()
        {
            return keyType;
        }

        private void DetermineType(string filterKey)
        {
            switch (filterKey)
            {
                case "Return":
                    keyType = SpecialKeys.Enter;
                    break;

                case "Back":
                    keyType = SpecialKeys.Backspace;
                    break;

                case "Escape":
                    keyType = SpecialKeys.Escape;
                    break;

                case "LShiftKey":
                    keyType = SpecialKeys.Shift;
                    break;

                case "RShiftKey":
                    keyType = SpecialKeys.Shift;
                    break;

                case "Capital":
                    keyType = SpecialKeys.Caps;
                    break;

                case "Insert":
                    keyType = SpecialKeys.Insert;
                    break;

                case "Space":
                    keyType = SpecialKeys.Space;
                    break;

                case "Tab":
                    keyType = SpecialKeys.Tab;
                    break;

                case "Oemtilde":
                    keyType = SpecialKeys.Tilde;
                    break;

                case "LControlKey":
                    keyType = SpecialKeys.Control;
                    break;

                case "RControlKey":
                    keyType = SpecialKeys.Control;
                    break;

                case "Oemplus":
                    keyType = SpecialKeys.Plus;
                    break;

                case "OemMinus":
                    keyType = SpecialKeys.Minus;
                    break;

                case "OemOpenBrackets":
                    keyType = SpecialKeys.LBracket;
                    break;

                case "Oem6":
                    keyType = SpecialKeys.RBracket;
                    break;

                case "Oem5":
                    keyType = SpecialKeys.Slash;
                    break;

                case "OemQuestion":
                    keyType = SpecialKeys.Question;
                    break;

                case "Oem1":
                    keyType = SpecialKeys.Colon;
                    break;

                case "Oem7":
                    keyType = SpecialKeys.Apost;
                    break;

                default:
                    keyType = SpecialKeys.None;
                    break;
            }
        }

        private SpecialKeys keyType;
    }
}
