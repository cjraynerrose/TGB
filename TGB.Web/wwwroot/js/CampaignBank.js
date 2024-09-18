export var easyMDE;
export function createEasyMde() {
    easyMDE = new EasyMDE({
        element: document.querySelector('#focusRecordDesc'),
        theme: null
    });
}

export function removeEasyMde() {
    if (easyMDE !== null) {
        easyMDE.toTextArea();
        easyMDE = null;
    }
}

export function easyMdeSetValue(value) {
    removeEasyMde();
    createEasyMde();
    easyMDE.value = value;
}