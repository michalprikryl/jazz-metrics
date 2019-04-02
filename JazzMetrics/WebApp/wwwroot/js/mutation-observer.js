/**
 * trida pro sledovani zmen trid nad danym elementem DOM
 * @param {Array} mutationsList list zmen nad elementem
 * */
class ClassWatcher {
    /**
     * konstruktor tridy pro sledovani zmen nad elementem DOM
     * @param {any} targetNode element DOM
     * @param {any} classToWatch trida, kterou hlidat
     * @param {any} classAddedCallback kdyz bude trida pridana, tak se spusti
     * @param {any} classRemovedCallback kdyz bude trida odebrana, tak se spusti
     */
    constructor(targetNode, classToWatch, classAddedCallback, classRemovedCallback) {
        this.targetNode = targetNode;
        this.classToWatch = classToWatch;
        this.classAddedCallback = classAddedCallback;
        this.classRemovedCallback = classRemovedCallback;
        this.observer = null;
        this.lastClassState = targetNode.classList.contains(this.classToWatch);

        this.init();
    }

    /**
     * inicializace
     * */
    init() {
        this.observer = new MutationObserver(this.mutationCallback);
        this.observe();
    }

    /**
     * pocatek naslouchani
     * */
    observe() {
        this.observer.observe(this.targetNode, { attributes: true });
    }

    /**
     * odpojeni od naslouchani zmenam
     * */
    disconnect() {
        this.observer.disconnect();
    }

    /**
     * callback vyvolany pri zmene
     * @param {Array} mutationsList list zmen nad elementem
     * */
    mutationCallback = mutationsList => {
        for (let mutation of mutationsList) {
            if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                let currentClassState = mutation.target.classList.contains(this.classToWatch);
                if (this.lastClassState !== currentClassState) {
                    this.lastClassState = currentClassState;
                    if (currentClassState) {
                        this.classAddedCallback(mutation.target);
                    }
                    else {
                        this.classRemovedCallback(mutation.target);
                    }
                }
            }
        }
    }
}