// this class was created based on Stack Overflow Website
// Topic: Angular2 & Typescript: How to add string extension method/prototype
// https://stackoverflow.com/questions/43647560/angular2-typescript-how-to-add-string-extension-method-prototype

String.prototype.formatName = function (maximumChar: number = 10): string {
    if (this) {
        const knownAs = this.trim();
        if (knownAs.length > maximumChar) {
            const slicedName = knownAs.substr(0, maximumChar);
            return `${slicedName}...`;
        } else {
            return knownAs;
        }
    } else {
        return '';
    }
};
