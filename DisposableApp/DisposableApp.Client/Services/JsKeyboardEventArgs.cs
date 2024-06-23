using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteva.Blazor.JsEvent.Model
{
    /// <summary>
    /// extrait de Microsoft.AspNetCore.Components.Web.KeyboardEventArgs
    /// </summary>
    public class JsKeyboardEventArgs : EventArgs
    {
        /// <summary>
        /// The key value of the key represented by the event.
        /// If the value has a printed representation, this attribute's value is the same as the char attribute.
        /// Otherwise, it's one of the key value strings specified in 'Key values'.
        /// If the key can't be identified, this is the string "Unidentified"
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// Holds a string that identifies the physical key being pressed.
        /// The value is not affected by the current keyboard layout or modifier state, so a particular key will always return the same value.
        /// </summary>
        public ConsoleKey KeyCode { get; set; }

        /// <summary>
        /// true if the control key was down when the event was fired. false otherwise.
        /// </summary>
        public bool CtrlKey { get; set; }

        /// <summary>
        /// true if the shift key was down when the event was fired. false otherwise.
        /// </summary>
        public bool ShiftKey { get; set; }

        /// <summary>
        /// est ce que la combinaison de touche CTRL+A est effectué ?
        /// </summary>
        public bool CtrlA {  get { return CtrlKey && KeyCode == ConsoleKey.A; } }

        /// <summary>
        /// est ce que la touche "Entrée" est effectué ?
        /// </summary>
        public bool Enter { get { return KeyCode == ConsoleKey.Enter; } }
    }
}
