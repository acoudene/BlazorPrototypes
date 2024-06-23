using Alteva.Blazor.JsEvent.Helpers;
using Alteva.Blazor.JsEvent.Model;
using Microsoft.JSInterop;

namespace Alteva.Blazor.JsEvent.Services
{
    /// <summary>
    /// Service permettant "d'écouter" certains evenements javascript
    /// </summary>
    public class JsEventService : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public JsEventService(IJSRuntime jsRuntime)
        {
            moduleTask = Helper.GetJavascriptModule(jsRuntime, "./JsEvent.js");
        }

        /// <summary>
        /// écouter les evenements keyDown et keyUp
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask AddKeyEventHandler(string objectId = "")
        {
            await AddKeyDownEventHandler(objectId);
            await AddKeyUpEventHandler(objectId);
        }

        /// <summary>
        /// ne plus écouter les evenements keyDown et keyUp
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask RemoveKeyEventHandler(string objectId = "")
        {
            await RemoveKeyDownEventHandler(objectId);
            await RemoveKeyUpEventHandler(objectId);
        }

        /// <summary>
        /// écouter l'evenement keyDown
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask AddKeyDownEventHandler(string objectId = "")
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("AddKeyDownEventHandler", objectId);
        }

        /// <summary>
        /// ne plus écouter l'evenement keyDown
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask RemoveKeyDownEventHandler(string objectId = "")
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("RemoveKeyDownEventHandler", objectId);
        }

        /// <summary>
        ///  écouter l'evenement keyUp
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask AddKeyUpEventHandler(string objectId = "")
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("AddKeyUpEventHandler", objectId);
        }

        /// <summary>
        /// ne plus écouter l'evenement keyUp
        /// </summary>
        /// <param name="objectId">Id de l'élément àécouter. Si null, l'écoute sur fera sur le Document du DOM</param>
        /// <returns></returns>
        public async ValueTask RemoveKeyUpEventHandler(string objectId = "")
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("RemoveKeyUpEventHandler", objectId);
        }

        /// <summary>
        /// evenement à appeler dans le client pour intéragir quand un keydown est effectué
        /// rq : la fonction renvoi un bool en retour. Il était destiné au JS pour pouvoir stopper la propagation de l'evenent si besoin mais ça ne fonctionne pas en asynchrone.
        /// Je laisse le code tel quel parce que ça ne mange pas de pain et sur le coup je n'ai pas trop de temps.
        /// rq2 : vu que le bool de retour ne sert à rien, on aurait pu écrire cet evenement différement :
        /// public static event EventHandler<JsKeyboardEventArgs> OnKeyDown;
        /// </summary>
        public static event Func<JsKeyboardEventArgs, bool>? OnKeyDown;

        /// <summary>
        /// evenement à appeler dans le client pour intéragir quand un keydown est effectué
        /// rq : la fonction renvoi un bool en retour. Il était destiné au JS pour pouvoir stopper la propagation de l'evenent si besoin mais ça ne fonctionne pas en asynchrone.
        /// Je laisse le code tel quel parce que ça ne mange pas de pain et sur le coup je n'ai pas trop de temps.
        /// rq2 : vu que le bool de retour ne sert à rien, on aurait pu écrire cet evenement différement :
        /// public static event EventHandler<JsKeyboardEventArgs> OnKeyDown;
        /// </summary>
        public static event Func<JsKeyboardEventArgs, bool>? OnKeyUp;

        /// <summary>
        /// est ce que la touche shift est enfoncée ?
        /// (à vérifier : ne devrait fonctionner que si AddKeyEventHandler est utilisé)
        /// </summary>
        public static bool ShiftKey = false;

        /// <summary>
        /// appelé par JavaScript lorsqu'un evenement Key Down est détecté
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyCode"></param>
        /// <param name="ctrlKey"></param>
        /// <param name="shiftKey"></param>
        /// <returns></returns>
        [JSInvokable]
        public async static Task<bool> OnJsKeyDown(string key, int keyCode, bool ctrlKey, bool shiftKey)
        {
            if (OnKeyDown == null) return false;

            ShiftKey = shiftKey;
            try
            {
                var consoleKey = (ConsoleKey)keyCode;
                var arg = new JsKeyboardEventArgs() { Key = key, KeyCode = consoleKey, CtrlKey = ctrlKey, ShiftKey = shiftKey };
                var res = OnKeyDown.Invoke(arg);
                return await Task.FromResult(res);
            }
            catch
            {
                Console.WriteLine($"Cound not find {nameof(ConsoleKey)} for JS key value {keyCode})");
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// appelé par JavaScript lorsqu'un evenement Key Up est détecté
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyCode"></param>
        /// <param name="ctrlKey"></param>
        /// <param name="shiftKey"></param>
        /// <returns></returns>
        [JSInvokable]
        public async static Task<bool> OnJsKeyUp(string key, int keyCode, bool ctrlKey, bool shiftKey)
        {
            if (OnKeyUp == null) return false;

            ShiftKey = shiftKey;
            try
            {
                var consoleKey = (ConsoleKey)keyCode;
                var arg = new JsKeyboardEventArgs() { Key = key, KeyCode = consoleKey, CtrlKey = ctrlKey, ShiftKey = shiftKey };
                var res = OnKeyUp.Invoke(arg);
                return await Task.FromResult(res);
            }
            catch
            {
                Console.WriteLine($"Cound not find {nameof(ConsoleKey)} for JS key value {keyCode})");
            }

            return await Task.FromResult(false);
        }

        #region IAsyncDisposable        
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await RemoveKeyEventHandler();

            if (moduleTask is not null && moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                if (module is not null)
                {
                    await module.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
        #endregion
    }
}