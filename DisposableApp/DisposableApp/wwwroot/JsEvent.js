// appel de la fonction c#
async function OnKeyUpEvent(evt) {

    return await DotNet.invokeMethodAsync('Alteva.Blazor.JsEvent', 'OnJsKeyUp', evt.key, evt.keyCode, evt.ctrlKey, evt.shiftKey);
}

// appel de la fonction c#
async function OnKeyDownEvent(evt) {

    return await DotNet.invokeMethodAsync('Alteva.Blazor.JsEvent', 'OnJsKeyDown', evt.key, evt.keyCode, evt.ctrlKey, evt.shiftKey);
}


export function RemoveKeyDownEventHandler(objectId) {
    var obj = GetObjectToListen(objectId);
    if (obj != null) obj.removeEventListener("keydown", KeyDownEventHandler);
}

export function AddKeyDownEventHandler(objectId) {
    var obj = GetObjectToListen(objectId);
    if (obj != null) obj.addEventListener('keydown', KeyDownEventHandler, false);
}

async function KeyDownEventHandler(event) {

    // si CTRL+A on bloque le comportement normal du browser (sinon ça sélectionne tout à l'écran et c'est tres moche)
    if (IsCtrlA(event)) {
        //console.log("preventDefault");
        // event.stopPropagation();
        event.preventDefault();
    }

    //   console.log("keydown");
    let result = await OnKeyDownEvent(event).then(res => {
        if (res) {
            //console.log("resultat ok stopPropagation");
            // inutile car ne fonctionne pas en asynchrone
            //   event.stopPropagation();
            //   event.preventDefault();
        }
        else {
            //console.log("resultat pas ok");
        }
        return res;
    });

    //if (result) {
    //...
    //}


    //   console.log("keydown end");
}




export function RemoveKeyUpEventHandler(objectId) {
    var obj = GetObjectToListen(objectId);
    if (obj != null) obj.removeEventListener("keyup", KeyUpEventHandler);
}

export function AddKeyUpEventHandler(objectId) {
    var obj = GetObjectToListen(objectId);
    if (obj != null) obj.addEventListener('keyup', KeyUpEventHandler, false);
}

async function KeyUpEventHandler(event) {
    if (IsCtrlA(event)) {
        event.preventDefault();
    }
    OnKeyUpEvent(event);
}

// renvoi l'objet à écouter si l'id est fourni ou le Document DOM par défaut
function GetObjectToListen(objectId) {
    var objectEventTmp = null;
    // si la chaine n'est pas vide, on recherche l'élément
    if (objectId) objectEventTmp = document.getElementById(objectId);
    var objControl = objectEventTmp ?? document;

    //if (objectEventTmp != null) console.log(objectId + " détecté");
    //else console.log(objectId + " pas trouvé");

    return objControl;
}

// renvoi true si l'event est CTRL+A
function IsCtrlA(event) {
    return event.ctrlKey && event.keyCode == 65;
}