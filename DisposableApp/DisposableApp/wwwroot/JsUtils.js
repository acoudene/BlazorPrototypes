//************************
//  get the window dimension or size
export function getWindowDimensions() {
    //console.log("height=" + window.innerHeight);
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
}

//************************
//  get the window dimension or size
export function getElementDimensions(elementId) {
    //console.log("elementId=" + elementId);
    var elem = document.getElementById(elementId);
    if (elem) {
        var rect = elem.getBoundingClientRect();
        console.log(rect);
        return rect;
    }
    return null;
}

//************************
// ouvre une url dans un nouvel onglet
export function openUrl(url) {
    // noreferrer permet de ne pas bloquer l'appli blazor. Sans ça elle est bloqué lorsqu'on ouvre un nouvel onglet
    //According to the documentation: (https://developer.mozilla.org/en-US/docs/Web/API/Window/open#Window_features) window.open() currently supports features. 
    //Hence we can pass noreferrer or noopener as shown below, which does not freeze or block the parent window.
    window.open(url, "_blank", "noreferrer");
}


//************************
// donne le focus à un elementId
export function focusInput(elementId) {
    var elt = document.getElementById(elementId);
    if (elt != null) elt.focus();
}



export function GetWidthByElementId(elementId) {
    if (!elementId) return 0;
    let elt = document.getElementById(elementId);
    if (elt == null) return 0;
    return elt.clientWidth;
}


export function GetHeightByElementId(elementId) {
    if (!elementId) return 0;
    let elt = document.getElementById(elementId);
    if (elt == null) return 0;
    return elt.clientHeight;
}


export function GetLeftPositionByElementId(elementId) {
    if (!elementId) return 0;
    let elt = document.getElementById(elementId);
    if (elt == null) return 0;
    return elt.getBoundingClientRect().left;
}


export function GetBottomPositionByElementId(elementId) {
    if (!elementId) return 0;
    let elt = document.getElementById(elementId);
    if (elt == null) return 0;
    return elt.getBoundingClientRect().bottom;
}

// simule un click sur un élément du DOM
export function SimulateClickByElementId(elementId) {
    if (!elementId) return;
    let elt = document.getElementById(elementId);
    if (elt == null) return;
    return elt.click();
}

/// <summary>
/// surcharge les liens d'un texte en html pour qu'ils s'affichent dans un autre onglet
/// en ajoutant target="_blank" pour chaque liens
/// </summary>
export function addTargetBlankToLinks(htmlString) {
    // Crée un objet DOM à partir de la chaîne HTML
    var range = document.createRange();
    var fragment = range.createContextualFragment(htmlString);

    // Crée un nouvel élément div
    var container = document.createElement('div');

    // Ajoute le DocumentFragment au div
    container.appendChild(fragment);

    // Sélectionne tous les éléments "a" à l'intérieur du div
    var links = container.querySelectorAll('a');

    // Parcoure tous les liens et ajoute l'attribut "target=_blank"
    links.forEach(function (link) {
        link.setAttribute('target', '_blank');
    });

    // Retourne la version modifiée en tant que chaîne HTML
    return container.innerHTML;
}

// enregistrer un fichier
// démo syncfusion : https://blazor.syncfusion.com/demos/excel/create-excel?theme=fluent
// source syncfusion : https://github.com/syncfusion/blazor-samples/blob/master/Common/wwwroot/scripts/common/index.js
export function saveAsFile(filename, bytesBase64) {
    if (window.navigator.msSaveBlob) {
        //Download document in Edge browser
        var data = window.atob(bytesBase64);
        var bytes = new Uint8Array(data.length);
        for (var i = 0; i < data.length;) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
        window.navigator.msSaveBlob(blob, filename);
    }
    else {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    }
}